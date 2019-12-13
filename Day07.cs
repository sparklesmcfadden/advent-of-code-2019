using System.Linq;
using System.Collections.Generic;

class Day07_AmplificationCircuit
{
    private int _highestOutput;
    private readonly string _path;
    private readonly string _program;

    public Day07_AmplificationCircuit(string path)
    {
        _path = path;
        _highestOutput = 0;
    }

    public Day07_AmplificationCircuit(string program, bool directLoad)
    {
        _program = program;
        _highestOutput = 0;
    }

    public int FindHighestOutput()
    {
        var phaseInputs = GetPhases();
        foreach (var input in phaseInputs)
        {
            var output = RunPhaseSet(input);
            if (output > _highestOutput) _highestOutput = output;
        }
        return _highestOutput;
    }

    public int RunPhaseInput(long[] phaseInput)
    {
        return RunPhaseSet(phaseInput);
    }

    private List<long[]> GetPhases()
    {
        var initialPhases = Utilities.GetPhaseInputs(0, 44444);
        return initialPhases.Where(p => !p.Any(i => i > 4)).ToList();
    }

    private int RunPhaseSet(long[] phaseInput)
    {
        var processorOutput = 0;


        for (int i = 0; i < phaseInput.Count(); i++)
        {
            var input = new Queue<long>();
            input.Enqueue(phaseInput[i]);
            input.Enqueue(processorOutput);

            var processor = new IntCodeComputer(_path, i);
            processor.RunProgram(input);
            processorOutput = (int)processor.Output;
        }

        return processorOutput;
    }
}
