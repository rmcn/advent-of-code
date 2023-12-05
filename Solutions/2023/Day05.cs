
using System.Data;

namespace AdventOfCode.Year2023;

public class Day05 : Solution
{
    public override string Example => @"";

    public override Answer One(string input)
    {
        var seeds = input.Lines().First().Longs();

        var maps = input.Lines().Skip(1).Where(IsNotBlank)
            .Segment(l => l.Contains(':'))
            .Select(ParseMap)
            .ToList();


        var ranges = seeds.Select(s => new ValueRange(s, 1)).ToList();

        return Chase(ranges, maps).Min(r => r.Start);
    }

    record Entry(long DestStart, long SourceStart, long Length);

    private List<Entry> ParseMap(IEnumerable<string> entries)
    {
        return entries.Where(e => !e.Contains(':')).Select(e => e.Longs()).Select(v => new Entry(v[0], v[1], v[2])).ToList();
    }

    public override Answer Two(string input)
    {
        var maps = input.Lines().Skip(1).Where(IsNotBlank)
            .Segment(l => l.Contains(':'))
            .Select(ParseMap)
            .ToList();

        var seedRanges = input.Lines().First().Longs().Batch(2)
            .Select(p => p.ToArray())
            .Select(p => new ValueRange(p[0], p[1]))
            .ToList();

        return Chase(seedRanges, maps).Min(r => r.Start);
    }


    private List<ValueRange> Chase(List<ValueRange> ranges, List<List<Entry>> maps)
    {
        foreach (var map in maps)
        {
            Log($"Amount {ranges.Count}");
            ranges = ApplyMap(ranges, map);
            LogEx(string.Join(", ", ranges));
        }
        return ranges;
    }

    List<ValueRange> ApplyMap(List<ValueRange> ranges, List<Entry> map)
    {
        Stack<ValueRange> inputRanges = new(ranges);
        List<ValueRange> outputRanges = new();

        while (inputRanges.Count > 0)
        {
            var range = inputRanges.Pop();
            bool mapped = false;
            foreach (var entry in map)
            {
                // Contained
                if (range.Start >= entry.SourceStart && range.End() < entry.SourceStart + entry.Length)
                {
                    outputRanges.Add(new ValueRange(range.Start - entry.SourceStart + entry.DestStart, range.Count));
                    mapped = true;
                    break;
                }
                // Start before, end within
                else if (range.Start < entry.SourceStart && range.End() >= entry.SourceStart && range.End() < entry.SourceStart + entry.Length)
                {
                    long countWithin = range.Start + range.Count - entry.SourceStart;
                    outputRanges.Add(new ValueRange(entry.SourceStart - entry.SourceStart + entry.DestStart, countWithin));
                    inputRanges.Push(new ValueRange(range.Start, range.Count - countWithin));
                    mapped = true;
                    break;
                }
                // Start within, end after
                else if (range.Start >= entry.SourceStart && range.Start < entry.SourceStart + entry.Length && range.End() >= entry.SourceStart + entry.Length)
                {
                    long countWithin = entry.SourceStart + entry.Length - range.Start;
                    outputRanges.Add(new ValueRange(range.Start - entry.SourceStart + entry.DestStart, countWithin));
                    inputRanges.Push(new ValueRange(entry.SourceStart + entry.Length, range.Count - countWithin));
                    mapped = true;
                    break;
                }
            }
            // Not mapped
            if (!mapped)
                outputRanges.Add(range);
        }
        return outputRanges;
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
        public long End() => Start + Count - 1;
        public bool Contains(long v) => v >= Start && v <= End();
        public override string ToString() => $"{Start}-{Count}";
    }
}
