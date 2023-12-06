using System.Numerics;
using System.Text.RegularExpressions;

namespace AdventOfCode;

public static class Extensions
{
    public static string[] Lines(this string s) => s.Split('\n');
    public static int Int(this string s) => s.Ints().Single();
    public static long Long(this string s) => s.Longs().Single();
    public static List<int> Ints(this string s) => new Regex("-?[0-9]+").Matches(s).Select(m => m.Value).Select(n => int.Parse(n)).ToList();
    public static List<long> Longs(this string s) => new Regex("-?[0-9]+").Matches(s).Select(m => m.Value).Select(n => long.Parse(n)).ToList();
    public static List<int> PosInts(this string s) => new Regex("[0-9]+").Matches(s).Select(m => m.Value).Select(n => int.Parse(n)).ToList();
    public static bool IsBlank(this string s) => string.IsNullOrWhiteSpace(s);
    public static bool IsNotBlank(this string s) => !s.IsBlank();
    public static SubmitAnswer Submit(this object v) => new(v);
    public static T Product<T>(this IEnumerable<T> values) where T : INumber<T>
        => values.Aggregate(T.One, (a, b) => a * b);
}
