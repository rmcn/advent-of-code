using System.Drawing;

namespace AdventOfCode.Year2023;

public class Day23 : Solution
{
    public override string Example => @"";

    public class State
    {
        public Point Loc { get; set; }
        public int Steps { get; set; }
        public HashSet<Point> Visited { get; set; }
        public State(Point loc, int steps, HashSet<Point> visited)
        {
            Loc = loc;
            Steps = steps;

            Visited = new HashSet<Point>(visited)
            {
                loc
            };
        }
    }

    public override Answer One(string input)
    {
        return 0;

        var g = Grid.Parse(input, '#');

        var start = new Point(1, 0);
        var end = new Point(g.Width - 2, g.Height - 1);

        var frontier = new Queue<State>();
        frontier.Enqueue(new State(start, 0, new HashSet<Point>()));

        var routes = new List<State>();

        while (frontier.Count > 0)
        {
            var current = frontier.Dequeue();

            if (current.Loc == end)
            {
                routes.Add(current);
                continue;
            }

            var candidates = new List<Point>();

            var down = current.Loc with { Y = current.Loc.Y + 1 };
            if (g[down] != '#' && g[down] != '^' && !current.Visited.Contains(down)) candidates.Add(down);

            var up = current.Loc with { Y = current.Loc.Y - 1 };
            if (g[up] != '#' && g[up] != 'v' && !current.Visited.Contains(up)) candidates.Add(up);

            var left = current.Loc with { X = current.Loc.X - 1 };
            if (g[left] != '#' && g[left] != '>' && !current.Visited.Contains(left)) candidates.Add(left);

            var right = current.Loc with { X = current.Loc.X + 1 };
            if (g[right] != '#' && g[right] != '<' && !current.Visited.Contains(right)) candidates.Add(right);                

            foreach (var next in candidates)
            {
                frontier.Enqueue(new State(next, current.Steps + 1, current.Visited));
            }
        }

        /*
        var longest = routes.OrderByDescending(r => r.Steps).First();

        for (int y = 0; y < g.Height; y++){
            for (int x = 0; x < g.Height; x++)
                Console.Write(longest.Visited.Contains(new Point(x,y)) ? 'X' : '.');
            Console.WriteLine();
        }*/

        return routes.Max(r => r.Steps);
    }

    record Edge(Point Dest, int Steps);

    public override Answer Two(string input)
    {
        var g = Grid.Parse(input, '#');

        var start = new Point(1, 0);
        var end = new Point(g.Width - 2, g.Height - 1);

        var junctions = new List<Point>() { start, end };

        foreach (var p in g.Cells.Where(c => c.Value != '#').Select(c => c.Key))
        {
            var isJunction = Neighbours(p).Count(n => g[n] != '#') > 2;

            if (isJunction)
                junctions.Add(p);
        }

        var graph = new Dictionary<Point, List<Edge>>();

        foreach (var junction in junctions)
        {
            var edges = new List<Edge>();
            var seen = new HashSet<Point> { junction };

            var frontier = new Queue<(Point p, int steps)>();
            frontier.Enqueue((junction, 0));

            while (frontier.Count > 0)
            {
                var (p, steps) = frontier.Dequeue();
                var neighbours = Neighbours(p).Where(n => g[n] != '#' && !seen.Contains(n)).ToList();

                foreach (var n in neighbours)
                {
                    if (junctions.Contains(n))
                    {
                        edges.Add(new Edge(n, steps + 1));
                    }
                    else
                    {
                        frontier.Enqueue((n, steps + 1));
                        seen.Add(n);
                    }
                }
            }

            graph.Add(junction, edges);
            //Log($"{junction} [{string.Join(", ", edges)}]");
        }


        var front = new Queue<State>();
        front.Enqueue(new State(start, 0, new HashSet<Point>()));

        var routes = new List<State>();

        while (front.Count > 0)
        {
            var current = front.Dequeue();

            if (current.Loc == end)
            {
                routes.Add(current);
                continue;
            }

            foreach (var edge in graph[current.Loc].Where(e => !current.Visited.Contains(e.Dest)))
            {
                front.Enqueue(new State(edge.Dest, current.Steps + edge.Steps, current.Visited));
            }
        }

        /*
        var longest = routes.OrderByDescending(r => r.Steps).First();

        for (int y = 0; y < g.Height; y++){
            for (int x = 0; x < g.Height; x++)
                Console.Write(longest.Visited.Contains(new Point(x,y)) ? 'X' : '.');
            Console.WriteLine();
        }*/

        return routes.Max(r => r.Steps);
    }

    private static Point[] Neighbours(Point p)
    {
        return new[]
        {
                p with { Y = p.Y + 1 },
                p with { Y = p.Y - 1 },
                p with { X = p.X - 1 },
                p with { X = p.X + 1 }
            };
    }
}
