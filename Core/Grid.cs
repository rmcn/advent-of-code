﻿using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace AdventOfCode;

public class Grid : Grid<char>
{
    private Grid(string input, char defaultValue, bool infinite) : base(input, c => c, defaultValue, infinite)
    {
    }

    /// <summary>
    /// Create a wrap-around infinite grid of characters.
    /// </summary>
    public static Grid ParseInfinite(string input)
        => new Grid(input, '?', infinite: true);

    /// <summary>
    /// Create a fixed sized grid of characters.
    /// Any points outside the grid will return the default value.
    /// </summary>
    public static Grid ParseFixed(string input, char outOfBoundsValue)
        => new Grid(input, outOfBoundsValue, infinite: false);

    public static Grid Fixed(int width, int height, char fillValue, char outOfBoundsValue)
    {
        var input = string.Join('\n', MoreEnumerable.Sequence(0, height-1).Select(_ => new string(fillValue, width)));
        return new Grid(input, outOfBoundsValue, infinite: false);
    }

    public static Grid Parse(string input, char outOfBoundsValue, bool infinite = false)
        => new Grid(input, outOfBoundsValue, infinite);
    public static Grid<T> Parse<T>(string input, Func<char, T> parse, T outOfBoundsValue, bool infinite = false)
        => new(input, parse, outOfBoundsValue, infinite);
}

public class Grid<T>
{
    private readonly T _outOfBoundsValue;
    private readonly bool _infinite;

    public Dictionary<Point, T> Cells { get; } = new();

    public int Width { get; }
    public int Height { get; }

    public Grid(string input, Func<char, T> parse, T outOfBoundsValue, bool infinite)
    {
        var rows = input.Lines().Where(IsNotBlank).Select(l => l.Trim()).ToList();

        Width = rows.First().Length;
        Height = rows.Count;

        _outOfBoundsValue = outOfBoundsValue;
        _infinite = infinite;

        foreach (var (s, y) in rows.Select((s, y) => (s, y)))
            foreach (var (c, x) in s.ToCharArray().Select((c, x) => (c, x)))
                Cells[new Point(x, y)] = parse(c);
    }

    public T this[Point p]
    {
        get => _infinite ? Cells[Wrap(p)] : Cells.GetValueOrDefault(p, _outOfBoundsValue);
        set
        {
            if (_infinite)
                p = Wrap(p);
            Cells[p] = value;
        }
    }

    private Point Wrap(Point p) => Grid.Wrap(p, Width, Height);

    public static Point Wrap(Point p, int width, int height)
    {
        var modX = p.X % width;
        var x = p.X < 0
            ? width + (modX == 0 ? -width : modX)
            : modX;

        var modY = p.Y % height;
        var y = p.Y < 0
            ? height + (modY == 0 ? -height : modY)
            : modY;

        return new Point(x, y);
    }

    public string Dump()
    {
        var sb = new StringBuilder();
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                sb.Append(Cells[new Point(x, y)]);
            }
            sb.Append('\n');
        }
        return sb.ToString();
    }

    public bool InBounds(Point p) => p.X >=0 && p.X < Width && p.Y >= 0 && p.Y < Height;
}
