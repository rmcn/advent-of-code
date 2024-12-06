using System.Drawing;
using System.Net.Security;
using System.Numerics;

namespace AdventOfCode.Year2023;

public class Day21 : Solution
{
    public override Answer One(string input)
    {
        var goalSteps = input.Length < 1000 ? 6 : 64;

        var g = Grid.Parse(input, '#');

        var frontier = new List<Point>
        {
            g.Cells.Where(c => c.Value == 'S').Select(c => c.Key).Single()
        };

        for (int step = 1; step <= goalSteps; step++)
        {
            frontier = frontier
                .SelectMany(p => new[]
                {
                    p with { X = p.X - 1 },
                    p with { X = p.X + 1 },
                    p with { Y = p.Y - 1 },
                    p with { Y = p.Y + 1 }
                })
                .Where(p => g[p] != '#')
                .Distinct()
                .ToList();
        }

        return frontier.Count;
    }

    const long PartTwoGoalSteps = 26501365;

    public override Answer Two(string input)
    {
        if (input.Length < 1000) return 0; // Skip example input

        var g = Grid.Parse(input, '#', infinite: true);

        var frontier = new HashSet<Point>
        {
            g.Cells.Where(c => c.Value == 'S').Select(c => c.Key).Single()
        };

        var sequence = new List<int>() { 1 };

        while (sequence.Count < 600)
        {
            frontier = Step(g, frontier);
            sequence.Add(frontier.Count);
            if (sequence.Count % 10 == 0)
                Console.Write(".");
        }
        Console.WriteLine();

        var (skip, every, a, b, c) = DeriveQuadratic(sequence);

        var n = (PartTwoGoalSteps - skip) / every + 1;

        return a * n * n + b * n + c;
    }

    private (int skip, int every, int a, int b, int c) DeriveQuadratic(List<int> counts)
    {
        for (int skip = 0; skip <= counts.Count; skip++)
        {
            for (int every = 1; every <= counts.Count; every++)
            {
                // Only want skip & every values that neatly divide into our goal steps
                if ((PartTwoGoalSteps - skip) % every != 0)
                    continue;

                var values = counts.Skip(skip).TakeEvery(every).ToList();
                if (values.Count < 5) // want at least 5 values to give us three second-differences for comparison
                    continue;

                var diffs = values.Zip(values.Skip(1)).Select(p => p.Second - p.First).ToList();
                var diffdiffs = diffs.Zip(diffs.Skip(1)).Select(p => p.Second - p.First).ToList();

                if (diffdiffs.Distinct().Count() == 1)
                {
                    Log($"Skip {skip} take every {every} has constant 2nd order diff of {diffdiffs.First()}, first 5 vals [{string.Join(", ", values.Take(5))}]");
                    var a = diffdiffs.First() / 2;
                    var b = diffs.First() - 3 * a;
                    var c = values.First() - a - b;
                    return (skip, every, a, b, c);
                }
            }
        }
        throw new Exception("No quadractic with integer coefficients found");
    }

    private static HashSet<Point> Step(Grid g, HashSet<Point> frontier)
    {
        var next = new HashSet<Point>();

        foreach (var p in frontier)
        {
            var l = p with { X = p.X - 1 };
            var r = p with { X = p.X + 1 };
            var u = p with { Y = p.Y - 1 };
            var d = p with { Y = p.Y + 1 };

            if (g[l] != '#') next.Add(l);
            if (g[r] != '#') next.Add(r);
            if (g[u] != '#') next.Add(u);
            if (g[d] != '#') next.Add(d);
        }

        return next;
    }
}
