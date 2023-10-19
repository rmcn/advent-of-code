using System.Text.RegularExpressions;

namespace AdventOfCode.Year2022;

class Day04 : IDay
{
    public int Year => 2022;
    public int Day => 4;

    public string One(string input) 
    {
        var t = 0;

        foreach (var line in input.Lines().Where(l => l != ""))
        {
            var nums = new Regex("[0-9]+").Matches(line).Select(m => m.Value).Select(n => int.Parse(n)).ToList();
            if ((nums[0] >= nums[2] && nums[1] <= nums[3]) || (nums[2] >= nums[0] && nums[3] <= nums[1]))
            {
                t++;
            }
        }

        return t.ToString();
    }

    public string Two(string input)
    {
        var t = 0;

        foreach (var line in input.Lines().Where(l => l != ""))
        {
            var nums = new Regex("[0-9]+").Matches(line).Select(m => m.Value).Select(n => int.Parse(n)).ToList();
            if ((nums[0] < nums[2] && nums[1] < nums[2]) || (nums[0] > nums[3] && nums[1] > nums[3]))
            {
            }
            else
                t++;
        }

        return t.ToString();
    }
}