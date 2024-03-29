using System;
using System.Collections.Generic;
using System.Linq;

class Day07_Part2
{
    private List<IntCodeComputer> Processors = new List<IntCodeComputer>();
    private readonly List<long> _program;
    public long HighestOutput = 0;

    public Day07_Part2(string path)
    {
        _program = Utilities.LoadProgram(path);
    }

    public Day07_Part2(List<long> program)
    {
        _program = program;
    }

    private bool ShouldHalt()
    {
        if (Processors.Any(p => p.OpCode == 99)) return true;
        return false;
    }

    private List<long[]> GetPhases()
    {
        var initialPhases = Utilities.GetPhaseInputs(55555, 99999);
        return initialPhases.Where(p => !p.Any(i => i < 5)).ToList();
    }

    public long GetHighestLoopOutput()
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

    public long RunProgram(long[] phasing)
    {
        Processors = new List<IntCodeComputer>();

        for (int i = 0; i < phasing.Count(); i++)
        {
            Processors.Add(new IntCodeComputer(Utilities.Clone(_program), i));
        }

        var firstLoop = true;
        var lastOutput = 0;
        while (!ShouldHalt())
        {
            var input = new Queue<long>();
            for (int i = 0; i < Processors.Count; i++)
            {
                var processor = Processors[i];
                if (firstLoop)
                {
                    input.Enqueue(phasing[i]);
                }
                input.Enqueue(lastOutput);
                processor.Resume();
                processor.RunProgram(input);
                lastOutput = (int)processor.Output;
            }
            input.Enqueue(lastOutput);
            firstLoop = false;
        }
        
        return lastOutput;
    }
}