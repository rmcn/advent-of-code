
namespace AdventOfCode.Year2023;

public class Day03 : Solution
{
    public override object One(string input)
    {
        int t = 0;

        var lines = input.Lines().Where(IsNotBlank).Select(s => $".{s}.").ToList();

        lines.Insert(0, new string('.', lines[0].Length));
        lines.Add(new string('.', lines[0].Length));

        var grid = lines.ToArray();

        for (int y = 1; y < grid.Length - 1; y++)
        {
            for (int x = 1; x < grid[y].Length - 1; x++)
            {
                var startX = x;
                while(char.IsDigit(grid[y][x]))
                    x++;
                
                if (startX != x)
                {
                    var num = grid[y][startX..x];
                    if(IsNextToSymbol(grid, y, startX, x))
                    {
                        t += num.Int();
                    }
                }
            }
        }

        return t;
    }

    private bool IsNextToSymbol(string[] grid, int y, int startX, int endX)
    {
        if(IsSymbol(grid[y][startX-1]))
            return true;

        if(IsSymbol(grid[y][endX]))
            return true;

        for(int x = startX-1; x <= endX; x++)
        {
            if(IsSymbol(grid[y-1][x]))
                return true;

            if(IsSymbol(grid[y+1][x]))
                return true;
        }

        return false;
    }

    bool IsSymbol(char c) => c != '.' && !char.IsDigit(c);

    public override object Two(string input)
    {
        var lines = input.Lines().Where(IsNotBlank).Select(s => $".{s}.").ToList();

        lines.Insert(0, new string('.', lines[0].Length));
        lines.Add(new string('.', lines[0].Length));

        var grid = lines.ToArray();

        var gearParts = new List<(string g, int num)>();

        for (int y = 1; y < grid.Length - 1; y++)
        {
            for (int x = 1; x < grid[y].Length - 1; x++)
            {
                var startX = x;
                while(char.IsDigit(grid[y][x]))
                    x++;
                
                if (startX != x)
                {
                    var num = grid[y][startX..x];

                    var gears = FindGearsFor(grid, y, startX, x);

                    foreach(var gear in gears)
                    {
                        gearParts.Add((gear, num.Int()));
                    }
                }
            }
        }

        return gearParts
            .GroupBy(gp => gp.g)
            .Where(gpg => gpg.Count() == 2)
            .Select(gpg => gpg.ToList()[0].num * gpg.ToList()[1].num)
            .Sum();
    }


    private List<string> FindGearsFor(string[] grid, int y, int startX, int endX)
    {
        var gears = new List<string>();
        if(IsSymbol(grid[y][startX-1]))
            gears.Add($"{y},{startX-1}");

        if(IsSymbol(grid[y][endX]))
            gears.Add($"{y},{endX}");

        for(int x = startX-1; x <= endX; x++)
        {
            if(IsSymbol(grid[y-1][x]))
                gears.Add($"{y-1},{x}");

            if(IsSymbol(grid[y+1][x]))
                gears.Add($"{y+1},{x}");
        }

        return gears;
    }

}
