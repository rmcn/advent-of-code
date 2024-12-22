using System.Drawing;

namespace AdventOfCode.Year2024;

public class Day16 : Solution
{
    public override string Example => @"";

    private readonly static Point[] Dirs = new []
    {
        new Point(1, 0),
        new Point(0, -1),
        new Point(-1, 0),
        new Point(0, 1)
    };

    record State(Point Pos, int Dir, int Score, HashSet<Point> Path)
    {
        public State Left() => this with { Dir = Dir == 0 ? 3 : Dir - 1, Score = Score + 1000, Path = new HashSet<Point>(Path) };
        public State Right() => this with { Dir = Dir == 3 ? 0 : Dir + 1, Score = Score + 1000, Path = new HashSet<Point>(Path) };
        public State Forward()
        {
            var delta = Dir switch
            {
                0 => new Point(1, 0),
                1 => new Point(0, -1),
                2 => new Point(-1, 0),
                3 => new Point(0, 1),
                _ => throw new Exception()
            };

            var path = new HashSet<Point>(Path);
            path.Add(Pos.Add(delta));

            return this with
            {
                Pos = Pos.Add(delta),
                Score = Score + 1,
                Path = path
            };
        }
    }

    public override Answer One(string input)
    {
        var g = Grid.ParseFixed(input, '#');

        var start = g.Cells.Single(c => c.Value == 'S').Key;
        var seen = new Dictionary<(Point, int), int>();
        seen.Add((start, 0), 0);

        var front = new PriorityQueue<State, int>();
        front.Enqueue(new State(start, 0, 0, new HashSet<Point> { start }), 0);

        while (true)
        {
            var state = front.Dequeue();

            var next = new []
            {
                state.Forward(),
                state.Left(),
                state.Right()
            };

            foreach (var n in next)
            {
                if (g[n.Pos] == '#')
                    continue;

                if (g[n.Pos] == 'E')
                    return n.Score;

                var key = (n.Pos, n.Dir);
                if (!seen.ContainsKey(key))
                {
                    seen.Add(key, n.Score);
                    front.Enqueue(n, n.Score);
                }
                else if (seen[key] > n.Score)
                {
                    seen[key] = n.Score;
                    front.Enqueue(n, n.Score);
                } 
            }
        }
    }

    public override Answer Two(string input)
    {
        var g = Grid.ParseFixed(input, '#');

        var start = g.Cells.Single(c => c.Value == 'S').Key;
        var seen = new Dictionary<(Point, int), int>();
        seen.Add((start, 0), 0);

        var front = new PriorityQueue<State, int>();
        front.Enqueue(new State(start, 0, 0, new HashSet<Point> { start }), 0);

        var completed = new List<State>();

        while (front.Count > 0)
        {
            var state = front.Dequeue();

            var next = new []
            {
                state.Forward(),
                state.Left(),
                state.Right()
            };

            foreach (var n in next)
            {
                if (completed.Count > 0 && n.Score > completed[0].Score)
                    continue;

                if (g[n.Pos] == '#')
                    continue;

                if (g[n.Pos] == 'E')
                {
                    completed.Add(n);
                    continue;
                }

                var key = (n.Pos, n.Dir);
                if (!seen.ContainsKey(key))
                {
                    seen.Add(key, n.Score);
                    front.Enqueue(n, n.Score);
                }
                else if (seen[key] >= n.Score)
                {
                    seen[key] = n.Score;
                    front.Enqueue(n, n.Score);
                } 
            }
        }

        var allVisited = new HashSet<Point>();
        foreach (var c in completed)
        {
            allVisited.UnionWith(c.Path);
        }
        return allVisited.Count;
    }
}
