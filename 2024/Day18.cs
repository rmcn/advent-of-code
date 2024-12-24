using System.Drawing;

namespace AdventOfCode.Year2024;

public class Day18 : Solution
{
    public override string Example => @"5,4
4,2
4,5
3,0
2,1
6,3
2,4
1,5
0,6
3,3
2,6
5,1
1,2
5,5
2,5
6,5
1,4
0,4
6,4
1,1
6,1
1,0
0,5
1,6
2,0";

    public override Answer One(string input)
    {
        var k = input.Lines().Where(IsNotBlank).Select(l => new Point(l.Ints()[0], l.Ints()[1])).ToList();
        var g = k.Count < 1000
            ? Grid.Fixed(7, 7, '.', '#')
            : Grid.Fixed(71, 71, '.', '#');

        var limit = k.Count < 1000 ? 12 : 1024;

        return CountSteps(k.Take(limit).ToList(), g);
    }
    
    public override Answer Two(string input)
    {
        var k = input.Lines().Where(IsNotBlank).Select(l => new Point(l.Ints()[0], l.Ints()[1])).ToList();
        var g = k.Count < 1000
            ? Grid.Fixed(7, 7, '.', '#')
            : Grid.Fixed(71, 71, '.', '#');

        var limit = k.Count < 1000 ? 12 : 1024;

        while (CountSteps(k.Take(limit).ToList(), g) != -1)
        {
            limit += 1;
        }
        
        return $"{k[limit-1].X},{k[limit-1].Y}";
    }

    private static int CountSteps(List<Point> k, Grid g)
    {
        var end = new Point(g.Width - 1, g.Height - 1);

        foreach (var p in k)
        {
            g[p] = '#';
        }

        int steps = 0;
        var front = new List<Point>() { new Point(0, 0) };
        var seen = new HashSet<Point>() { new(0, 0) };

        while (front.Count > 0)
        {
            var next = new List<Point>();
            foreach (var p in front)
            {
                foreach (var d in new[] { new Point(-1, 0), new Point(0, -1), new Point(0, 1), new Point(1, 0) })
                {
                    var n = p.Add(d);

                    if (g[n] != '.')
                        continue;

                    if (seen.Contains(n))
                        continue;

                    if (n == end)
                        return steps + 1;

                    next.Add(n);
                    seen.Add(n);
                }
            }
            front = next;
            steps += 1;
        }

        return -1;
    }
}
