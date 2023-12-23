﻿using System.Drawing;

namespace AdventOfCode.Year2023;

public class Day17 : Solution
{
    public override string Example => @"";

    public enum Face { North, East, South, West };

    public static Point[] Delta = new []
    {
        new Point(0, -1),
        new Point(1, 0),
        new Point(0, +1),
        new Point(-1, 0)
    };

    public record State(Point Loc, Face Dir, int Steps);

    public State Move(State s, Face dir, int steps)
    {
        return s with 
        {
            Dir = dir,
            Loc = s.Loc.Add(Delta[(int)dir]),
            Steps = steps
        };
    }

    public State Left(State s) => Move(s, s.Dir == Face.North ? Face.West : s.Dir - 1, 1);
    public State Right(State s) => Move(s, s.Dir == Face.West ? Face.North : s.Dir + 1, 1);
    public State Forward(State s) => Move(s, s.Dir, s.Steps + 1);

    public override Answer One(string input)
    {
        var g = Grid.Parse(input, c => int.Parse(c.ToString()), -1);

        var frontier = new PriorityQueue<State, int>();
        var start = new State(new Point(0, 0), Face.East, 0);
        frontier.Enqueue(start, 0);

        var costSoFar = new Dictionary<State, int>
        {
            [start] = 0
        };

        while (frontier.Count > 0)
        {
            var current = frontier.Dequeue();

            if (current.Loc.X == g.Width - 1 && current.Loc.Y == g.Height - 1)
                return costSoFar[current];

            var successors = new[] { Left(current), Right(current), Forward(current) }.Where(s => s.Steps <= 3 && g[s.Loc] != -1).ToList();

            foreach (var next in successors)
            {
                var newCost = costSoFar[current] + g[next.Loc];

                if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
                {
                    costSoFar[next] = newCost;
                    var priority = newCost + Abs(g.Width - 1 - next.Loc.X) + Abs(g.Height - 1 - next.Loc.Y);
                    frontier.Enqueue(next, priority);
                }
            }
        }
        return 0;
    }

    public override Answer Two(string input)
    {
        return 0;
    }
}
