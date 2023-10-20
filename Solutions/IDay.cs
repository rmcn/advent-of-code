namespace AdventOfCode;

interface IDay
{
    int Year => this.GetType().FullName!.Ints()[0];
    int Day => this.GetType().FullName!.Ints()[1];
    (bool, string) One(string input);
    (bool, string) Two(string input);
}