using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

class Utilities
{
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
}