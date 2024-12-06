using System.Drawing;

namespace AdventOfCode.Year2020;

public class Day03 : Solution
{
    public override string Example => @"";

    public override Answer One(string input)
    {
        var g = Grid.ParseInfinite(input);

        return CountTrees(g, new Point(3, 1));
    }

    private static long CountTrees(Grid g, Point delta)
    {
        var p = new Point(0, 0);

        long t = 0;
        while (p.Y < g.Height)
        {
            if (g[p] == '#')
                t++;

            p = p.Add(delta);
        }

        return t;
    }

    public override Answer Two(string input)
    {
        var g = Grid.ParseInfinite(input);

        return
            CountTrees(g, new Point(1, 1))
            * CountTrees(g, new Point(3, 1))
            * CountTrees(g, new Point(5, 1))
            * CountTrees(g, new Point(7, 1))
            * CountTrees(g, new Point(1, 2));
    }
}
