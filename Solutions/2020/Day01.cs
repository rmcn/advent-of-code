namespace AdventOfCode.Year2020;

public class Day01 : Solution
{
    public override string FilePath => MetaHelper.FilePath();
    public override string Example => @"";

    public override Answer One(string input)
    {
        var numbers = new HashSet<int>(input.Ints());

        foreach (var num1 in numbers)
        {
            var num2 = 2020 - num1;
            if (numbers.Contains(num2))
                return num1 * num2;
        }

        return 0;
    }

    public override Answer Two(string input)
    {
        var numbers = new HashSet<int>(input.Ints());

        foreach (var num1 in numbers)
        {
            foreach (var num2 in numbers)
            {
                var num3 = 2020 - num1 - num2;
                if (numbers.Contains(num3))
                    return num1 * num2 * num3;
            }
        }

        return 0;
    }
}
