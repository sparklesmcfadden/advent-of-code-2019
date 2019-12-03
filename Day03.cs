using System;
using System.Collections.Generic;
using System.Linq;

class Day03_CrossedWires
{
    public static int IntersectWires(Wire wireA, Wire wireB)
    {
        var intersect = wireA.WireMap.Intersect(wireB.WireMap).Select(pos => {
                var coords = pos.Split(',');
                var x = Convert.ToInt32(coords[0]);
                var y = Convert.ToInt32(coords[1]);
                return Math.Abs(x) + Math.Abs(y);
            });

        return intersect.Min();
    }

    public class Wire
    {
        public int x { get; set; }
        public int y { get; set; }
        public string CurrentLocation
        {
            get { return $"{x},{y}"; }
        }
        public int DistanceFromStart
        {
            get { return Math.Abs(x) + Math.Abs(y); }
        }
        public List<string> WireMap { get; set; }

        public void MoveUp(int distance)
        {
            for (int i = 0; i < distance; i++)
            {
                x++;
                WireMap.Add(this.CurrentLocation);
            }
        }
        public void MoveDown(int distance)
        {
            for (int i = 0; i < distance; i++)
            {
                x--;
                WireMap.Add(this.CurrentLocation);
            }
        }
        public void MoveLeft(int distance)
        {
            for (int i = 0; i < distance; i++)
            {
                y--;
                WireMap.Add(this.CurrentLocation);
            }
        }
        public void MoveRight(int distance)
        {
            for (int i = 0; i < distance; i++)
            {
                y++;
                WireMap.Add(this.CurrentLocation);
            }
        }

        public Wire(string wireDefinition)
        {
            x = 0;
            y = 0;
            WireMap = new List<string>();

            var wireSegments = wireDefinition.Split(',');
            foreach (var segment in wireSegments)
            {
                var direction = segment[0];
                var length = Convert.ToInt32(segment.Substring(1));
                switch (direction)
                {
                    case 'U':
                        this.MoveUp(length);
                        break;
                    case 'D':
                        this.MoveDown(length);
                        break;
                    case 'L':
                        this.MoveLeft(length);
                        break;
                    case 'R':
                        this.MoveRight(length);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}