
using System.Drawing;

namespace AdventOfCode.Year2023;

public class Day10 : Solution
{
    public override string Example => @"..F7.
.FJ|.
SJ.L7
|F--J
LJ...";

    // Top left is 0, 0, y increases downwards
    Dictionary<char, Point[]> tiles = new Dictionary<char, Point[]>
    {
        {'.', new Point[] {}},
        {'|', new Point[] {new(0, -1), new(0, 1)}},
        {'-', new Point[] {new(-1, 0), new(1, 0)}},
        {'J', new Point[] {new(0, -1), new(-1, 0)}},
        {'L', new Point[] {new(0, -1), new(1, 0)}},
        {'F', new Point[] {new(0, 1), new(1, 0)}},
        {'7', new Point[] {new(0, 1), new(-1, 0)}},
    };

    public override Answer One(string input)
    {
        var grid = Grid.Parse(input, '.');
        return FindLoop(grid).Count() / 2;
    }

    Dictionary<Point, char> FindLoop(Grid grid)
    {
        var start = grid.Cells.Where(e => e.Value == 'S').Select(e => e.Key).Single();

        foreach (var startDir in new Point[] { new(0, 1), new(0, -1), new(1, 0), new(-1, 0) })
        {
            var loop = new Dictionary<Point, char>();
            var current = new Point(start.X + startDir.X, start.Y + startDir.Y);
            LogEx($"Start {current}");
            var prev = start;

            int steps = 1;
            while (true)
            {
                var currentTile = grid[current];

                loop.Add(current, currentTile);

                var exits = Exits(prev, current, currentTile);
                if (!exits.Any())
                {
                    LogEx($"Dead end {currentTile} at {current} not connected to {prev}");
                    break;
                }
                LogEx($"{currentTile} at {current} exits " + string.Join(", ", exits));
                steps++;
                prev = current;
                current = exits.Single();

                if (current == start)
                {
                    // work out what start is?!
                    var lastStep = new Point(prev.X - current.X, prev.Y - current.Y);
                    var startTile = tiles.Single(t => t.Value.Contains(startDir) && t.Value.Contains(lastStep)).Key;
                    LogEx($"Start tile is {startTile}");
                    loop.Add(current, startTile);
                    return loop;
                }
            }
        }
        throw new Exception("No loop");
    }

    private Point[] Exits(Point from, Point current, char currentTile)
    {
        var dirs = tiles[currentTile];
        var exits = dirs.Select(d => new Point(current.X + d.X, current.Y + d.Y)).ToArray();
        if (exits.Any(e => e == from))
        {
            return exits.Where(e => e != from).ToArray();
        }
        return new Point[] { };
    }

    enum Loc { Outside, InsideBelow, InsideAbove, Inside };

    public override Answer Two(string input)
    {
        var grid = Grid.Parse(input, '.');

        var loop = FindLoop(grid);

        var lines = input.Lines().Where(IsNotBlank).ToList();
        var height = lines.Count;
        var width = lines.First().Trim().Length;

        int t = 0;
        for (int y = 0; y < height; y++)
        {
            var loc = Loc.Outside;
            for (int x = 0; x < width; x++)
            {
                var pos = new Point(x, y);
                var tile = loop.GetValueOrDefault(pos, '.');
                if (tile == '.' && loc != Loc.Outside)
                    t++;

                if (loc == Loc.Outside)
                {
                    if (tile == '|')
                        loc = Loc.Inside;
                    else if (tile == 'F')
                        loc = Loc.InsideBelow;
                    else if (tile == 'L')
                        loc = Loc.InsideAbove;
                    else if (tile != '.')
                        throw new Exception($"Found {tile} at {x},{y}");
                }
                else if (loc == Loc.Inside)
                {
                    if (tile == '|')
                        loc = Loc.Outside;
                    else if (tile == 'F')
                        loc = Loc.InsideAbove;
                    else if (tile == 'L')
                        loc = Loc.InsideBelow;
                    else if (tile != '.')
                        throw new Exception();
                }
                else if (loc == Loc.InsideAbove)
                {
                    if (tile == 'J')
                        loc = Loc.Outside;
                    else if (tile == '7')
                        loc = Loc.Inside;
                    else if (tile != '-')
                        throw new Exception();
                }
                else if (loc == Loc.InsideBelow)
                {
                    if (tile == 'J')
                        loc = Loc.Inside;
                    else if (tile == '7')
                        loc = Loc.Outside;
                    else if (tile != '-')
                        throw new Exception($"Found {tile} at {x},{y}");
                }
                else
                    throw new Exception();
            }
        }

        return t;
    }
}
