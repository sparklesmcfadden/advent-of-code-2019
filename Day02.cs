using System;
using System.Collections.Generic;

class Day02_1202ProgramAlarm
{
    public static List<int> Add(List<int> input, int position1, int position2, int position3)
    {
        var result = input[input[position1]] + input[input[position2]];
        input[input[position3]] = result;
        return input;
    }
    
    public static List<int> Multiply(List<int> input, int position1, int position2, int position3)
    {
        var result = input[input[position1]] * input[input[position2]];
        input[input[position3]] = result;
        return input;
    }

    public static List<int> RunProgram(List<int> program)
    {
        var position = 0;
        var opCode = program[position];
        while (opCode != 99)
        {
            switch (opCode)
            {
                case 1:
                    program = Add(program, position + 1, position + 2, position + 3);
                    break;
                case 2:
                    program = Multiply(program, position + 1, position + 2, position + 3);
                    break;
                case 99:
                    return program;
                default:
                    break;
            }
            position = position + 4;
            opCode = program[position];
        }

        return program;

    }

}