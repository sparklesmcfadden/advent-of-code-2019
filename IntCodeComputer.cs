using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

class IntCodeComputer
{
    private List<int> _program;
    private int _processorId;
    private string _path;
    private int[] _inputs;
    public int _inputPosition;
    private int _position;
    public int Output;
    public string OutputString = "";
    public int OpCode;
    public bool Halted;
    public bool HaltOnOutput;
    public bool Stopped;
    private int _relativeBase = 0;

    public IntCodeComputer(string path, int processorId, bool haltOnOutput = true)
    {
        _path = path;
        _program = LoadProgram(path);
        _position = 0;
        _inputPosition = 0;
        _processorId = processorId;
        HaltOnOutput = haltOnOutput;
    }

    public IntCodeComputer(List<int> program, int processorId, bool haltOnOutput = true)
    {
        _program = program;
        _position = 0;
        _inputPosition = 0;
        _processorId = processorId;
        HaltOnOutput = haltOnOutput;
    }

    public IntCodeComputer(string program, bool directLoad, int processorId, bool haltOnOutput = true)
    {
        _program = program.Split(",").Select(c => Convert.ToInt32(c)).ToList();
        _position = 0;
        _inputPosition = 0;
        _processorId = processorId;
        HaltOnOutput = haltOnOutput;
    }

    private class Instruction
    {
        public int opCode { get; set; }
        public int param1 { get; set; } = 0;
        public int param2 { get; set; } = 0;
        public int param3 { get; set; } = 0;
    }

    public void Reset()
    {
        _program = LoadProgram(_path);
        _position = 0;
        _inputPosition = 0;
    }

    public void Resume()
    {
        Halted = false;
    }

    private void Add(Instruction instruction)
    {
        var argument1 = FindArgument(instruction.param1, 1);
        var argument2 = FindArgument(instruction.param2, 2);
        var argument3Pointer = FindPointer(instruction.param3, 3);

        var result = argument1 + argument2;
        _program[argument3Pointer] = result;

        _position = _position + 4;
    }
    
    private void Multiply(Instruction instruction)
    {
        var argument1 = FindArgument(instruction.param1, 1);
        var argument2 = FindArgument(instruction.param2, 2);
        var argument3Pointer = FindPointer(instruction.param3, 3);

        var result = argument1 * argument2;
        _program[argument3Pointer] = result;

        _position = _position + 4;
    }

    private void SaveAt(Instruction instruction)
    {
        var input = _inputs[_inputPosition];

        if (instruction.param1 == 0) _program[_program[_position + 1]] = input;
        if (instruction.param1 == 1) _program[_position + 1] = input;
        if (instruction.param1 == 2) _program[_relativeBase + 1] = input;

        _position = _position + 2;
        _inputPosition++;
    }

    private void PrintOutput(Instruction instruction)
    {
                var output = FindArgument(instruction.param1, 1);
        Output = output;
        OutputString += output + ",";
        if (HaltOnOutput) Halted = true;
        _position = _position + 2;
    }

    private void JumpIfTrue(Instruction instruction)
    {
        var argument1 = FindArgument(instruction.param1, 1);
        var argument2 = FindArgument(instruction.param2, 2);
        var argument3Pointer = FindPointer(instruction.param3, 3);

        if (argument1 != 0)
        {
            _position = argument2;
            return;
        }

        _position = _position + 3;
    }

    private void JumpIfFalse(Instruction instruction)
    {
        var argument1 = FindArgument(instruction.param1, 1);
        var argument2 = FindArgument(instruction.param2, 2);
        var argument3Pointer = FindPointer(instruction.param3, 3);

        if (argument1 == 0)
        {
            _position = argument2;
            return;
        };

        _position = _position + 3;
    }

    private void LessThan(Instruction instruction)
    {
        var argument1 = FindArgument(instruction.param1, 1);
        var argument2 = FindArgument(instruction.param2, 2);
        var argument3Pointer = FindPointer(instruction.param3, 3);

        if (argument1 < argument2) _program[argument3Pointer] = 1;
        else _program[argument3Pointer] = 0;

        _position = _position + 4;
    }

    private void EqualTo(Instruction instruction)
    {
        var argument1 = FindArgument(instruction.param1, 1);
        var argument2 = FindArgument(instruction.param2, 2);
        var argument3Pointer = FindPointer(instruction.param3, 3);

        if (argument1 == argument2) _program[argument3Pointer] = 1;
        else _program[argument3Pointer] = 0;

        _position = _position + 4;
    }

    private void AdjustBase(Instruction instruction)
    {
        var adjustmentValue = FindArgument(instruction.param1, 1);
        _relativeBase += adjustmentValue;
        _position = _position + 2;
    }

    private int FindPointer(int parameter, int argNumber)
    {
        var pointer = 0;
        switch (parameter)
        {
            case 0:
                pointer = _program[_position + argNumber];
                break;
            case 1:
                pointer = _position + argNumber;
                break;
            case 2:
                pointer = _program[_relativeBase + argNumber];
                break;
            default:
                break;
        }
        return pointer;
    }

    private int FindArgument(int parameter, int argNumber)
    {
        var argumentValue = 0;
        switch (parameter)
        {
            case 0:
                argumentValue = _program[_program[_position + argNumber]];
                break;
            case 1:
                argumentValue = _program[_position + argNumber];
                break;
            case 2:
                argumentValue = _program[_relativeBase + argNumber];
                break;
            default:
                break;
        }
        return argumentValue;
    }

    private Instruction ProcessInstruction(int instruction)
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

    public static List<int> LoadProgram(string path)
    {
        var input = File.ReadAllLines(path);
        return input[0].Split(",").Select(c => Convert.ToInt32(c)).ToList();
    }

    private void ExtendMemory()
    {
        var memory = new List<int>();
        for (int i = 0; i < 1024; i++)
        {
            memory.Add(0);
        }
        _program.AddRange(memory);
    }

    public List<int> RunProgram(int[] input)
    {
        ExtendMemory();
        _inputs = input;
        var instruction = ProcessInstruction(_program[_position]);

        while (!Halted && !Stopped)
        {
            OpCode = instruction.opCode;
            RunInstruction(OpCode, instruction);
            instruction = ProcessInstruction(_program[_position]);
        }

        return _program;
    }

    private void RunInstruction(int opCode, Instruction instruction)
    {   
        switch (OpCode)
        {
            case 1:
                Add(instruction);
                break;
            case 2:
                Multiply(instruction);
                break;
            case 3:
                SaveAt(instruction);
                break;
            case 4:
                PrintOutput(instruction);
                break;
            case 5:
                JumpIfTrue(instruction);
                break;
            case 6:
                JumpIfFalse(instruction);
                break;
            case 7:
                LessThan(instruction);
                break;
            case 8:
                EqualTo(instruction);
                break;
            case 9:
                AdjustBase(instruction);
                break;
            case 99:
                Stopped = true;
                break;
            default:
                break;
        }
    }

}