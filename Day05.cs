using System.Threading;
using System;
using System.Collections.Generic;
using System.Linq;

class Day05_SunnyWithAChanceOfAsteroids
{
    private class Instruction
    {
        public int opCode { get; set; }
        public int param1 { get; set; } = 0;
        public int param2 { get; set; } = 0;
        public int param3 { get; set; } = 0;
    }

    private static List<int> Add(List<int> program, Instruction instruction, int position)
    {
        var argument1 = instruction.param1 == 0 ? program[program[position + 1]] : program[position + 1];
        var argument2 = instruction.param2 == 0 ? program[program[position + 2]] : program[position + 2];
        var argument3Pointer = instruction.param3 == 0 ? program[position + 3] : position + 3;

        var result = argument1 + argument2;
        program[argument3Pointer] = result;
        return program;
    }
    
    private static List<int> Multiply(List<int> program, Instruction instruction, int position)
    {
        var argument1 = instruction.param1 == 0 ? program[program[position + 1]] : program[position + 1];
        var argument2 = instruction.param2 == 0 ? program[program[position + 2]] : program[position + 2];
        var argument3Pointer = instruction.param3 == 0 ? program[position + 3] : position + 3;

        var result = argument1 * argument2;
        program[argument3Pointer] = result;
        return program;
    }

    private static List<int> SaveAt(List<int> program, int input, Instruction instruction, int position)
    {
        if (instruction.param1 == 0) program[program[position + 1]] = input;
        if (instruction.param1 == 1) program[position + 1] = input;
        return program;
    }

    private static void PrintOutput(List<int> program, Instruction instruction, int position)
    {
        
        Console.WriteLine(instruction.param1 == 0 ? program[program[position + 1]] : program[position + 1]);
    }

    private static int JumpIfTrue(List<int> program, Instruction instruction, int position)
    {
        var argument1 = instruction.param1 == 0 ? program[program[position + 1]] : program[position + 1];
        var argument2 = instruction.param2 == 0 ? program[program[position + 2]] : program[position + 2];
        var argument3Pointer = instruction.param3 == 0 ? program[position + 3] : position + 3;

        if (argument1 != 0) return argument2;

        return position + 3;
    }

    private static int JumpIfFalse(List<int> program, Instruction instruction, int position)
    {
        var argument1 = instruction.param1 == 0 ? program[program[position + 1]] : program[position + 1];
        var argument2 = instruction.param2 == 0 ? program[program[position + 2]] : program[position + 2];
        var argument3Pointer = instruction.param3 == 0 ? program[position + 3] : position + 3;

        if (argument1 == 0) return argument2;

        return position + 3;
    }

    private static List<int> LessThan(List<int> program, Instruction instruction, int position)
    {
        var argument1 = instruction.param1 == 0 ? program[program[position + 1]] : program[position + 1];
        var argument2 = instruction.param2 == 0 ? program[program[position + 2]] : program[position + 2];
        var argument3Pointer = instruction.param3 == 0 ? program[position + 3] : position + 3;

        if (argument1 < argument2) program[argument3Pointer] = 1;
        else program[argument3Pointer] = 0;

        return program;
    }

    private static List<int> EqualTo(List<int> program, Instruction instruction, int position)
    {
        var argument1 = instruction.param1 == 0 ? program[program[position + 1]] : program[position + 1];
        var argument2 = instruction.param2 == 0 ? program[program[position + 2]] : program[position + 2];
        var argument3Pointer = instruction.param3 == 0 ? program[position + 3] : position + 3;

        if (argument1 == argument2) program[argument3Pointer] = 1;
        else program[argument3Pointer] = 0;

        return program;
    }

    private static Instruction ProcessInstruction(int instruction)
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

    public static List<int> RunProgram(string programString, int input)
    {
        var program = programString.Split(',').Select(x => Convert.ToInt32(x)).ToList();
        var position = 0;
        var instruction = ProcessInstruction(program[position]);

        while (instruction.opCode != 99)
        {
            switch (instruction.opCode)
            {
                case 1:
                    program = Add(program, instruction, position);
                    position = position + 4;
                    break;
                case 2:
                    program = Multiply(program, instruction, position);
                    position = position + 4;
                    break;
                case 3:
                    program = SaveAt(program, input, instruction, position);
                    position = position + 2;
                    break;
                case 4:
                    PrintOutput(program, instruction, position);
                    position = position + 2;
                    break;
                case 5:
                    position = JumpIfTrue(program, instruction, position);
                    break;
                case 6:
                    position = JumpIfFalse(program, instruction, position);
                    break;
                case 7:
                    program = LessThan(program, instruction, position);
                    position = position + 4;
                    break;
                case 8:
                    program = EqualTo(program, instruction, position);
                    position = position + 4;
                    break;
                case 99:
                    return program;
                default:
                    break;
            }
            instruction = ProcessInstruction(program[position]);
        }

        return program;

    }

}