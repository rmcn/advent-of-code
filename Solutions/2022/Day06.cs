namespace AdventOfCode.Year2022;

class Day06 : IDay
{
    public object One(string input) 
    {
        int i;
        for (i = 0; i < input.Length; i++)
        {
            if (input.Skip(i).Take(4).Distinct().Count() == 4)
            {
                break;
            }
        }

        return i + 4;
    }

    public object Two(string input)
    {
        int i;
        for (i = 0; i < input.Length; i++)
        {
            if (input.Skip(i).Take(14).Distinct().Count() == 14)
            {
                break;
            }
        }

        return i + 14;
    }
}