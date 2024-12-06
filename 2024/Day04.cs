using System.Drawing;

namespace AdventOfCode.Year2024;

public class Day04 : Solution
{
    public override string FilePath => MetaHelper.FilePath();
    public override string Example => @"";

    public override Answer One(string input)
    {
        var g = Grid.ParseFixed(input, '.');

        int t = 0;

        for (int x = 0; x < g.Width; x++)
        {
            for (int y = 0; y < g.Height; y++)
            {
                var p = new Point(x, y);
                if (XmasAt(g, p, new Point(1, 0)))
                    t++;
                if (XmasAt(g, p, new Point(-1, 0)))
                    t++;
                if (XmasAt(g, p, new Point(0, 1)))
                    t++;
                if (XmasAt(g, p, new Point(0, -1)))
                    t++;
                if (XmasAt(g, p, new Point(1, 1)))
                    t++;
                if (XmasAt(g, p, new Point(-1, -1)))
                    t++;
                if (XmasAt(g, p, new Point(-1, 1)))
                    t++;
                if (XmasAt(g, p, new Point(1, -1)))
                    t++;
            }
        }

        return t;
    }

    private bool XmasAt(Grid g, Point p, Point delta)
    {
        if (g[p] != 'X') return false;
        p = p.Add(delta);
        if (g[p] != 'M') return false;
        p = p.Add(delta);
        if (g[p] != 'A') return false;
        p = p.Add(delta);
        if (g[p] != 'S') return false;
        return true;
    }

    public override Answer Two(string input)
    {
        var g = Grid.ParseFixed(input, '.');

        int t = 0;

        for (int x = 0; x < g.Width; x++)
        {
            for (int y = 0; y < g.Height; y++)
            {
                var p = new Point(x, y);
                if (MasDownAt(g, p) && MasUpAt(g, p))
                    t++;
            }
        }

        return t;
    }

    private bool MasDownAt(Grid g, Point p)
    {
        if (g[p] != 'A') return false;
        var c1 = g[p.Add(new Point(-1, -1))];
        var c2 = g[p.Add(new Point(1, 1))];
        if (c1 == 'M' && c2 == 'S') return true;
        if (c1 == 'S' && c2 == 'M') return true;
        return false;
    }
    private bool MasUpAt(Grid g, Point p)
    {
        if (g[p] != 'A') return false;
        var c1 = g[p.Add(new Point(-1, 1))];
        var c2 = g[p.Add(new Point(1, -1))];
        if (c1 == 'M' && c2 == 'S') return true;
        if (c1 == 'S' && c2 == 'M') return true;
        return false;
    }
}
