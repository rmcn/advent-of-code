namespace AdventOfCode;

interface IDay
{
    int Year => this.GetType().FullName!.Ints()[0];
    int Day => this.GetType().FullName!.Ints()[1];

    object One(string input);
    object Two(string input);
}