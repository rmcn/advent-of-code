global using MoreLinq;
global using static AdventOfCode.Globals;

namespace AdventOfCode
{
    public static class Globals
    {
        public static bool IsBlank(string s) => string.IsNullOrWhiteSpace(s);
        public static bool IsNotBlank(string s) => !IsBlank(s);
        public static int Int(string s) => int.Parse(s);
    }
}