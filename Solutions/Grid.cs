using System.Drawing;

namespace AdventOfCode;

public class Grid : Grid<char>
{
    private Grid(string input, char defaultValue) : base(input, c => c, defaultValue)
    {
    }

    public static Grid Parse(string input, char defaultValue) => new Grid(input, defaultValue);
    public static Grid<T> Parse<T>(string input, Func<char, T> parse, T defaultValue) => new(input, parse, defaultValue);
}

public class Grid<T>
{
    private readonly T _default;

    public Dictionary<Point, T> Cells { get; } = new();

    public int Width { get; }
    public int Height { get; }

    public Grid(string input, Func<char, T> parse, T defaultValue)
    {
        var rows = input.Lines().Where(IsNotBlank).ToList();

        Width = rows.First().Length;
        Height = rows.Count;

        _default = defaultValue;

        foreach (var (s, y) in rows.Select((s, y) => (s, y)))
            foreach (var (c, x) in s.ToCharArray().Select((c, x) => (c, x)))
                Cells[new Point(x, y)] = parse(c);
    }

    public T this[Point p] => Cells.GetValueOrDefault(p, _default);
}
