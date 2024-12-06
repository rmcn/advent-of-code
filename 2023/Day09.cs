
namespace AdventOfCode.Year2023;

public class Day09 : Solution
{
    public override string Example => @"";

    public override Answer One(string input)
    {
        int t = 0;

        foreach (var line in input.Lines().Where(IsNotBlank))
        {
            var list = line.Ints();
            LogEx("Procesing " + string.Join(", ", list));
            var next = list.Last() + ExtrapolateNext(list);
            t += next;
        }
        return t;
    }

    private int ExtrapolateNext(List<int> list)
    {
        var diffs = list.Zip(list.Skip(1)).Select(t => t.Second - t.First).ToList();

        LogEx("Diffs " + string.Join(", ", list));

        if (diffs.All(d => d == 0))
        {
            LogEx("Next Zero");
            return 0;
        }
        else
        {
            var next = diffs.Last() + ExtrapolateNext(diffs);
            LogEx($"Next {next}");
            return next;
        }
    }

    public override Answer Two(string input)
    {
        int t = 0;

        foreach (var line in input.Lines().Where(IsNotBlank))
        {
            var list = line.Ints();
            LogEx("Procesing " + string.Join(", ", list));
            var prev = list.First() - ExtrapolatePrevious(list);
            t += prev;
        }
        return t;
    }

    private int ExtrapolatePrevious(List<int> list)
    {
        var diffs = list.Zip(list.Skip(1)).Select(t => t.Second - t.First).ToList();

        LogEx("Diffs " + string.Join(", ", list));

        if (diffs.All(d => d == 0))
        {
            LogEx("Prev Zero");
            return 0;
        }
        else
        {
            var prev = diffs.First() - ExtrapolatePrevious(diffs);
            LogEx($"Prev {prev}");
            return prev;
        }
    }
}
