using System;
using System.Collections.Generic;
using System.Linq;
using static Utilities;

class Day11_SpacePolice
{
    private string _path;
    private List<string> _blackPanels = new List<string>();
    private List<string> _whitePanels = new List<string>();
    private Direction _direction = Direction.Up;

    private enum Direction
    {
        Up = 1,
        Right = 2,
        Down = 3,
        Left = 4
    }

    public class Color
    {
        public const string Black = " ";
        public const string White = "#";
        public const int BlackRaw = 0;
        public const int WhiteRaw = 1;
    }

    public Day11_SpacePolice(string path)
    {
        _path = path;
    }

    public List<Pixel> PaintSpaceship(Queue<long> input)
    {        
        var program = Utilities.LoadProgram(_path);
        var processor = new IntCodeComputer(program, 1);

        var panels = new List<Pixel>();
        var currentX = 0;
        var currentY = 0;

        while (!processor.Halted)
        {
            var currentPanelColor = GetCurrentPanelColor(panels, currentX, currentY); // this should be the color of the current location. default black.
            input.Enqueue((long)currentPanelColor);

            var instPair = new List<long>();
            for (int i = 0; i < 2; i++)
            {
                processor.RunProgram(input);
                instPair.Add(processor.Output);
                processor.Resume();
            }
            panels.Add(PaintPanel(currentX, currentY, GetColor(instPair[0])));
            MoveRobot(instPair[1], ref currentX, ref currentY);
        }

        return panels;
    }

    public void PaintRegCode(List<Pixel> panels)
    {
        Utilities.RenderScreen(panels, true);    
    }

    public long GetCurrentPanelColor(List<Pixel> panels, int x, int y)
    {
        var touchedPanel = panels.Where(p => p.X == x && p.Y == y).ToList();
        if (!touchedPanel.Any()) return Color.BlackRaw;

        return touchedPanel.Last().Value == Color.Black ? Color.BlackRaw : Color.WhiteRaw;
    }

    public Pixel PaintPanel(int x, int y, string color)
    {
        return new Pixel(x, y, color);
    }

    private string GetColor(long instruction)
    {
        return instruction == 1 ? Color.White : Color.Black;
    }

    private void MoveRobot(long instruction, ref int x, ref int y)
    {
        switch (_direction)
        {
            case Direction.Up:
                _direction = instruction == 1 ? Direction.Right : Direction.Left;
                if (instruction == 1)
                {
                    _direction = Direction.Right;
                    x++;;
                }
                else
                {
                    _direction = Direction.Left;
                    x--;
                }
                break;
            case Direction.Right:
                if (instruction == 1)
                {
                    _direction = Direction.Down;
                    y--;
                }
                else
                {
                    _direction = Direction.Up;
                    y++;
                }
                break;
            case Direction.Down:
                _direction = instruction == 1 ? Direction.Right : Direction.Left;
                if (instruction == 1)
                {
                    _direction = Direction.Left;
                    x--;
                }
                else
                {
                    _direction = Direction.Right;
                    x++;;
                }
                break;
            case Direction.Left:
                if (instruction == 1)
                {
                    _direction = Direction.Up;
                    y++;
                }
                else
                {
                    _direction = Direction.Down;
                    y--;
                }
                break;
        }
    }
}