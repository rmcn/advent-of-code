global using MoreLinq;
global using static AdventOfCode.Globals;
global using static System.Math;

namespace AdventOfCode
{
    public static class Globals
    {
        public static bool IsBlank(string s) => string.IsNullOrWhiteSpace(s);
        public static bool IsNotBlank(string s) => !IsBlank(s);
        public static int Int(string s) => int.Parse(s);

        public static Dictionary<TKey, TValue> Merge<TKey, TValue>(params IDictionary<TKey, TValue>[] dicts)
            where TKey: notnull
        {
            var result = new Dictionary<TKey, TValue>();

            foreach (var d in dicts)
                foreach (var kvp in d)
                    result[kvp.Key] = kvp.Value;

            return result;
        }
    }
}