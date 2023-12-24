namespace AdventOfCode.Year2023;

public class Day24 : Solution
{
    public override string Example => @"";

    public record V3(double X, double Y, double Z);

    public class Hailstone
    {
        public V3 P { get; set; }
        public V3 V { get; set; }
        public double M { get; set; }

        public Hailstone(V3 p, V3 v)
        {
            P = p;
            V = v;
            M = V.Y / V.X;
        }

        public bool InFutureXY(V3 f)
        {
            var xInFuture = (V.X < 0) ? f.X <= P.X : f.X >= P.X;
            var yInFuture = (V.Y < 0) ? f.Y <= P.Y : f.Y >= P.Y;
            return xInFuture && yInFuture;
        }

        public bool Parallel(Hailstone other) => M == other.M;

        public V3 Intersect(Hailstone other)
        {
            var x1 = P.X; var y1 = P.Y;
            var x2 = P.X + V.X * 100000.0; var y2 = P.Y + V.Y * 100000.0;

            var x3 = other.P.X; var y3 = other.P.Y;
            var x4 = other.P.X + other.V.X * 100000.0; var y4 = other.P.Y + other.V.Y * 100000.0;

            var px =
                ((x1 * y2 - y1 * x2) * (x3 - x4) - (x1 - x2) * (x3 * y4 - y3 * x4))
                /
                ((x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4));

            var py =
                ((x1 * y2 - y1 * x2) * (y3 - y4) - (y1 - y2) * (x3 * y4 - y3 * x4))
                /
                ((x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4));

            return new V3(px, py, 0);
        }
    }

    public Hailstone ParseHailstone(string s)
    {
        var parts = s.Split('@');
        var p = parts[0].Split(',').Select(a => double.Parse(a.Trim())).ToList();
        var v = parts[1].Split(',').Select(a => double.Parse(a.Trim())).ToList();

        return new Hailstone(new V3(p[0], p[1], p[2]), new V3(v[0], v[1], v[2]));
    }


    public override Answer One(string input)
    {
        var hailstones = input.Lines().Where(IsNotBlank).Select(ParseHailstone).ToList();
        double min = hailstones.Count == 5 ? 7 : 200000000000000;
        double max = hailstones.Count == 5 ? 27 : 400000000000000;

        int t = 0;
        for (int i = 0; i < hailstones.Count - 1; i++)
        {
            var a = hailstones[i];
            for (int j = i + 1; j < hailstones.Count; j++)
            {
                var b = hailstones[j];

                if (a.Parallel(b))
                    continue;

                var p = a.Intersect(b);

                if (p.X >= min && p.X <= max && p.Y >= min && p.Y <= max && a.InFutureXY(p) && b.InFutureXY(p))
                    t++;
            }
        }

        return t;
    }

    public override Answer Two(string input)
    {
        return 0;
    }
}
