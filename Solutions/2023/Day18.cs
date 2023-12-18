using System.Drawing;

namespace AdventOfCode.Year2023;

public class Day18 : Solution
{
    public override string Example => @"";

    public override Answer One(string input)
    {

        var holes = new HashSet<Point>();

        var current = new Point(0, 0);
        holes.Add(current);

        foreach (var line in input.Lines().Where(IsNotBlank))
        {
            var dir = line[0];
            var dist = line.Split(' ')[1].Int();
            if (dir == 'U')
                while (dist-- != 0) { current.Y -= 1; holes.Add(current); }
            else if (dir == 'D')
                while (dist-- != 0) { current.Y += 1; holes.Add(current); }
            else if (dir == 'L')
                while (dist-- != 0) { current.X -= 1; holes.Add(current); }
            else if (dir == 'R')
                while (dist-- != 0) { current.X += 1; holes.Add(current); }
            else
                throw new Exception();
        }

        var flood = new Queue<Point>();
        flood.Enqueue(new Point(1, 1));

        while (flood.Count > 0)
        {
            var p = flood.Dequeue();

            if (!holes.Contains(p))
            {
                holes.Add(p);
                flood.Enqueue(new Point(p.X + 1, p.Y));
                flood.Enqueue(new Point(p.X - 1, p.Y));
                flood.Enqueue(new Point(p.X, p.Y + 1));
                flood.Enqueue(new Point(p.X, p.Y - 1));
            }
        }

        return holes.Count;
    }

    public override Answer Two(string input)
    {
        return 0;
    }
}
