using System.Text.RegularExpressions;

namespace AdventOfCode;

public static class Extensions
{
    public static string[] Lines(this string s) => s.Split('\n');
    public static int Int(this string s) => s.Ints().Single();
    public static List<int> Ints(this string s) => new Regex("-?[0-9]+").Matches(s).Select(m => m.Value).Select(n => int.Parse(n)).ToList();
    public static List<int> PosInts(this string s) => new Regex("[0-9]+").Matches(s).Select(m => m.Value).Select(n => int.Parse(n)).ToList();
    public static bool IsBlank(this string s) => string.IsNullOrWhiteSpace(s);
    public static bool IsNotBlank(this string s) => !s.IsBlank();
    public static SubmitAnswer Submit(this object v) => new(v);
}
