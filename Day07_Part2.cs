using System;
using System.Collections.Generic;
using System.Linq;

class Day07_Part2
{
    private List<IntCodeComputer> Processors = new List<IntCodeComputer>();
    private readonly List<int> _program;
    public int HighestOutput = 0;

    public Day07_Part2(string path)
    {
        _program = Utilities.LoadProgram(path);
    }

    public Day07_Part2(List<int> program)
    {
        _program = program;
    }

    private bool ShouldHalt()
    {
        if (Processors.Any(p => p.OpCode == 99)) return true;
        return false;
    }

    private List<int[]> GetPhases()
    {
        var initialPhases = Utilities.GetPhaseInputs(55555, 99999);
        return initialPhases.Where(p => !p.Any(i => i < 5)).ToList();
    }

    public int GetHighestLoopOutput()
    {
        var phases = GetPhases();

        foreach (var phaseSet in phases)
        {
            var result = RunProgram(phaseSet);
            if (result > HighestOutput)
            {
                HighestOutput = result;
            }
        }

        return HighestOutput;
    }

    public int RunProgram(int[] phasing)
    {
        var inputStack = new List<int> {0};
        Processors = new List<IntCodeComputer>();

        for (int i = 0; i < phasing.Count(); i++)
        {
            Processors.Add(new IntCodeComputer(Utilities.Clone(_program), i));
        }

        var lastOutput = 0;
        while (!ShouldHalt())
        {
            for (int i = 0; i < Processors.Count; i++)
            {
                var processor = Processors[i];
                var programInput = new List<int> {phasing[i]};
                programInput.AddRange(inputStack);
                processor.Resume();
                processor.RunProgram(programInput.ToArray());
                lastOutput = processor.Output;
                inputStack[inputStack.Count - 1] = lastOutput;
            }
            inputStack.Add(lastOutput);
        }
        
        return lastOutput;
    }
}