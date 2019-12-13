using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Utilities;

class Day13_CarePackage
{
    private List<Pixel> _gamePixels = new List<Pixel>();
    private long _score;
    private IntCodeComputer _arcadeMachine;

    public class Sprite
    {
        public const string EMPTY = " ";
        public const string WALL = "█";
        public const string BLOCK = "x";
        public const string PADDLE = "=";
        public const string BALL = "o";
    }

    public List<long> GetOutput()
    {
        var program = Utilities.LoadProgram("Data/Day13_Input.txt");
        var processor = new IntCodeComputer(program, 1, false);
        processor.RunProgram(new Queue<long> {});
        return Utilities.LoadProgramFromString(processor.OutputString);
    }

    private void AddInstruction(Queue<long> input)
    {
        for (int i = 0; i < 4; i++)
        {
            input.Enqueue(1);
            input.Enqueue(0);
            input.Enqueue(-1);
            input.Enqueue(0);
        }
    }

    public void PlayGame()
    {
        var program = Utilities.LoadProgram("Data/Day13_Input.txt");
        _arcadeMachine = new IntCodeComputer(program, 1, false);
        _arcadeMachine.EnableLogging = true;
        _arcadeMachine.UpdateAddress(0, 2);
        var input = new Queue<long>();
        AddInstruction(input);
        var loopCount = 0;
        while (!_arcadeMachine.Stopped)
        {
            _arcadeMachine.RunProgram(input);
            var machineState = Utilities.LoadProgramFromString(_arcadeMachine.OutputString);
            GetPixels(machineState);
            DrawScreen();
            _arcadeMachine.SoftReset();
            loopCount++;
        }
    }


    public void GetPixels(List<long> machineState)
    {
        _gamePixels = new List<Pixel>();
        for (int i = 0; i < machineState.Count - 1; i += 3)
        {
            if (machineState[i] == -1 && machineState[i+1] == 0)
            {
                _score = machineState[i+2];
                continue;
            }
            var pixel = new Pixel(machineState[i], machineState[i+1], GetPixelValue(machineState[i+2]));
            _gamePixels.Add(pixel);
        }
    }

    public void DrawScreen()
    {
        var machineState = GetOutput();
        GetPixels(machineState);
        Console.WriteLine($" SCORE: {_score}");
        Utilities.RenderScreen(_gamePixels);
    }

    public int GetBlockCount()
    {
        var machineState = GetOutput();
        GetPixels(machineState);
        return _gamePixels.Where(p => p.Value == Sprite.BLOCK).Count();
    }

    private string GetPixelValue(long input)
    {
        switch (input)
        {
            case 0: return Sprite.EMPTY;
            case 1: return Sprite.WALL;
            case 2: return Sprite.BLOCK;
            case 3: return Sprite.PADDLE;
            case 4: return Sprite.BALL;
            default: return Sprite.EMPTY;
        }
    }
}