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
    public int OpCode;

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

    private void Add(Instruction instruction)
    {
        var argument1 = instruction.param1 == 0 ? _program[_program[_position + 1]] : _program[_position + 1];
        var argument2 = instruction.param2 == 0 ? _program[_program[_position + 2]] : _program[_position + 2];
        var argument3Pointer = instruction.param3 == 0 ? _program[_position + 3] : _position + 3;

        var result = argument1 + argument2;
        _program[argument3Pointer] = result;

        _position = _position + 4;
    }
    
    private void Multiply(Instruction instruction)
    {
        var argument1 = instruction.param1 == 0 ? _program[_program[_position + 1]] : _program[_position + 1];
        var argument2 = instruction.param2 == 0 ? _program[_program[_position + 2]] : _program[_position + 2];
        var argument3Pointer = instruction.param3 == 0 ? _program[_position + 3] : _position + 3;

        var result = argument1 * argument2;
        _program[argument3Pointer] = result;

        _position = _position + 4;
    }

    private void SaveAt(Instruction instruction)
    {
        var input = _inputs[_inputPosition];

        if (instruction.param1 == 0) _program[_program[_position + 1]] = input;
        if (instruction.param1 == 1) _program[_position + 1] = input;

        _position = _position + 2;
        _inputPosition++;
    }

    private void PrintOutput(Instruction instruction)
    {
        var outputValue = instruction.param1 == 0 ? _program[_program[_position + 1]] : _program[_position + 1];
        // Console.WriteLine(" TEST > " + outputValue);
        output = outputValue;
        _position = _position + 2;
    }

    private void JumpIfTrue(Instruction instruction)
    {
        var argument1 = instruction.param1 == 0 ? _program[_program[_position + 1]] : _program[_position + 1];
        var argument2 = instruction.param2 == 0 ? _program[_program[_position + 2]] : _program[_position + 2];
        var argument3Pointer = instruction.param3 == 0 ? _program[_position + 3] : _position + 3;

        if (argument1 != 0)
        {
            _position = argument2;
            return;
        }

        _position = _position + 3;
    }

    private void JumpIfFalse(Instruction instruction)
    {
        var argument1 = instruction.param1 == 0 ? _program[_program[_position + 1]] : _program[_position + 1];
        var argument2 = instruction.param2 == 0 ? _program[_program[_position + 2]] : _program[_position + 2];
        var argument3Pointer = instruction.param3 == 0 ? _program[_position + 3] : _position + 3;

        if (argument1 == 0)
        {
            _position = argument2;
            return;
        };

        _position = _position + 3;
    }

    private void LessThan(Instruction instruction)
    {
        var argument1 = instruction.param1 == 0 ? _program[_program[_position + 1]] : _program[_position + 1];
        var argument2 = instruction.param2 == 0 ? _program[_program[_position + 2]] : _program[_position + 2];
        var argument3Pointer = instruction.param3 == 0 ? _program[_position + 3] : _position + 3;

        if (argument1 < argument2) _program[argument3Pointer] = 1;
        else _program[argument3Pointer] = 0;

        _position = _position + 4;
    }

    private void EqualTo(Instruction instruction)
    {
        var argument1 = instruction.param1 == 0 ? _program[_program[_position + 1]] : _program[_position + 1];
        var argument2 = instruction.param2 == 0 ? _program[_program[_position + 2]] : _program[_position + 2];
        var argument3Pointer = instruction.param3 == 0 ? _program[_position + 3] : _position + 3;

        if (argument1 == argument2) _program[argument3Pointer] = 1;
        else _program[argument3Pointer] = 0;

        _position = _position + 4;
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

        while (OpCode != 99)
        {
            OpCode = instruction.opCode;
            if (_inputPosition > _inputs.Count() - 1 && OpCode == 3) return _program;
            RunInstruction(OpCode, instruction);
            if (OpCode == 4)
            {
                Console.WriteLine(output);
                return _program;
            }
            instruction = ProcessInstruction(_program[_position]);
        }

        return _program;
    }
    public List<int> RunProgramWithExtraInputs(int[] input, List<int> additionalInputs)
    {
        input.ToList().AddRange(additionalInputs);
        _inputs = input.ToArray();
        var instruction = ProcessInstruction(_program[_position]);

        while (OpCode != 99)
        {
            OpCode = instruction.opCode;
            if (_inputPosition > _inputs.Count() - 1 && OpCode == 3) return _program;
            RunInstruction(OpCode, instruction);
            if (OpCode == 4)
            {
                Console.WriteLine(output);
                return _program;
            }
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
            case 99:
                break;
            default:
                break;
        }
    }

}