using System;
using System.Collections.Generic;
using System.Linq;

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

    public enum Color
    {
        Black = 0,
        White = 1
    }

    public class Panel
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Color Color { get; set; }

        public Panel(int x, int y, Color color)
        {
            X = x;
            Y = y;
            Color = color;
        }
    }

    public Day11_SpacePolice(string path)
    {
        _path = path;
    }

    public List<Panel> PaintSpaceship(List<long> input)
    {        
        var program = Utilities.LoadProgram(_path);
        var processor = new IntCodeComputer(program, 1);

        var panels = new List<Panel>();
        var currentX = 0;
        var currentY = 0;

        while (!processor.Stopped)
        {
            var currentPanelColor = GetCurrentPanelColor(panels, currentX, currentY); // this should be the color of the current location. default black.
            input.Add((long)currentPanelColor);

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

    public void PaintRegCode(List<Panel> panels)
    {
        var maxX = panels.Max(p => p.X);
        var minX = panels.Min(p => p.X);
        var maxY = panels.Max(p => p.Y);
        var minY = panels.Min(p => p.Y);

        for (int y = maxY; y >= minY; y--)
        {
            var imageRow = "";
            for (int x = minX; x <= maxX; x++)
            {
                var panel = panels.FirstOrDefault(p => p.X == x && p.Y == y);
                if (panel == null)
                {
                    imageRow += " ";
                    continue;
                }
                imageRow += panel.Color == Color.White ? "@" : " ";
            }
            Console.WriteLine(imageRow);
        }
    }

    public Color GetCurrentPanelColor(List<Panel> panels, int x, int y)
    {
        var touchedPanel = panels.Where(p => p.X == x && p.Y == y).ToList();
        if (!touchedPanel.Any()) return Color.Black;

        return touchedPanel.Last().Color;
    }

    public Panel PaintPanel(int x, int y, Color color)
    {
        return new Panel(x, y, color);
    }

    private Color GetColor(long instruction)
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