
using System.Linq.Expressions;

namespace AdventOfCode.Year2023;



public class Day10 : Solution
{
    public override string Example => @"..F7.
.FJ|.
SJ.L7
|F--J
LJ...";

    // Top left is 0, 0, y increases downwards
    Dictionary<char, (int x, int y)[]> tiles = new Dictionary<char, (int x, int y)[]>
    {
        {'.', new (int x, int y)[] {}},
        {'|', new (int x, int y)[] {(0, -1), (0, 1)}},
        {'-', new (int x, int y)[] {(-1, 0), (1, 0)}},
        {'J', new (int x, int y)[] {(0, -1), (-1, 0)}},
        {'L', new (int x, int y)[] {(0, -1), (1, 0)}},
        {'F', new (int x, int y)[] {(0, 1), (1, 0)}},
        {'7', new (int x, int y)[] {(0, 1), (-1, 0)}},
    };

    public override Answer One(string input)
    {
        var grid = ParseGrid(input);
        return FindLoop(grid).Count() / 2;
    }

    Dictionary<(int x, int y), char> FindLoop(Dictionary<(int x, int y), char> grid)
    {
        var start = grid.Where(e => e.Value == 'S').Select(e => e.Key).Single();

        foreach (var startDir in new (int x, int y)[] { (0, 1), (0, -1), (1, 0), (-1, 0) })
        {
            var loop = new Dictionary<(int x, int y), char>();
            var current = (x: start.x + startDir.x, y: start.y + startDir.y);
            LogEx($"Start {current}");
            var prev = start;

            int steps = 1;
            while (true)
            {
                var currentTile = grid.GetValueOrDefault(current, '.');

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
                    var lastStep = (x: prev.x - current.x, y: prev.y - current.y);
                    var startTile = tiles.Single(t => t.Value.Contains(startDir) && t.Value.Contains(lastStep)).Key;
                    LogEx($"Start tile is {startTile}");
                    loop.Add(current, startTile);
                    return loop;
                }
            }
        }
        throw new Exception("No loop");
    }

    private (int x, int y)[] Exits((int x, int y) from, (int x, int y) current, char currentTile)
    {
        var dirs = tiles[currentTile];
        var exits = dirs.Select(d => (x: current.x + d.x, y: current.y + d.y)).ToArray();
        if (exits.Any(e => e == from))
        {
            return exits.Where(e => e != from).ToArray();
        }
        return new (int x, int y)[] { };
    }

    enum Loc { Outside, InsideBelow, InsideAbove, Inside };

    public override Answer Two(string input)
    {
        var grid = ParseGrid(input);

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
                var pos = (x: x, y: y);
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

    private static Dictionary<(int x, int y), char> ParseGrid(string input)
    {
        Dictionary<(int x, int y), char> grid = new();
        int y = 0;
        foreach (var line in input.Lines().Where(IsNotBlank))
        {
            int x = 0;
            foreach (var c in line.Trim())
            {
                grid.Add((x, y), c);
                x++;
            }
            y++;
        }

        return grid;
    }
}
