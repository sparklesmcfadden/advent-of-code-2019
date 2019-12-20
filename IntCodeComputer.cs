using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading;

class IntCodeComputer
{
    private List<long> _program;
    private int _processorId;
    private string _path;
    private Queue<long> _inputs = new Queue<long>();
    private long _position;
    public long Output;
    public string OutputString = "";
    public int OpCode;
    private Instruction _instruction;
    public bool Paused;
    public bool Halted;
    public bool PauseOnOutput;
    private long _relativeBase = 0;
    public bool EnableLogging = false;

    public IntCodeComputer(string path, int processorId, bool pauseOnOutput = true)
    {
        _path = path;
        _program = LoadProgram(path);
        _position = 0;
        _processorId = processorId;
        PauseOnOutput = pauseOnOutput;
        ExtendMemory();
    }

    public IntCodeComputer(List<long> program, int processorId, bool pauseOnOutput = true)
    {
        _program = program;
        _position = 0;
        _processorId = processorId;
        PauseOnOutput = pauseOnOutput;
        ExtendMemory();
    }

    public IntCodeComputer(string program, bool directLoad, int processorId, bool pauseOnOutput = true)
    {
        _program = program.Split(",").Select(c => Convert.ToInt64(c)).ToList();
        _position = 0;
        _processorId = processorId;
        PauseOnOutput = pauseOnOutput;
        ExtendMemory();
    }

    private class Instruction
    {
        public int opCode { get; set; }
        public int param1 { get; set; } = 0;
        public int param2 { get; set; } = 0;
        public int param3 { get; set; } = 0;
    }

    private void LogMessage(List<long> arguments)
    {
        if (EnableLogging)
        {
            Console.WriteLine($"Processor {_processorId}: position {_position}, running {GetOpCodeName(OpCode)} with arguments");
            arguments.ForEach(a => Console.WriteLine($"     {arguments.IndexOf(a)}: {a}"));
            // Thread.Sleep(10);
        }
    }

    public void Reset()
    {
        _program = LoadProgram(_path);
        _position = 0;
    }

    public void Resume()
    {
        Paused = false;
    }

    public void ClearOutput()
    {
        OutputString = "";
    }

    public void UpdateAddress(int address, long value)
    {
        _program[address] = value;
    }

    private void Add()
    {
        var argument1 = FindArgument(_instruction.param1, 1);
        var argument2 = FindArgument(_instruction.param2, 2);
        var argument3Pointer = FindPointer(_instruction.param3, 3);

        LogMessage(new List<long> {argument1, argument2, argument3Pointer});

        var result = argument1 + argument2;
        _program[(int)argument3Pointer] = result;

        _position = _position + 4;
    }
    
    private void Multiply()
    {
        var argument1 = FindArgument(_instruction.param1, 1);
        var argument2 = FindArgument(_instruction.param2, 2);
        var argument3Pointer = FindPointer(_instruction.param3, 3);

        LogMessage(new List<long> {argument1, argument2, argument3Pointer});

        var result = argument1 * argument2;
        _program[(int)argument3Pointer] = result;

        _position = _position + 4;
    }

    private void SaveAt()
    {
        long input;
        if (_inputs.Any())
        {
            input = _inputs.Dequeue();
        }
        else
        {
            // input = Convert.ToInt64(Console.ReadLine());
            input = 1;
        }

        LogMessage(new List<long> {input});

        if (_instruction.param1 == 0) _program[Convert.ToInt32(_program[(int)_position + 1])] = input;
        if (_instruction.param1 == 1) _program[(int)_position + 1] = input;
        if (_instruction.param1 == 2) _program[(int)_relativeBase] = input;

        _position = _position + 2;
    }

    private void PrintOutput()
    {
        var output = FindArgument(_instruction.param1, 1);

        LogMessage(new List<long> {output});

        Output = output;
        if (OutputString.Length == 0) OutputString += output;
        else OutputString += "," + output;
        if (PauseOnOutput) Paused = true;
        _position = _position + 2;
    }

    private void JumpIfTrue()
    {
        var argument1 = FindArgument(_instruction.param1, 1);
        var argument2 = FindArgument(_instruction.param2, 2);
        var argument3Pointer = FindPointer(_instruction.param3, 3);

        LogMessage(new List<long> {argument1, argument2, argument3Pointer});

        if (argument1 != 0)
        {
            _position = argument2;
            return;
        }

        _position = _position + 3;
    }

    private void JumpIfFalse()
    {
        var argument1 = FindArgument(_instruction.param1, 1);
        var argument2 = FindArgument(_instruction.param2, 2);
        var argument3Pointer = FindPointer(_instruction.param3, 3);

        LogMessage(new List<long> {argument1, argument2, argument3Pointer});

        if (argument1 == 0)
        {
            _position = argument2;
            return;
        };

        _position = _position + 3;
    }

