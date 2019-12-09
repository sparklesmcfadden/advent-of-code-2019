using System.Reflection.Emit;
using System.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

class IntCodeComputer
{
    private List<int> _program;
    private int[] _inputs;
    private int _inputPosition;
    public int output;
    private int _position;

    public IntCodeComputer(string path)
    {
        _program = LoadProgram(path);
        _position = 0;
        _inputPosition = 0;
    }

    public IntCodeComputer(string program, bool directLoad)
    {
        _program = program.Split(",").Select(c => Convert.ToInt32(c)).ToList();
    }

    private class Instruction
    {
        public int opCode { get; set; }
        public int param1 { get; set; } = 0;
        public int param2 { get; set; } = 0;
        public int param3 { get; set; } = 0;
    }

    private List<int> Add(List<int> program, Instruction instruction)
    {
        var argument1 = instruction.param1 == 0 ? program[program[_position + 1]] : program[_position + 1];
        var argument2 = instruction.param2 == 0 ? program[program[_position + 2]] : program[_position + 2];
        var argument3Pointer = instruction.param3 == 0 ? program[_position + 3] : _position + 3;

        var result = argument1 + argument2;
        program[argument3Pointer] = result;

        _position = _position + 4;

        return program;
    }
    
    private List<int> Multiply(List<int> program, Instruction instruction)
    {
        var argument1 = instruction.param1 == 0 ? program[program[_position + 1]] : program[_position + 1];
        var argument2 = instruction.param2 == 0 ? program[program[_position + 2]] : program[_position + 2];
        var argument3Pointer = instruction.param3 == 0 ? program[_position + 3] : _position + 3;

        var result = argument1 * argument2;
        program[argument3Pointer] = result;

        _position = _position + 4;

        return program;
    }

    private List<int> SaveAt(List<int> program, Instruction instruction)
    {
        var input = _inputs[_inputPosition];

        if (instruction.param1 == 0) program[program[_position + 1]] = input;
        if (instruction.param1 == 1) program[_position + 1] = input;

        _position = _position + 2;
        _inputPosition++;

        return program;
    }

    private void PrintOutput(List<int> program, Instruction instruction)
    {
        var outputValue = instruction.param1 == 0 ? program[program[_position + 1]] : program[_position + 1];
        // Console.WriteLine(" TEST > " + outputValue);
        output = outputValue;
        _position = _position + 2;
    }

    private void JumpIfTrue(List<int> program, Instruction instruction)
    {
        var argument1 = instruction.param1 == 0 ? program[program[_position + 1]] : program[_position + 1];
        var argument2 = instruction.param2 == 0 ? program[program[_position + 2]] : program[_position + 2];
        var argument3Pointer = instruction.param3 == 0 ? program[_position + 3] : _position + 3;

        if (argument1 != 0)
        {
            _position = argument2;
            return;
        }

        _position = _position + 3;
    }

    private void JumpIfFalse(List<int> program, Instruction instruction)
    {
        var argument1 = instruction.param1 == 0 ? program[program[_position + 1]] : program[_position + 1];
        var argument2 = instruction.param2 == 0 ? program[program[_position + 2]] : program[_position + 2];
        var argument3Pointer = instruction.param3 == 0 ? program[_position + 3] : _position + 3;

        if (argument1 == 0)
        {
            _position = argument2;
            return;
        };

        _position = _position + 3;
    }

    private List<int> LessThan(List<int> program, Instruction instruction)
    {
        var argument1 = instruction.param1 == 0 ? program[program[_position + 1]] : program[_position + 1];
        var argument2 = instruction.param2 == 0 ? program[program[_position + 2]] : program[_position + 2];
        var argument3Pointer = instruction.param3 == 0 ? program[_position + 3] : _position + 3;

        if (argument1 < argument2) program[argument3Pointer] = 1;
        else program[argument3Pointer] = 0;

        _position = _position + 4;

        return program;
    }

    private List<int> EqualTo(List<int> program, Instruction instruction)
    {
        var argument1 = instruction.param1 == 0 ? program[program[_position + 1]] : program[_position + 1];
        var argument2 = instruction.param2 == 0 ? program[program[_position + 2]] : program[_position + 2];
        var argument3Pointer = instruction.param3 == 0 ? program[_position + 3] : _position + 3;

        if (argument1 == argument2) program[argument3Pointer] = 1;
        else program[argument3Pointer] = 0;

        _position = _position + 4;

        return program;
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

    public List<int> RunProgram(int[] input)
    {
        _inputs = input;
        var instruction = ProcessInstruction(_program[_position]);

        while (instruction.opCode != 99)
        {
            switch (instruction.opCode)
            {
                case 1:
                    _program = Add(_program, instruction);
                    break;
                case 2:
                    _program = Multiply(_program, instruction);
                    _position = _position + 4;
                    break;
                case 3:
                    _program = SaveAt(_program, instruction);
                    break;
                case 4:
                    PrintOutput(_program, instruction);
                    break;
                case 5:
                    JumpIfTrue(_program, instruction);
                    break;
                case 6:
                    JumpIfFalse(_program, instruction);
                    break;
                case 7:
                    _program = LessThan(_program, instruction);
                    break;
                case 8:
                    _program = EqualTo(_program, instruction);
                    break;
                case 99:
                    return _program;
                default:
                    break;
            }
            Console.WriteLine(instruction.opCode);
            instruction = ProcessInstruction(_program[_position]);
        }

        return _program;

    }

}