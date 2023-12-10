
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
        int t = 0;

        Dictionary<(int x, int y), char> grid = new();
        int y = 0;
        foreach (var line in input.Lines().Where(IsNotBlank))
        { 
            int x = 0;
            foreach(var c in line.Trim())
            {
                grid.Add((x, y), c);
                x++;
            }
            y++;
        }

        var start = grid.Where(e => e.Value == 'S').Select(e => e.Key).Single();

        foreach(var startDir in new (int x, int y)[] {(0, 1), (0, -1), (1, 0), (-1, 0)})
        {
            var current = (x: start.x + startDir.x, y: start.y + startDir.y);
            LogEx($"Start {current}");
            var prev = start;

            int steps = 1;
            while(true)
            {
                var exits = Exits(prev, current, grid.GetValueOrDefault(current, '.'));
                if (!exits.Any())
                {
                    LogEx($"Dead end {current} at " + grid.GetValueOrDefault(current, '.') + $" not connected to {prev}");
                    break;
                }
                LogEx($"{current} at " + grid.GetValueOrDefault(current, '.') + " exits " + string.Join(", ", exits));
                steps++;
                prev = current;
                current = exits.Single();

                if (current == start)
                    return steps / 2;
            }
        }


        return t;
    }

    private (int x, int y)[] Exits((int x, int y) from, (int x, int y) current, char currentTile)
    {
        var dirs = tiles[currentTile];
        var exits = dirs.Select(d => (x: current.x + d.x, y: current.y + d.y)).ToArray();
        if (exits.Any(e => e == from))
        {
            return exits.Where(e => e != from).ToArray();
        }
        return new (int x, int y)[] {};
    }

    public override Answer Two(string input)
    {
        return 0;
    }
}
