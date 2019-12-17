using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

class Utilities
{
    public class Pixel
    {
        public long X { get; set; }
        public long Y { get; set; }
        public string Value { get; set; }

        public Pixel(long x, long y, string value)
        {
            X = x;
            Y = y;
            Value = value;
        }
    }

    public static void RenderScreen(List<Pixel> pixels, bool invert = false)
    {        
        var maxX = pixels.Max(p => p.X);
        var minX = pixels.Min(p => p.X);
        var maxY = pixels.Max(p => p.Y);
        var minY = pixels.Min(p => p.Y);

        if (invert)
        {
            for (long y = maxY; y >= minY; y--)
            {
                RenderImageRow(pixels, y, minX, maxX);
            }
        }
        else
        {
            for (long y = minY; y <= maxY; y++)
            {
                RenderImageRow(pixels, y, minX, maxX);
            }
        }

    }

    private static void RenderImageRow(List<Pixel> pixels, long y, long minX, long maxX)
    {
        var imageRow = "";
            for (long x = minX; x <= maxX; x++)
            {
                var panel = pixels.FirstOrDefault(p => p.X == x && p.Y == y);
                if (panel == null)
                {
                    imageRow += " ";
                    continue;
                }
                imageRow += panel.Value;
            }
            Console.WriteLine(imageRow);
    }

    public static List<long[]> GetPhaseInputs(int start, int end)
    {
        var allInputs = new List<long[]>();
        for (int i = start; i <= end; i++)
        {
            var phaseStr = i.ToString("00000");
            var phaseInput = phaseStr.ToCharArray().ToList().Select(c => Convert.ToInt64(c.ToString())).ToArray();
            if (phaseInput.GroupBy(i => i).Any(d => d.Count() > 1)) continue;
            allInputs.Add(phaseInput);
        }

        return allInputs;
    }
    public static List<long> LoadProgram(string path)
    {
        var input = File.ReadAllLines(path);
        return input[0].Split(",").Select(c => Convert.ToInt64(c)).ToList();
    }

    public static string LoadFile(string path)
    {
        var file = File.ReadAllLines(path);
        return file[0];
    }

    public static List<long> LoadProgramFromString(string program)
    {
        return program.Split(",").Select(c => Convert.ToInt64(c)).ToList();
    }
    public static T Clone<T>(T source)
    {
        var serialized = JsonConvert.SerializeObject(source);
        return JsonConvert.DeserializeObject<T>(serialized);
    }

    public static List<long> NumberStringToList(string input)
    {
        return input.ToCharArray().ToList().Select(c => Convert.ToInt64(c.ToString())).ToList();
    }
}