using System.Drawing;
using System.Runtime.InteropServices;

namespace AdventOfCode;

public class Grid : Grid<char>
{
    private Grid(string input, char defaultValue, bool infinite) : base(input, c => c, defaultValue, infinite)
    {
    }

    public static Grid Parse(string input, char defaultValue, bool infinite = false)
        => new Grid(input, defaultValue, infinite);
    public static Grid<T> Parse<T>(string input, Func<char, T> parse, T defaultValue, bool infinite = false)
        => new(input, parse, defaultValue, infinite);
}

public class Grid<T>
{
    private readonly T _default;
    private readonly bool _infinite;

    public Dictionary<Point, T> Cells { get; } = new();

    public int Width { get; }
    public int Height { get; }

    public Grid(string input, Func<char, T> parse, T defaultValue, bool infinite)
    {
        var rows = input.Lines().Where(IsNotBlank).ToList();

        Width = rows.First().Length;
        Height = rows.Count;

        _default = defaultValue;
        _infinite = infinite;

        foreach (var (s, y) in rows.Select((s, y) => (s, y)))
            foreach (var (c, x) in s.ToCharArray().Select((c, x) => (c, x)))
                Cells[new Point(x, y)] = parse(c);
    }

    public T this[Point p]
    {
        get => _infinite ? Cells[Wrap(p)] : Cells.GetValueOrDefault(p, _default);
        set
        {
            if (_infinite)
                p = Wrap(p);
            Cells[p] = value;
        }
    }

    private Point Wrap(Point p)
    {
        var modX = p.X % Width;
        var x = p.X < 0
            ? Width + (modX == 0 ? -Width : modX)
            : modX;

        var modY = p.Y % Height;
        var y = p.Y < 0
            ? Height + (modY == 0 ? -Height : modY)
            : modY;

        return new Point(x, y);
    }
}
