namespace AdventOfCode;

interface IDay
{
    int Year { get; }
    int Day { get; }
    string One(string input);
    string Two(string input);
}