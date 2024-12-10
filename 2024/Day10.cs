using System.Drawing;

namespace AdventOfCode.Year2024;

public class Day10 : Solution
{
    public override string Example => @"";

    public override Answer One(string input)
    {
        var g = Grid.ParseFixed(input, '.');

        return g.Cells.Where(c => c.Value == '0').Select(c => c.Key).Sum(c => CountGoals(g, c));
    }

    private static readonly Point[] Dirs = new Point[]
    {
        new Point(-1, 0),
        new Point(1, 0),
        new Point(0, -1),
        new Point(0, 1)
    };

    private int CountGoals(Grid g, Point c)
    {
        var front = new HashSet<Point>();
        front.Add(c);
        var next = '1';

        while (front.Count > 0 && next <= '9')
        {
            var nextFront = new HashSet<Point>();
            foreach (var p in front)
            {
                foreach (var d in Dirs)
                {
                    if (g[p.Add(d)] == next)
                        nextFront.Add(p.Add(d));
                }
            }
            front = nextFront;
            next++;
        }

        return front.Count(p => g[p] == '9');
    }

    public override Answer Two(string input)
    {
        var g = Grid.ParseFixed(input, '.');

        return g.Cells.Where(c => c.Value == '0').Select(c => c.Key).Sum(c => CountRoutes(g, c));
    }

    private int CountRoutes(Grid g, Point c)
    {
        var routes = new List<List<Point>>();
        var route = new List<Point>() { c };
        routes.Add(route);
        var next = '1';

        while (routes.Count > 0 && next <= '9')
        {
            var nextRoutes = new List<List<Point>>();
            foreach (var r in routes)
            {
                var p = r[r.Count - 1];
                foreach (var d in Dirs)
                {
                    if (g[p.Add(d)] == next)
                    {
                        var nr = new List<Point>(r);
                        nr.Add(p.Add(d));
                        nextRoutes.Add(nr);
                    }
                }
            }
            routes = nextRoutes;
            next++;
        }

        return routes.Count;
    }

}
