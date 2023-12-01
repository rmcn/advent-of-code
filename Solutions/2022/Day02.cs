namespace AdventOfCode.Year2022;

class Day02 : IDay
{
    public (bool, object) One(string input) => (true, input.Lines().Where(IsNotBlank).Select(ScoreOne).Sum().ToString());

    private static int ScoreOne(string line)
    {
        return (line[0], line[2]) switch
        {
            ('A', 'X') => 1 + 3,
            ('A', 'Y') => 2 + 6,
            ('A', 'Z') => 3 + 0,

            ('B', 'X') => 1 + 0,
            ('B', 'Y') => 2 + 3,
            ('B', 'Z') => 3 + 6,

            ('C', 'X') => 1 + 6,
            ('C', 'Y') => 2 + 0,
            ('C', 'Z') => 3 + 3,
            _ => throw new Exception()
        };
    }

    public (bool, object) Two(string input) => (true, input.Lines().Where(IsNotBlank).Select(ScoreTwo).Sum().ToString());

    private static int ScoreTwo(string line)
    {
        return (line[0], line[2]) switch
        {
            // R
            ('A', 'X') => 3 + 0, // S
            ('A', 'Y') => 1 + 3, // R
            ('A', 'Z') => 2 + 6, // P

            // P
            ('B', 'X') => 1 + 0, // R
            ('B', 'Y') => 2 + 3, // P
            ('B', 'Z') => 3 + 6, // S

            // S
            ('C', 'X') => 2 + 0, // P
            ('C', 'Y') => 3 + 3, // S
            ('C', 'Z') => 1 + 6, // R
            _ => throw new Exception()
        };
    }
}