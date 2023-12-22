namespace AdventOfCode.Year2023;

public class Day22 : Solution
{
    public override string Example => @"";

    public record V3(int X, int Y, int Z);
    public class Brick
    {
        public V3 A { get; set; }
        public V3 B { get; set; }
        public Brick(V3 a, V3 b)
        {
            A = a;
            B = b;

            MinX = Min(A.X, B.X);
            MaxX = Max(A.X, B.X);
            MinY = Min(A.Y, B.Y);
            MaxY = Max(A.Y, B.Y);
            MinZ = Min(A.Z, B.Z);
            MaxZ = Max(A.Z, B.Z);
        }

        public int MinX { get; set; }
        public int MaxX { get; set; }
        public int MinY { get; set; }
        public int MaxY { get; set; }
        public int MinZ { get; set; }
        public int MaxZ { get; set; }

        public bool Contains(V3 p)
        {
            return p.X >= MinX && p.X <= MaxX
                && p.Y >= MinY && p.Y <= MaxY
                && p.Z >= MinZ && p.Z <= MaxZ;
        }

        public bool Intersects(Brick other)
        {
            for (int x = other.MinX; x <= other.MaxX; x++)
                for (int y = other.MinY; y <= other.MaxY; y++)
                    for (int z = other.MinZ; z <= other.MaxZ; z++)
                        if (Contains(new V3(x, y, z)))
                            return true;
            return false;
        }

        public bool CanFallRealativeTo(Brick other)
        {
            for (int x = MinX; x <= MaxX; x++)
                for (int y = MinY; y <= MaxY; y++)
                    if (other.Contains(new V3(x, y, MinZ - 1)))
                        return false;
            return true;
        }

        public void FallOne()
        {
            A = A with { Z = A.Z - 1 };
            B = B with { Z = B.Z - 1 };
            MaxZ--;
            MinZ--;
        }
    }

    public Brick ParseBrick(string s)
    {
        var e = s.Split('~');
        var a = e[0].Split(',').Select(v => v.Int()).ToList();
        var b = e[1].Split(',').Select(v => v.Int()).ToList();
        return new Brick(new V3(a[0], a[1], a[2]), new V3(b[0], b[1], b[2]));
    }

    public override Answer One(string input)
    {
        var bricks = input.Lines().Where(IsNotBlank).Select(ParseBrick).ToList();

        var floor = new Brick(
            new V3(bricks.Min(b => b.MinX), bricks.Min(b => b.MinY), 0),
            new V3(bricks.Max(b => b.MaxX), bricks.Max(b => b.MaxY), 0)
        );

        bricks.Add(floor);

        foreach (var brick in bricks.OrderBy(b => b.MinZ).Skip(1))
        {
            while (bricks.Where(b => b != brick).Where(b => b.MaxZ < brick.MinZ).All(b => brick.CanFallRealativeTo(b)))
            {
                brick.FallOne();
            }
        }

        int t = 0;

        foreach (var brick in bricks.OrderBy(b => b.MinZ).Skip(1))
        {
            if (!UnstableWithout(brick, bricks))
                t++;
        }

        return t;
    }

    bool UnstableWithout(Brick destroyedBrick, List<Brick> bricks)
    {
        foreach (var brick in bricks.Where(b => b.MinZ - 1 == destroyedBrick.MaxZ))
        {
            if (bricks.Where(b => b != brick && b != destroyedBrick).All(b => brick.CanFallRealativeTo(b)))
            {
                return true;
            }
        }
        return false;
    }

    int CountFallingBricks(List<Brick> bricks)
    {
        int count = 0;
        foreach (var brick in bricks.OrderBy(b => b.MinZ).Skip(1))
        {
            bool didFall = false;
            while (bricks.Where(b => b != brick).Where(b => b.MaxZ < brick.MinZ).All(b => brick.CanFallRealativeTo(b)))
            {
                brick.FallOne();
                didFall = true;
            }
            if (didFall) count++;
        }
        return count;
    }

    public override Answer Two(string input)
    {
        var bricks = input.Lines().Where(IsNotBlank).Select(ParseBrick).ToList();

        var floor = new Brick(
            new V3(bricks.Min(b => b.MinX), bricks.Min(b => b.MinY), 0),
            new V3(bricks.Max(b => b.MaxX), bricks.Max(b => b.MaxY), 0)
        );

        bricks.Add(floor);

        foreach (var brick in bricks.OrderBy(b => b.MinZ).Skip(1))
        {
            while (bricks.Where(b => b != brick).All(b => brick.CanFallRealativeTo(b)))
            {
                brick.FallOne();
            }
        }

        int t = 0;

        foreach (var brick in bricks.OrderBy(b => b.MinZ).Skip(1))
        {
            var bricksWithout = bricks.Where(b => b != brick).Select(b => new Brick(b.A, b.B)).ToList();
            t += CountFallingBricks(bricksWithout);
            Console.Write(".");
        }

        return t;
    }
}
