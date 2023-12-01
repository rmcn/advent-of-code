namespace AdventOfCode;

interface IDay
{
    int Year => this.GetType().FullName!.Ints()[0];
    int Day => this.GetType().FullName!.Ints()[1];
    (bool, object) One(string input);
    (bool, object) Two(string input);
}