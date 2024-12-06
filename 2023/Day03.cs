
namespace AdventOfCode.Year2023;

public class Day03 : Solution
{
    public override Answer One(string input)
    {
        int t = 0;

        string[] grid = ParseGrid(input);

        for (int y = 1; y < grid.Length - 1; y++)
        {
            for (int x = 1; x < grid[y].Length - 1; x++)
            {
                var startX = x;
                while (char.IsDigit(grid[y][x]))
                    x++;

                if (startX != x)
                {
                    var partNumber = grid[y][startX..x].Int();
                    if (FindAround(grid, y, startX, endX: x, IsSymbol).Any())
                    {
                        t += partNumber;
                    }
                }
            }
        }

        return t;
    }

    string[] ParseGrid(string input)
    {
        var rows = input.Lines().Where(IsNotBlank).Select(s => $".{s}.").ToList();

        var blankRow = new string('.', rows[0].Length);
        rows.Insert(0, blankRow);
        rows.Add(blankRow);

        return rows.ToArray();
    }

    bool IsSymbol(char c) => c != '.' && !char.IsDigit(c);
    bool IsGear(char c) => c == '*';

    private List<string> FindAround(string[] grid, int y, int startX, int endX, Func<char, bool> isMatch)
    {
        var gears = new List<string>();
        if (isMatch(grid[y][startX - 1]))
            gears.Add($"{y},{startX - 1}");

        if (isMatch(grid[y][endX]))
            gears.Add($"{y},{endX}");

        for (int x = startX - 1; x <= endX; x++)
        {
            if (isMatch(grid[y - 1][x]))
                gears.Add($"{y - 1},{x}");

            if (isMatch(grid[y + 1][x]))
                gears.Add($"{y + 1},{x}");
        }

        return gears;
    }

    public override Answer Two(string input)
    {
        string[] grid = ParseGrid(input);

        var gearCandidates = new List<(string location, int partNumber)>();

        for (int y = 1; y < grid.Length - 1; y++)
        {
            for (int x = 1; x < grid[y].Length - 1; x++)
            {
                var startX = x;
                while (char.IsDigit(grid[y][x]))
                    x++;

                if (startX != x)
                {
                    var partNumber = grid[y][startX..x].Int();

                    var gearLocations = FindAround(grid, y, startX, endX: x, IsGear);

                    foreach (var location in gearLocations)
                    {
                        gearCandidates.Add((location, partNumber));
                    }
                }
            }
        }

        return gearCandidates
            .GroupBy(gc => gc.location)
            .Where(g => g.Count() == 2)
            .Select(g => g.Select(g => g.partNumber).Product())
            .Sum();
    }
}
