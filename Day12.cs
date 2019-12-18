using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Day12
{
    public Dictionary<int, Moon> Moons = new Dictionary<int, Moon>();

    public class Moon
    {
        public List<int> Position { get; set; } = new List<int> { 0, 0, 0};
        public List<int> Velocity { get; set; } = new List<int> { 0, 0, 0};
    }

    private int[] ParseLine(string inputLine)
    {
        inputLine = inputLine.Replace("<", "").Replace(">", "").Replace(" ", "");
        var parts = inputLine.Split(",");
        var x = Convert.ToInt32(parts.Single(s => s.ToLower().StartsWith("x")).Split("=").Last());
        var y = Convert.ToInt32(parts.Single(s => s.ToLower().StartsWith("y")).Split("=").Last());
        var z = Convert.ToInt32(parts.Single(s => s.ToLower().StartsWith("z")).Split("=").Last());

        return new int[] {x, y, z};
    }

    public void Part1(string path, int timeSteps)
    {
        LoadInitialScan(path);
        CalculatePositionAfterTime(timeSteps);

        Console.WriteLine($"Day 12: Part 1: Total Energy after {timeSteps} steps: {CalculateSystemEnergy()}");
    }

    public void Part1Test(string scan, int timeSteps)
    {
        LoadFromString(scan);
        CalculatePositionAfterTime(timeSteps);

        Console.WriteLine($"Day 12: Part 1: Total Energy after {timeSteps} steps: {CalculateSystemEnergy()}");
    }

    private int CalculateSystemEnergy()
    {        
        var systemEnergy = 0;
        foreach (var moon in Moons)
        {
            systemEnergy += CalculateMoonEnergy(moon.Value);
        }
        return systemEnergy;
    }

    public void LoadInitialScan(string path)
    {
        var inputScan = File.ReadAllLines(path);
        
        var counter = 0;
        foreach (var line in inputScan)
        {
            var initialValues = ParseLine(line);
            var moon = new Moon { Position = new List<int> { initialValues[0], initialValues[1], initialValues[2] }};
            Moons.Add(counter, moon);
            counter++;
        }
    }

    public void LoadFromString(string inputString)
    {
        var inputScan = inputString.Split("|");
        
        var counter = 0;
        foreach (var line in inputScan)
        {
            var initialValues = ParseLine(line);
            var moon = new Moon { Position = new List<int> { initialValues[0], initialValues[1], initialValues[2] }};
            Moons.Add(counter, moon);
            counter++;
        }
    }

    private void ApplyVelocity(Moon moon)
    {
        for (int i = 0; i < 3; i++)
        {
            moon.Position[i] += moon.Velocity[i];
        }
    }

    private void InteractMoons(Moon moonA, Moon moonB)
    {
        for (int i = 0; i < 3; i++)
        {
            if (moonA.Position[i] > moonB.Position[i])
            {
                moonA.Velocity[i]--;
                moonB.Velocity[i]++;
            }
            if (moonA.Position[i] < moonB.Position[i])
            {
                moonA.Velocity[i]++;
                moonB.Velocity[i]--;
            }
        }
    }

    public int CalculateMoonEnergy(Moon moon)
    {
        var potentialEnergy = moon.Position.Sum(p => Math.Abs(p));
        var kineticEnergy = moon.Velocity.Sum(v => Math.Abs(v));

        return potentialEnergy * kineticEnergy;
    }

    public void CalculatePositionAfterTime(int timeSteps)
    {
        for (int i = 0; i < timeSteps; i++)
        {
            foreach (var moonRecord in Moons)
            {
                var moon = moonRecord.Value;
                var otherMoons = Moons.Where(m => m.Key != moonRecord.Key);
                foreach(var otherMoon in otherMoons)
                {
                    InteractMoons(moon, otherMoon.Value);
                }
            }
            foreach (var moon in Moons)
            {
                ApplyVelocity(moon.Value);
            }
            Console.WriteLine(CalculateSystemEnergy());
        }
    }
}