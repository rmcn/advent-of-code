
using System.Data;

namespace AdventOfCode.Year2023;

public class Day05 : Solution
{
    public override Answer One(string input)
    {
        var maps = input.Lines().Skip(1).Where(IsNotBlank)
            .Segment(l => l.Contains(':'))
            .Select(ParseMap)
            .ToList();

        var seedRanges = input.Lines().First().Longs()
            .Select(s => new ValueRange(s, 1)).ToList();

        return ApplyMaps(seedRanges, maps).Min(r => r.Start);
    }

    public override Answer Two(string input)
    {
        var maps = input.Lines().Skip(1).Where(IsNotBlank)
            .Segment(l => l.Contains(':'))
            .Select(ParseMap)
            .ToList();

        var seedRanges = input.Lines().First().Longs()
            .Batch(2)
            .Select(p => p.ToArray())
            .Select(p => new ValueRange(p[0], p[1]))
            .ToList();

        return ApplyMaps(seedRanges, maps).Min(r => r.Start);
    }


    record MapEntry(long DestStart, long SourceStart, long Length);

    private List<MapEntry> ParseMap(IEnumerable<string> entries) =>
        entries
            .Where(e => !e.Contains(':'))
            .Select(e => e.Longs())
            .Select(v => new MapEntry(v[0], v[1], v[2]))
            .ToList();

    private List<ValueRange> ApplyMaps(List<ValueRange> ranges, List<List<MapEntry>> maps)
    {
        foreach (var map in maps)
        {
            Log($"Range count {ranges.Count}");
            ranges = ApplyMap(ranges, map);
            LogEx(string.Join(", ", ranges));
        }
        Log($"Final range count {ranges.Count}");
        return ranges;
    }

    List<ValueRange> ApplyMap(List<ValueRange> ranges, List<MapEntry> map)
    {
        Stack<ValueRange> inputRanges = new(ranges);
        List<ValueRange> mappedRanges = new();

        while (inputRanges.Count > 0)
        {
            var range = inputRanges.Pop();
            bool isMatched = false;
            foreach (var entry in map)
            {
                // Input range contained within map entry - output single mapped range
                if (range.Start >= entry.SourceStart && range.End < entry.SourceStart + entry.Length)
                {
                    mappedRanges.Add(new ValueRange(range.Start - entry.SourceStart + entry.DestStart, range.Count));
                    isMatched = true;
                    break;
                }
                // Input range starts before and ends within map entry - split into mapped range and other input
                else if (range.Start < entry.SourceStart && range.End >= entry.SourceStart && range.End < entry.SourceStart + entry.Length)
                {
                    long countWithin = range.Start + range.Count - entry.SourceStart;
                    mappedRanges.Add(new ValueRange(entry.SourceStart - entry.SourceStart + entry.DestStart, countWithin));
                    inputRanges.Push(new ValueRange(range.Start, range.Count - countWithin));
                    isMatched = true;
                    break;
                }
                // Input range starts within and ends after map entry - split into mapped range and other input
                else if (range.Start >= entry.SourceStart && range.Start < entry.SourceStart + entry.Length && range.End >= entry.SourceStart + entry.Length)
                {
                    long countWithin = entry.SourceStart + entry.Length - range.Start;
                    mappedRanges.Add(new ValueRange(range.Start - entry.SourceStart + entry.DestStart, countWithin));
                    inputRanges.Push(new ValueRange(entry.SourceStart + entry.Length, range.Count - countWithin));
                    isMatched = true;
                    break;
                }
            }
            // No map etries match - pass through unchanged
            if (!isMatched)
                mappedRanges.Add(range);
        }
        return mappedRanges;
    }


    public class ValueRange
    {
        public ValueRange(long start, long count)
        {
            Start = start;
            Count = count;
        }
        public long Start { get; set; }
        public long Count { get; set; }
        public long End => Start + Count - 1;
        public override string ToString() => $"{Start}-{Count}";
    }
}
