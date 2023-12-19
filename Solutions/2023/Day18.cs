using System.Drawing;

namespace AdventOfCode.Year2023;

public class Day18 : Solution
{
    /*
        ###
        # #
        # ###
        #   #
        #   #
        #####

        #####
        #   #
        #   #
        # ###
        # #
        ###

            ###
          ### #
          #   #
          ### #
            # #
          ### #
          #   #
          #####
    */
    public override string Example => @"R 2
D 2
R 2
D 3
L 4
U 5";

    record Step(char Dir, int Dist);

    Step ParseOne(string s)
    {
        var p = s.Split(' ');
        return new Step(p[0][0], p[1].Int());
    }

    public override Answer One(string input)
    {
        var steps = input
            .Lines()
            .Where(IsNotBlank)
            .Select(ParseOne)
            .ToList();

        var holes = new HashSet<Point>();

        var current = new Point(0, 0);
        holes.Add(current);

        foreach (var step in steps)
        {
            var dist = step.Dist;
            if (step.Dir == 'U')
                while (dist-- != 0) { current.Y -= 1; holes.Add(current); }
            else if (step.Dir == 'D')
                while (dist-- != 0) { current.Y += 1; holes.Add(current); }
            else if (step.Dir == 'L')
                while (dist-- != 0) { current.X -= 1; holes.Add(current); }
            else if (step.Dir == 'R')
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

    record Line(int X, int Y1, int Y2);

    public override Answer Two(string input)
    {
        var steps = input
            .Lines()
            .Where(IsNotBlank)
            .Select(ParseOne)
            .ToList();

        var lines = new List<Line>();

        var current = new Point(0, 0);
        foreach (var step in steps)
        {
            var dist = step.Dist;
            if (step.Dir == 'U')
            {
                var prev = current;
                while (dist-- != 0) { current.Y -= 1; }
                lines.Add(new Line(current.X, Min(prev.Y, current.Y), Max(prev.Y, current.Y)));
            }
            else if (step.Dir == 'D')
            {
                var prev = current;
                while (dist-- != 0) { current.Y += 1; }
                lines.Add(new Line(current.X, Min(prev.Y, current.Y), Max(prev.Y, current.Y)));
            }
            else if (step.Dir == 'L')
            {
                while (dist-- != 0) { current.X -= 1; }
            }
            else if (step.Dir == 'R')
            {
                while (dist-- != 0) { current.X += 1; }
            }
            else
                throw new Exception();
        }

        lines = lines.OrderBy(l => l.X).ThenBy(l => l.Y1).ToList();

        var xes = lines.Select(l => l.X).Distinct().ToList();

        var front = lines.Where(l => l.X == xes[0]).ToList();

        long total = 0;
        total += front.Select(l => l.Y2 - l.Y1 + 1).Sum();
        for (int i = 1; i < xes.Count; i++)
        {
            var xs = xes[i - 1];
            var xe = xes[i];
            long width = xe - xs - 1;

            total += front.Select(l => width * (l.Y2 - l.Y1 + 1)).Sum();

            // Add or extend now, split or reduce after
            var newLines = lines.Where(l => l.X == xe).ToList();
            var reducedBy = 0;
            foreach (var line in newLines)
            {
                var extend = front.Where(fl => fl.Y1 == line.Y2 || fl.Y2 == line.Y1).ToList();
                var reduce = front.Where(fl => fl.Y1 == line.Y1 || fl.Y2 == line.Y2).ToList();
                var split = front.Where(fl => fl.Y1 < line.Y1 && fl.Y2 > line.Y2).ToList();

                if (extend.Count > 2) throw new Exception();
                if (reduce.Count > 1) throw new Exception();
                if (split.Count > 1) throw new Exception();

                var match = (extend.Count > 0 ? 1 : 0) + (reduce.Count > 0 ? 1 : 0) + (split.Count > 0 ? 1 : 0);
                if (match > 1) throw new Exception();

                if (extend.Count > 0)
                {
                    front = front.Except(extend).ToList();
                    extend.Add(line);
                    front.Add(new Line(xe, extend.Min(l => l.Y1), extend.Max(l => l.Y2)));
                }
                else if (reduce.Count > 0)
                {
                    front = front.Except(reduce).ToList();
                    if (reduce[0].Y1 == line.Y1 && reduce[0].Y2 == line.Y2)
                    {
                        // entirely gone
                        reducedBy += line.Y2 - line.Y1 + 1;
                    }
                    else if (reduce[0].Y1 == line.Y1)
                    {
                        var shortened = new Line(xe, Min(line.Y2, reduce[0].Y2), Max(line.Y2, reduce[0].Y2));
                        reducedBy += shortened.Y1 - line.Y1;
                        front.Add(shortened);
                    }
                    else if (reduce[0].Y2 == line.Y2)
                    {
                        var shortened = new Line(xe, Min(line.Y1, reduce[0].Y1), Max(line.Y1, reduce[0].Y1));
                        reducedBy += line.Y2 - shortened.Y2;
                        front.Add(shortened);
                    }
                }
                else if (split.Count > 0)
                {
                    front = front.Except(split).ToList();
                    front.Add(new Line(xe, split[0].Y1, line.Y1));
                    front.Add(new Line(xe, line.Y2, split[0].Y2));
                    reducedBy = line.Y2 - line.Y1 - 1;
                }
                else
                {
                    front.Add(line);
                }
            }
            total += front.Select(l => l.Y2 - l.Y1 + 1).Sum() + reducedBy;
        }

        return total;
    }
}
