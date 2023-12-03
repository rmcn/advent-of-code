using System.Text.RegularExpressions;

namespace AdventOfCode.Year2022;

public class Day04 : Solution
{
    public override object One(string input)
    {
        var t = 0;

        foreach (var line in input.Lines().Where(l => l != ""))
        {
            var nums = line.PosInts();
            if ((nums[0] >= nums[2] && nums[1] <= nums[3]) || (nums[2] >= nums[0] && nums[3] <= nums[1]))
            {
                t++;
            }
        }

        return t;
    }

    public override object Two(string input)
    {
        var t = 0;

        foreach (var line in input.Lines().Where(l => l != ""))
        {
            var nums = line.PosInts();
            if ((nums[0] < nums[2] && nums[1] < nums[2]) || (nums[0] > nums[3] && nums[1] > nums[3]))
            {
            }
            else
                t++;
        }

        return t;
    }
}