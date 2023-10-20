namespace AdventOfCode.Year2022;

class Day05 : IDay
{
    public (bool, string) One(string input) 
    {
        var lines = input.Lines();

        var stacks = lines.TakeWhile(s => s.Contains("["))
            .Transpose()
            .Skip(1)
            .TakeEvery(4)
            .Select(s => new Stack<char>(s.Where(c => c != ' ').Reverse()))
            .ToList();

        var instructions = lines.SkipWhile(s => s.Contains("[")).Skip(2).ToList();

        foreach (var i in instructions.Where(IsNotBlank))
        {
            var nums = i.Ints();
            for (int n = 0; n < nums[0]; n++)
            {
                stacks[nums[2] - 1].Push(stacks[nums[1] - 1].Pop());    
            }
        }

        return (true, new string(stacks.Select(s => s.Pop()).ToArray()));
    }

    public (bool, string) Two(string input)
    {
        var lines = input.Lines();

        var stacks = lines.TakeWhile(s => s.Contains("["))
            .Transpose()
            .Skip(1)
            .TakeEvery(4)
            .Select(s => new Stack<char>(s.Where(c => c != ' ').Reverse()))
            .ToList();

        var instructions = lines.SkipWhile(s => s.Contains("[")).Skip(2).ToList();

        foreach (var i in instructions.Where(IsNotBlank))
        {
            var nums = i.Ints();
            var s = new Stack<char>();
            for (int n = 0; n < nums[0]; n++)
            {
                s.Push(stacks[nums[1] - 1].Pop());    
            }
            for (int n = 0; n < nums[0]; n++)
            {
                stacks[nums[2] - 1].Push(s.Pop());    
            }
        }

        return (true, new string(stacks.Select(s => s.Pop()).ToArray()));
    }
}