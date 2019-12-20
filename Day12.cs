using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Day12
{
    public List<Moon> Moons = new List<Moon>();
    public List<string> UniverseHistory = new List<string>();

    public class Moon
    {
        public int MoonId { get; set; }
        public List<int> Position { get; set; } = new List<int> { 0, 0, 0 };
        public List<int> Velocity { get; set; } = new List<int> { 0, 0, 0 };
        public List<int> InitialPosition { get; set; } = new List<int> { 0, 0, 0, };
        public List<int> InitialVelocity { get; set; } = new List<int> { 0, 0, 0, };
        public List<int> CycleCount = new List<int> { 0, 0, 0 };
        public int Cycles { get; set; } = 0;
        public bool IsInitialX => Position[0] == InitialPosition[0] && Velocity[0] == 0;
        public bool IsInitialY => Position[1] == InitialPosition[1] && Velocity[1] == 0;
        public bool IsInitialZ => Position[2] == InitialPosition[2] && Velocity[2] == 0;
        public bool IsInitialPostion => IsInitialX && IsInitialY && IsInitialZ;
        private static int A(int n) => Math.Abs(n);
        public int KineticEnergy => A(Position[0]) + A(Position[1]) + A(Position[2]);
        public int PotentialEnergy => A(Velocity[0]) + A(Velocity[1]) + A(Velocity[2]);
        public int TotalEnergy => KineticEnergy * PotentialEnergy;
        public string StateString() => $"pos=<x={Position[0]}, y={Position[1]}, z={Position[2]}>, vel=<x={Velocity[0]}, y={Velocity[1]}, z={Velocity[2]}>";
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

        Console.WriteLine($"Day 12: Part 1: Total Energy after {timeSteps} steps: {Moons.Sum(m => m.TotalEnergy)}");
    }

    public void Part2(string path)
    {
        LoadInitialScan(path);
        var repeatTime = RunUniverseForever();

        Console.WriteLine($"Day 12: Part 2: Time to reset: {repeatTime}");
    }

    public void Part2Test(string scan)
    {
        LoadFromString(scan);
        var repeatTime = RunUniverseForever();

        Console.WriteLine($"Day 12: Part 2: Time to reset: {repeatTime}");
    }

    public void Part1Test(string scan, int timeSteps)
    {
        LoadFromString(scan);
        CalculatePositionAfterTime(timeSteps);

        Console.WriteLine($"Day 12: Part 1: Total Energy after {timeSteps} steps: {Moons.Sum(m => m.TotalEnergy)}");
    }

    public void LoadInitialScan(string path)
    {
        var inputScan = File.ReadAllLines(path);
        LoadMoons(inputScan);
    }

    public void LoadFromString(string inputString)
    {
        var inputScan = inputString.Split("|");
        LoadMoons(inputScan);
    }

    public void LoadMoons(string[] inputScan)
    {        
        var counter = 1;
        foreach (var line in inputScan)
        {
            var initialValues = ParseLine(line);
            var moon = new Moon { MoonId = counter, Position = new List<int> { initialValues[0], initialValues[1], initialValues[2] }};
            moon.InitialPosition = new List<int> { initialValues[0], initialValues[1], initialValues[2] };
            Moons.Add(moon);
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
            }
            if (moonA.Position[i] < moonB.Position[i])
            {
                moonA.Velocity[i]++;
            }
        }
    }

    public void CalculatePositionAfterTime(int timeSteps)
    {
        for (int i = 0; i < timeSteps; i++)
        {
            RunSimulationStep(i);
        }
    }

    public long RunUniverseForever()
    {
        var allHaveRepeated = false;
        var cycleCount = 0;
        while (!allHaveRepeated)
        {
            cycleCount++;
            RunSimulationStep(cycleCount);
            if (Moons.All(m => m.CycleCount.All(c => c != 0))) allHaveRepeated = true;
        }

        var repeatCycles = new List<long>();
        Moons.ForEach(m => {
            var lcm = Utilities.LCM(m.CycleCount[2], Utilities.LCM(m.CycleCount[1], m.CycleCount[0]));
            repeatCycles.Add(lcm);
        });
        long prevLCM = repeatCycles[0];

        for (int i = 1; i < repeatCycles.Count; i++)
        {
            prevLCM = Utilities.LCM(repeatCycles[i], prevLCM);
        }

        return prevLCM;
    }

    private void RunSimulationStep(int cycleCount)
    {
        foreach (var moon in Moons)
        {
            var otherMoons = Moons.Where(m => m.MoonId != moon.MoonId);
            foreach(var otherMoon in otherMoons)
            {
                InteractMoons(moon, otherMoon);
            }
        }
        foreach (var moon in Moons)
        {
            ApplyVelocity(moon);
        }
        if (Moons.All(m => m.IsInitialX && m.CycleCount[0] == 0)) 
        {
            Moons.ForEach(m => m.CycleCount[0] = cycleCount);
        }
        if (Moons.All(m => m.IsInitialY && m.CycleCount[1] == 0))
        {
            Moons.ForEach(m => m.CycleCount[1] = cycleCount);
        }
        if (Moons.All(m => m.IsInitialZ && m.CycleCount[2] == 0))
        { 
            Moons.ForEach(m => m.CycleCount[2] = cycleCount);
        }
    }
}