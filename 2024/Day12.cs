using System.Drawing;

namespace AdventOfCode.Year2024;

public class Day12 : Solution
{
    public override string Example => @"";

    private readonly static Point[] Dirs =
    {
        new Point(-1, 0),
        new Point(1, 0),
        new Point(0, -1),
        new Point(0, 1)
    };

    public override Answer One(string input) => Price(input, Perim);
    public override Answer Two(string input) => Price(input, Sides);

    public int Price(string input, Func<HashSet<Point>, int> length)
    {
        int t = 0;

        var g = Grid.ParseFixed(input, '.');

        var seen = new HashSet<Point>();

        foreach (var c in g.Cells)
        {
            if (seen.Contains(c.Key))
                continue;

            var neig = new HashSet<Point> { c.Key };
            while (true)
            {
                var next = neig.SelectMany(p => Dirs.Select(d => p.Add(d))).Where(p => g[p] == c.Value && !neig.Contains(p)).ToHashSet();

                if (next.Count == 0)
                    break;

                neig.UnionWith(next);
            }

            t += length(neig) * neig.Count;

            seen.UnionWith(neig);
        }

        return t;
    }

    private int Perim(HashSet<Point> neig)
    {
        return neig.Sum(p => Dirs.Count(d => !neig.Contains(p.Add(d))));
    }

    private int Sides(HashSet<Point> neig)
    {
        int s = 0;
        foreach (var p in neig)
        {
            // start of top left-to-right - nothing above and either nothing to left or something above-left
            if (!neig.Contains(p.Add(0, -1)) && (!neig.Contains(p.Add(-1, 0)) || neig.Contains(p.Add(-1, -1))))
                s++;

            // start of bottom left-to-right - nothing under and either nothing to left or something below-left
            if (!neig.Contains(p.Add(0, 1)) && (!neig.Contains(p.Add(-1, 0)) || neig.Contains(p.Add(-1, 1))))
                s++;

            // start of left top-to-bottom - nothing left and either nothing to above or something above-left
            if (!neig.Contains(p.Add(-1, 0)) && (!neig.Contains(p.Add(0, -1)) || neig.Contains(p.Add(-1, -1))))
                s++;

            // start of right top-to-bottom - nothing right and either nothing to above or something above-righy
            if (!neig.Contains(p.Add(1, 0)) && (!neig.Contains(p.Add(0, -1)) || neig.Contains(p.Add(1, -1))))
                s++;
        }
        return s;
    }

}