    private void LessThan()
    {
        var argument1 = FindArgument(_instruction.param1, 1);
        var argument2 = FindArgument(_instruction.param2, 2);
        var argument3Pointer = FindPointer(_instruction.param3, 3);

        LogMessage(new List<long> {argument1, argument2, argument3Pointer});

        if (argument1 < argument2) _program[(int)argument3Pointer] = 1;
        else _program[(int)argument3Pointer] = 0;

        _position = _position + 4;
    }

    private void EqualTo()
    {
        var argument1 = FindArgument(_instruction.param1, 1);
        var argument2 = FindArgument(_instruction.param2, 2);
        var argument3Pointer = FindPointer(_instruction.param3, 3);

        LogMessage(new List<long> {argument1, argument2, argument3Pointer});

        if (argument1 == argument2) _program[(int)argument3Pointer] = 1;
        else _program[(int)argument3Pointer] = 0;

        _position = _position + 4;
    }

    private void AdjustBase()
    {
        var adjustmentValue = FindArgument(_instruction.param1, 1);

        LogMessage(new List<long> {adjustmentValue});

        _relativeBase += adjustmentValue;
        _position = _position + 2;
    }

    private void Halt()
    {
        Halted = true;

        LogMessage(new List<long>());
    }

    private void Pause()
    {
        Paused = true;
    }

    private long FindPointer(long parameter, long argNumber)
    {
        long pointer = 0;
        switch (parameter)
        {
            case 0:
                pointer = _program[Convert.ToInt32(_position + argNumber)];
                break;
            case 1:
                pointer = _position + argNumber;
                break;
            case 2:
                pointer = Convert.ToInt32(_relativeBase + _program[(int)(_position + argNumber)]);
                break;
            default:
                break;
        }
        return pointer;
    }

    private long FindArgument(long parameter, long argNumber)
    {
        long argumentValue = 0;
        switch (parameter)
        {
            case 0:
                argumentValue = _program[Convert.ToInt32(_program[(int)(_position + argNumber)])];
                break;
            case 1:
                argumentValue = _program[Convert.ToInt32(_position + argNumber)];
                break;
            case 2:
                argumentValue = _program[Convert.ToInt32(_relativeBase + _program[(int)(_position + argNumber)])];
                break;
            default:
                break;
        }
        return argumentValue;
    }

    private Instruction ProcessInstruction(long instruction)
    {
        var result = new Instruction();

        var instructionList = instruction.ToString().Select(i => Convert.ToInt32(i.ToString())).ToList();

        result.opCode = instructionList.Count >= 2 ?
                            Convert.ToInt32(instruction.ToString().Substring(instructionList.Count - 2, 2))
                            : instructionList.Last();
        result.param1 = instructionList.Count >= 3 ? instructionList[instructionList.Count - 3] : 0;
        result.param2 = instructionList.Count >= 4 ? instructionList[instructionList.Count - 4] : 0;
        result.param3 = instructionList.Count >= 5 ? instructionList[instructionList.Count - 5] : 0;

        return result;
    }

    public static List<long> LoadProgram(string path)
    {
        var input = File.ReadAllLines(path);
        return input[0].Split(",").Select(c => Convert.ToInt64(c)).ToList();
    }

    private void ExtendMemory()
    {
        var memory = new List<long>();
        for (int i = 0; i < 1024; i++)
        {
            memory.Add(0);
        }
        _program.AddRange(memory);
    }

    public List<long> RunProgram(Queue<long> input)
    {
        _inputs = input;
        _instruction = ProcessInstruction(_program[(int)_position]);

        while (!Halted && !Paused)
        {
            OpCode = _instruction.opCode;
            RunInstruction();
            _instruction = ProcessInstruction(_program[(int)_position]);
        }

        return _program;
    }

    private string GetOpCodeName(int opCode)
    {
        switch (opCode)
        {
            case 1: return "Add";
            case 2: return "Multiply";
            case 3: return "Save Input";
            case 4: return "Output";
            case 5: return "Jump If True";
            case 6: return "Jump If False";
            case 7: return "Less Than";
            case 8: return "Equal To";
            case 9: return "Adjust Base";
            case 99: return "HALT";
            default: return "None";
        }
    }

    private void RunInstruction()
    {   
        switch (OpCode)
        {
            case 1:
                Add();
                break;
            case 2:
                Multiply();
                break;
            case 3:
                SaveAt();
                break;
            case 4:
                PrintOutput();
                break;
            case 5:
                JumpIfTrue();
                break;
            case 6:
                JumpIfFalse();
                break;
            case 7:
                LessThan();
                break;
            case 8:
                EqualTo();
                break;
            case 9:
                AdjustBase();
                break;
            case 99:
                Halt();
                break;
            default:
                break;
        }
    }

}