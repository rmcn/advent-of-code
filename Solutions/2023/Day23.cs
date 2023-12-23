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

    record Edge(int Dest, int Steps);

    public override Answer Two(string input)
    {
        var g = Grid.Parse(input, '#');

        var start = new Point(1, 0);
        var end = new Point(g.Width - 2, g.Height - 1);

        // Identify all junctions (path cells with more than two neighbour paths)
        // These will be our graph vertices
        var junctions = new List<Point>() { start, end };

        foreach (var p in g.Cells.Where(c => c.Value != '#').Select(c => c.Key))
        {
            var isJunction = Neighbours(p).Count(n => g[n] != '#') > 2;

            if (isJunction)
                junctions.Add(p);
        }

        var graph = new List<List<Edge>>();

        // For each junction, work out what other junctions are reachable and how many steps they are away
        for (int i = 0; i < junctions.Count; i++)
        {
            var junction = junctions[i];
            var edges = new List<Edge>();

            // Use flood fill from junction to find neighbour junctions (and distance)
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
                        edges.Add(new Edge(junctions.IndexOf(n), steps + 1));
                    }
                    else
                    {
                        frontier.Enqueue((n, steps + 1));
                        seen.Add(n);
                    }
                }
            }

            graph.Add(edges);
            //Log($"{junction} [{string.Join(", ", edges)}]");
        }

        // Do a depth-first search from start (index 0), recording total steps in routes everytime we
        // reach end (index 0)
        bool[] visited = new bool[junctions.Count];
        visited[0] = true;
        var routes = new List<int>();

        Dfs(graph, 0, 0, visited, routes);

        return routes.Max();
    }

    private void Dfs(List<List<Edge>> graph, int current, int steps, bool[] visited, List<int> routes)
    {
        if (current == 1) // 1 is index of end
        {
            routes.Add(steps); // record how many steps to reach the end
            return;
        }

        foreach (var edge in graph[current])
        {
            if (visited[edge.Dest])
                continue;

            visited[edge.Dest] = true;
            Dfs(graph, edge.Dest, steps + edge.Steps, visited, routes);
            visited[edge.Dest] = false;
        }
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
