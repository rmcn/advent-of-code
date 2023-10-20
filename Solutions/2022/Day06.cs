namespace AdventOfCode.Year2022;

class Day06 : IDay
{
    public int Year => 2022;
    public int Day => 6;

    public (bool, string) One(string input) 
    {
        int i;
        for (i = 0; i < input.Length; i++)
        {
            if (input.Skip(i).Take(4).Distinct().Count() == 4)
            {
                break;
            }
        }

        return (true, (i + 4).ToString());
    }

    public (bool, string) Two(string input)
    {
        int i;
        for (i = 0; i < input.Length; i++)
        {
            if (input.Skip(i).Take(14).Distinct().Count() == 14)
            {
                break;
            }
        }

        return (true, (i + 14).ToString());
    }
}