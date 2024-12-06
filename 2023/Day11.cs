using System.Drawing;

namespace AdventOfCode.Year2023;

public class Day11 : Solution
{
    public override string Example => @"";

    public override Answer One(string input) => Solve(input, 1);

    public override Answer Two(string input) => Solve(input, 1000000 - 1);


    private static Answer Solve(string input, int scale)
    {
        var lines = input.Lines().Select(l => l.Trim()).Where(IsNotBlank).ToList();

        var blankCols = new List<int>();

        for (int x = 0; x < lines[0].Length; x++)
        {
            for (int y = 0; y < lines.Count; y++)
            {
                if (lines[y][x] == '#')
                    break;

                if (y == lines.Count - 1)
                    blankCols.Add(x);
            }
        }

        var galax = new List<Point>();

        int blankRowCount = 0;
        for (int y = 0; y < lines.Count; y++)
        {
            var line = lines[y];
            bool allBlankRow = true;
            for (int x = 0; x < line.Length; x++)
            {
                if (lines[y][x] == '#')
                    allBlankRow = false;

                if (lines[y][x] == '#')
                    galax.Add(new Point(x + blankCols.Count(c => c <= x) * scale, y + blankRowCount * scale));
            }
            if (allBlankRow)
                blankRowCount++;
        }


        ulong t = 0;

        for (int i = 0; i < galax.Count - 1; i++)
        {
            for (int j = i; j < galax.Count; j++)
            {
                t += (ulong)Abs(galax[i].X - galax[j].X) + (ulong)Abs(galax[i].Y - galax[j].Y);
            }
        }

        return t;
    }

}
