using System.Drawing;

namespace AdventOfCode.Year2024;

public class Day14 : Solution
{
    public override string Example => @"";

    public override Answer One(string input)
    {
        var robots = input.Lines().Where(IsNotBlank).Select(s => s.Ints());

        int width = robots.Count() <  20 ? 11 : 101;
        int height = robots.Count() <  20 ? 7 : 103;

        var finalPos = robots
            .Select(r => new Point(r[0] + 100 * r[2], r[1] + 100 * r[3]))
            .Select(p => Grid.Wrap(p, width, height))
            .ToList();

        var midX = (width - 1) / 2;
        var midY = (height - 1) / 2;

        var q1 = finalPos.Count(p => p.X < midX && p.Y < midY);
        var q2 = finalPos.Count(p => p.X > midX && p.Y < midY);
        var q3 = finalPos.Count(p => p.X < midX && p.Y > midY);
        var q4 = finalPos.Count(p => p.X > midX && p.Y > midY);

        return q1 * q2 * q3 * q4;
    }

    public override Answer Two(string input)
    {
        var robots = input.Lines().Where(IsNotBlank).Select(s => s.Ints());

        if (robots.Count() <  20)
            return 0;

        int width = robots.Count() <  20 ? 11 : 101;
        int height = robots.Count() <  20 ? 7 : 103;

        var gridInput = string.Join('\n', MoreEnumerable.Sequence(0, height-1).Select(_ => new string('.', width)));

        for (int i = 0; i <= 10000; i++)
        {
            var finalPos = robots
                .Select(r => new Point(r[0] + i * r[2], r[1] + i * r[3]))
                .Select(p => Grid.Wrap(p, width, height))
                .ToList();

            if (finalPos.GroupBy(p => p.Y).Any(g => g.Count() > 30))
            {
                var g = Grid.ParseFixed(gridInput, '.');
                foreach (var p in finalPos)
                {
                    g[p] = '#';
                }
                Console.WriteLine($"Step {i}\n{g.Dump()}\n\n");
                Console.ReadKey();
            }
        }

        return 0;
    }
}
