using System;
using System.Collections.Generic;
using System.Linq;

class PointComparer : IEqualityComparer<Day03_CrossedWires.Point>
{
    public bool Equals(Day03_CrossedWires.Point a, Day03_CrossedWires.Point b)
    {
        return a.x == b.x && a.y == b.y;
    }

    public int GetHashCode(Day03_CrossedWires.Point obj)
    {
        return obj.x.GetHashCode();
    }
}

class Day03_CrossedWires
{
    public static List<Point> GetIntersections(Wire wireA, Wire wireB)
    {
        return wireA.WireMap.Intersect(wireB.WireMap, new PointComparer()).ToList();
    }

    public static int GetShortestWireLength(Wire wireA, Wire wireB)
    {
        var wireAIntersections = GetIntersections(wireA, wireB);
        var wireBIntersections = GetIntersections(wireB, wireA);

        var intersectionWireLengths = new List<int>();
        foreach (var intersectionA in wireAIntersections)
        {
            var intersectionB = wireBIntersections.Single(b => b.x == intersectionA.x && b.y == intersectionA.y);
            var newDistance = intersectionA.id + intersectionB.id;
            intersectionWireLengths.Add(newDistance);
        }
        return intersectionWireLengths.Where(d => d> 0).Min();
    }

    public static int IntersectWires(Wire wireA, Wire wireB)
    {
        var intersections = GetIntersections(wireA, wireB);
        var distances = intersections.Select(pos => Math.Abs(pos.x) + Math.Abs(pos.y));

        return distances.Where(d => d > 0).Min();
    }

    public static int ShortestIntersection(Wire wireA, Wire wireB)
    {
        return 0;
    }

    public class Point
    {
        public int x { get; set; }
        public int y { get; set; }
        public int id { get; set; }

        public Point(int _x, int _y, int _id)
        {
            x = _x;
            y = _y;
            id = _id;
        }
    }

    public class Wire
    {
        public Point LastPoint
        {
            get { return this.WireMap.Last(); }
        }
        public int DistanceFromStart
        {
            get { return Math.Abs(this.LastPoint.x) + Math.Abs(this.LastPoint.y); }
        }
        public List<Point> WireMap { get; set; } = new List<Point> { new Point(0, 0, 0) };

        public void MoveUp(int distance)
        {
            for (int i = 0; i < distance; i++)
            {
                var newLocation = new Point(this.LastPoint.x + 1, this.LastPoint.y, this.LastPoint.id + 1);
                WireMap.Add(newLocation);
            }
        }
        public void MoveDown(int distance)
        {
            for (int i = 0; i < distance; i++)
            {
                var newLocation = new Point(this.LastPoint.x - 1, this.LastPoint.y, this.LastPoint.id + 1);
                WireMap.Add(newLocation);
            }
        }
        public void MoveLeft(int distance)
        {
            for (int i = 0; i < distance; i++)
            {
                var newLocation = new Point(this.LastPoint.x, this.LastPoint.y - 1, this.LastPoint.id + 1);
                WireMap.Add(newLocation);
            }
        }
        public void MoveRight(int distance)
        {
            for (int i = 0; i < distance; i++)
            {
                var newLocation = new Point(this.LastPoint.x, this.LastPoint.y + 1, this.LastPoint.id + 1);
                WireMap.Add(newLocation);
            }
        }

        public Wire(string wireDefinition)
        {
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