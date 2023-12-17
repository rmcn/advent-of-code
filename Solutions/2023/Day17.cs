using System.Drawing;
using System.Runtime;

namespace AdventOfCode.Year2023;

public class Day17 : Solution
{
    public override string Example => @"";

    class State
    {
        public Ray Ray { get; set; } = new Ray(new Point(0,0), Dir.Right);
        public int Steps { get; set; }
        public int Loss { get; set; }
        public List<Ray> Route { get; set; } =  new();

        public State? Next(Grid<int> g)
        {
            var state = new State
            {
                Steps = Steps + 1,
                Loss = Loss,
                Ray = Ray.Next()
            };

            var loss = g[state.Ray.Loc];
            state.Loss += loss;
            //state.Route.AddRange(Route);
            //state.Route.Add(state.Ray);
            return loss == -1 ? null : state;
        }

        public State? Left(Grid<int> g)
        {
            var state = new State
            {
                Steps = 1,
                Loss = Loss
            };

            if (Ray.Dir == Dir.Up)
                state.Ray = Ray.Left();
            else if (Ray.Dir == Dir.Left)
                state.Ray = Ray.Down();
            else if (Ray.Dir == Dir.Down)
                state.Ray = Ray.Right();
            else if (Ray.Dir == Dir.Right)
                state.Ray = Ray.Up();

            var loss = g[state.Ray.Loc];
            state.Loss += loss;
            //state.Route.AddRange(Route);
            //state.Route.Add(state.Ray);
            return loss == -1 ? null : state;
        }

        public State? Right(Grid<int> g)
        {
            var state = new State
            {
                Steps = 1,
                Loss = Loss
            };

            if (Ray.Dir == Dir.Up)
                state.Ray = Ray.Right();
            else if (Ray.Dir == Dir.Left)
                state.Ray = Ray.Up();
            else if (Ray.Dir == Dir.Down)
                state.Ray = Ray.Left();
            else if (Ray.Dir == Dir.Right)
                state.Ray = Ray.Down();

            var loss = g[state.Ray.Loc];
            state.Loss += loss;
            state.Route.AddRange(Route);
            state.Route.Add(state.Ray);
            return loss == -1 ? null : state;
        }
    }

    public override Answer One(string input)
    {
        var g = Grid.Parse(input, c => int.Parse(c.ToString()), -1);

        var states = new PriorityQueue<State, int>();
        var firstState = new State { Ray = new Ray(new Point(0,0), Dir.Right), Loss = 0, Steps = 1 };
        states.Enqueue(firstState, firstState.Loss);

        var seen = new HashSet<Ray>();

        while(states.Count > 0)
        {
            //var x = states.UnorderedItems.OrderBy(v => v.Priority).Select(v => v.Element).Select(s => $"({s.Ray.Loc.X},{s.Ray.Loc.Y}):{s.Ray.Dir}={s.Loss}");
            //LogEx("\n\n" + string.Join(" ", x));

            var state = states.Dequeue();
            seen.Add(state.Ray);



            if (state.Ray.Loc.X == g.Width - 1 && state.Ray.Loc.Y == g.Height -1)
            {
                //LogEx(string.Join('\n', state.Route));
                return state.Loss;
            }

            if (state.Steps < 3) {
                var next = state.Next(g);
                if (next != null && !seen.Contains(next.Ray)) states.Enqueue(next, next.Loss);
            }

            var left = state.Left(g);
            if (left != null && !seen.Contains(left.Ray)) states.Enqueue(left, left.Loss);

            var right = state.Right(g);
            if (right != null && !seen.Contains(right.Ray)) states.Enqueue(right, right.Loss);
        }

        return 0;
    }

    public override Answer Two(string input)
    {
        return 0;
    }
}
