using System.Drawing;

namespace AdventOfCode.Year2024;

public class Day06 : Solution
{
    public override string FilePath => MetaHelper.FilePath();
    public override string Example => @"";

    public override Answer One(string input)
    {
        var g = Grid.ParseFixed(input, '*');

        var pos = g.Cells.Single(c => c.Value == '^').Key;
        var seen = Visits(g, pos);

        return seen.Count;
    }

    private HashSet<Point> Visits(Grid g, Point pos)
    {
        var dir = 0;
        var dirs = new[]
        {
            new Point(0, -1),
            new Point(1, 0),
            new Point(0, 1),
            new Point(-1, 0)
        };

        var seen = new HashSet<Point>();

        while (g[pos] != '*')
        {
            seen.Add(pos);
            var next = pos.Add(dirs[dir]);
            if (g[next] == '#')
            {
                dir = (dir + 1) % dirs.Length;
                next = pos.Add(dirs[dir]);
            }
            pos = next;
        }

        return seen;
    }

    public override Answer Two(string input)
    {
        var g = Grid.ParseFixed(input, '*');

        var start = g.Cells.Single(c => c.Value == '^').Key;

        var seen = Visits(g, start);


        int t = 0;
        foreach (var change in seen)
        {
            if (change != start)
            {
                g[change] = 'O';
                if (Loops(g, start))
                    t++;
                g[change] = '.';
            }
        }

        return t; 
    }

    private bool Loops(Grid g, Point pos)
    {
        var dir = 0;
        var dirs = new[]
        {
            new Point(0, -1),
            new Point(1, 0),
            new Point(0, 1),
            new Point(-1, 0)
        };

        var seen = new HashSet<(Point, int)>();

        while (g[pos] != '*' && !seen.Contains((pos, dir)))
        {
            seen.Add((pos, dir));
            var next = pos.Add(dirs[dir]);
            while (g[next] == '#' || g[next] == 'O')
            {
                dir = (dir + 1) % dirs.Length;
                seen.Add((pos, dir));
                next = pos.Add(dirs[dir]);
            }
            pos = next;
        }

        return g[pos] != '*';
    }
}
