using System.Text.RegularExpressions;

namespace AdventOfCode.Year2024;

public class Day03 : Solution
{
    public override string Example => @"";

    public override Answer One(string input)
    {
        return Regex.Matches(input, @"mul\(\d+,\d+\)").Sum(m => m.Value.Ints().Product());
    }

    public override Answer Two(string input)
    {
        var t = 0;

        bool include = true;
        foreach (var m in Regex.Matches(input, @"(mul\(\d+,\d+\)|do\(\)|don't\(\))").ToList())
        {
            if (m.Value == "do()")
                include = true;
            else if (m.Value == "don't()")
                include = false;
            else if (include)
                t += m.Value.Ints().Product();
        }
    
        return t;
    }
}
