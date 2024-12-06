using System.Runtime.CompilerServices;
using System.Text;

namespace AdventOfCode.Year2023;

public class Day14 : Solution
{
    public override Answer One(string input)
    {
        var g = input.Lines().Where(IsNotBlank).Select(s => new StringBuilder(s.Trim())).ToArray();

        North(g);

        LogEx("\nAfter:\n" + string.Join('\n', g.Select(s => s.ToString() + ' ' + s.ToString().Count(r => r == 'O'))));

        return g.Select((s, i) => s.ToString().Count(c => c == 'O') * (g.Length - i)).Sum();
    }

    private static void North(StringBuilder[] g)
    {
        for (int c = 0; c < g[0].Length; c++)
        {
            var stopIndex = -1;
            for (int r = 0; r < g.Length; r++)
            {
                if (g[r][c] == '#')
                {
                    stopIndex = r;
                }
                else if (g[r][c] == 'O')
                {
                    if (r > stopIndex + 1)
                    {
                        stopIndex++;
                        g[stopIndex][c] = 'O';
                        g[r][c] = '.';
                    }
                    else
                        stopIndex = r;
                }
            }
        }
    }

    private static void South(StringBuilder[] g)
    {
        for (int c = 0; c < g[0].Length; c++)
        {
            var stopIndex = g.Length;
            for (int r = g.Length - 1; r >= 0; r--)
            {
                if (g[r][c] == '#')
                {
                    stopIndex = r;
                }
                else if (g[r][c] == 'O')
                {
                    if (r < stopIndex - 1)
                    {
                        stopIndex--;
                        g[stopIndex][c] = 'O';
                        g[r][c] = '.';
                    }
                    else
                        stopIndex = r;
                }
            }
        }
    }

    private static void West(StringBuilder[] g)
    {
        for (int r = 0; r < g.Length; r++)
        {
            var stopIndex = -1;
            for (int c = 0; c < g[0].Length; c++)
            {
                if (g[r][c] == '#')
                {
                    stopIndex = c;
                }
                else if (g[r][c] == 'O')
                {
                    if (c > stopIndex + 1)
                    {
                        stopIndex++;
                        g[r][stopIndex] = 'O';
                        g[r][c] = '.';
                    }
                    else
                        stopIndex = c;
                }
            }
        }
    }

    private static void East(StringBuilder[] g)
    {
        for (int r = 0; r < g.Length; r++)
        {
            var stopIndex = g[0].Length;
            for (int c = g[0].Length - 1; c >= 0; c--)
            {
                if (g[r][c] == '#')
                {
                    stopIndex = c;
                }
                else if (g[r][c] == 'O')
                {
                    if (c < stopIndex - 1)
                    {
                        stopIndex--;
                        g[r][stopIndex] = 'O';
                        g[r][c] = '.';
                    }
                    else
                        stopIndex = c;
                }
            }
        }
    }

    public override Answer Two(string input)
    {
        var g = input.Lines().Where(IsNotBlank).Select(s => new StringBuilder(s.Trim())).ToArray();

        var cur = Dump(g);

        Dictionary<string, string> fromTo = new();

        int steps = 1000;
        for (int i = 0; i <= 1000000000; i += steps)
        {
            if (fromTo.ContainsKey(cur))
            {
                cur = fromTo[cur];
            }
            else
            {
                var gc = cur.Split('\n').Select(s => new StringBuilder(s)).ToArray();

                for (int s = 0; s < steps; s++)
                {
                    North(gc);
                    West(gc);
                    South(gc);
                    East(gc);
                }

                var after = Dump(gc);
                fromTo.Add(cur, after);
                var current = after;
            }
        }

        return cur.Split('\n').Select(s => new StringBuilder(s)).ToArray().Select((s, i) => s.ToString().Count(c => c == 'O') * (g.Length - i)).Sum();
    }

    string Dump(StringBuilder[] g) => string.Join('\n', g.Select(s => s.ToString()));
}
