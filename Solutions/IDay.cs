namespace AdventOfCode;

interface IDay
{
    int Year { get; }
    int Day { get; }
    (bool, string) One(string input);
    (bool, string) Two(string input);
}