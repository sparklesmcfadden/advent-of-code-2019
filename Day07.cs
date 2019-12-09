using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;

class Day07_AmplificationCircuit
{
    private int startInput = 0;
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

    private bool ShouldHalt()
    {
        if (_ampA.OpCode == 99
            || _ampB.OpCode == 99
            || _ampC.OpCode == 99
            || _ampD.OpCode == 99
            || _ampE.OpCode == 99
        ) return true;
        return false;
    }

    public int RunFeedbackLoop()
    {
        var phaseInputs = GetPart2Phases();
        var loopOutput = 0;

        foreach (var input in phaseInputs)
        {
            var additionalInputs = new List<int>();
            while (!ShouldHalt())
            {
                loopOutput = RunPhaseSetWithExtraInputs(input, additionalInputs);
                additionalInputs.Add(loopOutput);
            }
        }

        return loopOutput;
    }

    public int FindHighestOutput()
    {
        var phaseInputs = GetPart1Phases();
        foreach (var input in phaseInputs)
        {
            var output = RunPhaseSet(input);
            if (output > _highestOutput) _highestOutput = output;
            Reset();
        }
        return _highestOutput;
    }

    public int RunPhaseInput(int[] phaseInput)
    {
        return RunPhaseSet(phaseInput);
    }

    private List<int[]> GetPhaseInputs(int start, int end)
    {
        var allInputs = new List<int[]>();
        for (int i = start; i <= end; i++)
        {
            var phaseStr = i.ToString("00000");
            var phaseInput = phaseStr.ToCharArray().ToList().Select(c => Convert.ToInt32(c.ToString())).ToArray();
            if (phaseInput.GroupBy(i => i).Any(d => d.Count() > 1)) continue;
            allInputs.Add(phaseInput);
        }

        return allInputs;
    }

    private List<int[]> GetPart1Phases()
    {
        var initialPhases = GetPhaseInputs(0, 44444);
        return initialPhases.Where(p => !p.Any(i => i > 4)).ToList();
    }

    private List<int[]> GetPart2Phases()
    {
        var initialPhases = GetPhaseInputs(55555, 99999);
        return initialPhases.Where(p => !p.Any(i => i < 5)).ToList();
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

    private int RunPhaseSetWithExtraInputs(int[] phaseInput, List<int> additionalInputs)
    {
        _ampA.RunProgramWithExtraInputs(new int[] {phaseInput[0], startInput}, additionalInputs);
        _ampB.RunProgram(new int[] {phaseInput[1], _ampA.output});
        _ampC.RunProgram(new int[] {phaseInput[2], _ampB.output});
        _ampD.RunProgram(new int[] {phaseInput[3], _ampC.output});
        _ampE.RunProgram(new int[] {phaseInput[4], _ampD.output});

        return _ampE.output;
    }
}
