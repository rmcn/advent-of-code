using System.Drawing;
using System.Numerics;
using System.Text.RegularExpressions;

namespace AdventOfCode;

public static partial class CompiledRegex
{
    [GeneratedRegex("-?[0-9]+")]
    public static partial Regex Integer();

    [GeneratedRegex("[0-9]+")]
    public static partial Regex PosInteger();
}

public static class Extensions
{
    public static string[] Lines(this string s) => s.Split('\n');
    public static int Int(this string s) => s.Ints().Single();
    public static long Long(this string s) => s.Longs().Single();
    public static List<int> Ints(this string s) => CompiledRegex.Integer().Matches(s).Select(m => int.Parse(m.Value)).ToList();
    public static List<long> Longs(this string s) => CompiledRegex.Integer().Matches(s).Select(m => long.Parse(m.Value)).ToList();
    public static List<int> PosInts(this string s) => CompiledRegex.PosInteger().Matches(s).Select(m => int.Parse(m.Value)).ToList();
    public static bool IsBlank(this string s) => string.IsNullOrWhiteSpace(s);
    public static bool IsNotBlank(this string s) => !s.IsBlank();
    public static SubmitAnswer Submit(this object v) => new(v);
    public static T Product<T>(this IEnumerable<T> values) where T : INumber<T>
        => values.Aggregate(T.One, (a, b) => a * b);
    public static Point Add(this Point a, Point b) => new(a.X + b.X, a.Y + b.Y);
    public static Point Add(this Point a, int x, int y) => new(a.X + x, a.Y + y);
}
