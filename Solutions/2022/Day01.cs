namespace AdventOfCode.Year2022;

class Day01 : IDay
{
    public int Year => 2022;
    public int Day => 1;

    public string One(string input)
    {
        var cals = ElfCalories(input);
        return cals.OrderByDescending(c => c).First().ToString();
    }


    public string Two(string input)
    {
        var cals = ElfCalories(input);
        return cals.OrderByDescending(c => c).Take(3).Sum().ToString();
    }

    private static List<int> ElfCalories(string input)
    {
        var lines = input.Split("\n");

        var sum = 0;

        var cals = new List<int>();

        foreach (var line in lines)
        {
            if (line == "")
            {
                cals.Add(sum);
                sum = 0;
            }
            else
            {
                sum += int.Parse(line);
            }
        }

        return cals;
    }
}