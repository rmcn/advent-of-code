using System.Drawing;

namespace AdventOfCode;

public class Grid
{
    private readonly char _default;

    public Dictionary<Point, char> Values { get; } = new();

    public int Width { get; }
    public int Height { get; }

    private Grid(string input, char defaultValue)
    {
        var rows = input.Lines().Where(IsNotBlank).ToList();

        Width = rows.First().Length;
        Height = rows.Count;

        _default = defaultValue;

        foreach (var (s, y) in rows.Select((s, y) => (s, y)))
            foreach (var (c, x) in s.ToCharArray().Select((c, x) => (c, x)))
                Values[new Point(x, y)] = c;
    }

    public char this[Point p] => Values.GetValueOrDefault(p, _default);

    public static Grid Parse(string input, char defaultValue) => new Grid(input, defaultValue);
}
