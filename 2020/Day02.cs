
namespace AdventOfCode.Year2020;

public class Day02 : Solution
{
    public override string FilePath => MetaHelper.FilePath();
    public override string Example => @"";

    public override Answer One(string input)
    {
        return input.Lines().Where(IsNotBlank).Count(IsValidOne);
    }

    private bool IsValidOne(string line)
    {
        var limits = line.PosInts();

        var parts = line.Split(":");
        var pw = parts[1].Trim();
        var ch = parts[0].Last();

        var count = pw.Count(c => c == ch);

        return count >= limits[0] && count <= limits[1];
    }

    public override Answer Two(string input)
    {
        return input.Lines().Where(IsNotBlank).Count(IsValidTwo);
    }

    private bool IsValidTwo(string line)
    {
        var limits = line.PosInts();

        var parts = line.Split(":");
        var pw = parts[1].Trim();
        var ch = parts[0].Last();

        var at1 = pw[limits[0]-1] == ch;
        var at2 = pw[limits[1]-1] == ch;
        return at1 != at2; // xor
    }
}
