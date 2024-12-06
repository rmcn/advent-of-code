namespace AdventOfCode.Year2020;

public class Day05 : Solution
{
    public override string FilePath => MetaHelper.FilePath();
    public override string Example => @"FBFBBFFRLR";

    public override Answer One(string input)
    {
        return input.Lines().Where(IsNotBlank).Max(ParseSeat);
    }

    private static int ParseSeat(string input)
    {
        var v = input.Trim()
            .Replace("F", "0")
            .Replace("B", "1")
            .Replace("L", "0")
            .Replace("R", "1");

        return Convert.ToInt32(v, 2);
    }

    public override Answer Two(string input)
    {
        var seats = new HashSet<int>(input.Lines().Where(IsNotBlank).Select(ParseSeat));

        return Enumerable.Range(seats.Min(), seats.Max() - seats.Min() + 1).Except(seats).FirstOrDefault();
    }
}
