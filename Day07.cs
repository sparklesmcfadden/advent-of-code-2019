using System.Linq;
using System;
using System.Collections.Generic;

class Day07_AmplificationCircuit
{
    private const int startInput = 0;
    private IntCodeComputer _ampA;
    private IntCodeComputer _ampB;
    private IntCodeComputer _ampC;
    private IntCodeComputer _ampD;
    private IntCodeComputer _ampE;
    private int _highestOutput;
    private readonly string _path;

    public Day07_AmplificationCircuit(string path)
    {
        _path = path;
        _ampA = new IntCodeComputer(_path);
        _ampB = new IntCodeComputer(_path);
        _ampC = new IntCodeComputer(_path);
        _ampD = new IntCodeComputer(_path);
        _ampE = new IntCodeComputer(_path);
        _highestOutput = 0;
    }

    public Day07_AmplificationCircuit(string program, bool directLoad)
    {
        _ampA = new IntCodeComputer(program, directLoad);
        _ampB = new IntCodeComputer(program, directLoad);
        _ampC = new IntCodeComputer(program, directLoad);
        _ampD = new IntCodeComputer(program, directLoad);
        _ampE = new IntCodeComputer(program, directLoad);
    }

    private void Reset()
    {
        _ampA = new IntCodeComputer(_path);
        _ampB = new IntCodeComputer(_path);
        _ampC = new IntCodeComputer(_path);
        _ampD = new IntCodeComputer(_path);
        _ampE = new IntCodeComputer(_path);
    }

    public int FindHighestOutput()
    {
        var phaseInputs = GetAllPhaseInputs();
        foreach (var input in phaseInputs)
        {
            var output = RunPhaseSet(input);
            Console.WriteLine(output);
            if (output > _highestOutput) _highestOutput = output;
            Reset();
        }
        return _highestOutput;
    }

    public int RunPhaseInput(int[] phaseInput)
    {
        return RunPhaseSet(phaseInput);
    }

    private List<int[]> GetAllPhaseInputs()
    {
        var allInputs = new List<int[]>();
        for (int i = 0; i < 100000; i++)
        {
            var phaseStr = i.ToString("00000");
            var phaseInput = phaseStr.ToCharArray().ToList().Select(c => Convert.ToInt32(c.ToString())).ToArray();
            if (phaseInput.Any(i => i > 4)) continue;
            allInputs.Add(phaseInput);
        }

        return allInputs;
    }

    private int RunPhaseSet(int[] phaseInput)
    {
        _ampA.RunProgram(new int[] {phaseInput[0], startInput});
        _ampB.RunProgram(new int[] {phaseInput[1], _ampA.output});
        _ampC.RunProgram(new int[] {phaseInput[2], _ampB.output});
        _ampD.RunProgram(new int[] {phaseInput[3], _ampC.output});
        _ampE.RunProgram(new int[] {phaseInput[4], _ampD.output});

        return _ampE.output;
    }
}
