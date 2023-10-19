namespace AdventOfCode;

public static class Extensions
{
    public static string[] Lines(this string s) => s.Split('\n');
    public static int Int(this string s) => int.Parse(s);
    public static bool IsBlank(this string s) => string.IsNullOrWhiteSpace(s);
    public static bool IsNotBlank(this string s) => !s.IsBlank();
}
