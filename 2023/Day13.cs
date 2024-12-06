


using System.Text;

namespace AdventOfCode.Year2023;

public class Day13 : Solution
{
    public override Answer One(string input)
    {
        int t = 0;

        foreach (var g in input.Lines().Segment(IsBlank))
        {
            var grid = g.Where(IsNotBlank).Select(s => new StringBuilder(s)).ToArray();
            t += Score(grid, 0);
        }

        return t;
    }

    private int Score(StringBuilder[] grid, int oldScore)
    {
        if (grid.Length == 0)
            return 0;

        for (int c = 1; c < grid[0].Length; c++)
        {
            if (IsVertReflection(grid, c) && c != oldScore)
                return c;
        }

        for (int r = 1; r < grid.Length; r++)
        {
            if (IsHorzReflection(grid, r) && r * 100 != oldScore)
                return r * 100;
        }

        return 0;
    }

    private bool IsVertReflection(StringBuilder[] grid, int c)
    {
        for (int r = 0; r < grid.Length; r++)
        {
            for (int dc = 0; c + dc - 1 < grid[r].Length && c - dc >= 0; dc++)
            {
                if (grid[r][c + dc - 1] != grid[r][c - dc])
                {
                    return false;
                }
            }
        }
        return true;
    }

    private bool IsHorzReflection(StringBuilder[] grid, int r)
    {
        for (int c = 0; c < grid[r].Length; c++)
        {
            for (int dr = 0; r + dr - 1 < grid.Length && r - dr >= 0; dr++)
            {
                if (grid[r + dr - 1][c] != grid[r - dr][c])
                {
                    return false;
                }
            }
        }
        return true;
    }

    public override Answer Two(string input)
    {
        int t = 0;

        foreach (var g in input.Lines().Segment(IsBlank))
        {
            var grid = g.Where(IsNotBlank).Select(s => new StringBuilder(s)).ToArray();
            t += ScoreTwo(grid);
        }

        return t;
    }

    private int ScoreTwo(StringBuilder[] grid)
    {
        if (grid.Length == 0)
            return 0;

        var oldScore = Score(grid, 0);

        for (int r = 0; r < grid.Length; r++)
        {
            for (int c = 0; c < grid[r].Length; c++)
            {
                Toggle(grid, r, c);
                var s = Score(grid, oldScore);
                if (s != 0)
                    return s;
                Toggle(grid, r, c);
            }
        }
        throw new Exception("No reflect");
    }

    private void Toggle(StringBuilder[] grid, int r, int c)
    {
        grid[r][c] = grid[r][c] == '.' ? '#' : '.';
    }
}
