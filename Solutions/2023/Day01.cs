namespace AdventOfCode.Year2023;

using DigitMap = Dictionary<string, char>;

public class Day01 : Solution
{
    public override object One(string input) => input.Lines().Where(IsNotBlank).Select(CalibrationOne).Sum();

    int CalibrationOne(string s) => $"{FirstDigit(s)}{LastDigit(s)}".Int();

    public override object Two(string input) => input.Lines().Where(IsNotBlank).Select(CalibrationTwo).Sum();

    int CalibrationTwo(string s) => $"{FirstDigitOrWord(s)}{LastDigitOrWord(s)}".Int();

    static DigitMap Digits = "123456789".ToDictionary(c => $"{c}", c => c);

    static DigitMap Words = new()
    {
        {"one", '1'},
        {"two", '2'},
        {"three", '3'},
        {"four", '4'},
        {"five", '5'},
        {"six", '6'},
        {"seven", '7'},
        {"eight", '8'},
        {"nine", '9'}
    };

    static DigitMap DigitsAndWords = Merge(Digits, Words);

    char FirstDigit(string s) => FirstMatch(s, Digits);
    char LastDigit(string s) => LastMatch(s, Digits);
    char FirstDigitOrWord(string s) => FirstMatch(s, DigitsAndWords);
    char LastDigitOrWord(string s) => LastMatch(s, DigitsAndWords);

    char FirstMatch(string s, DigitMap map)
    {
        for (int i = 0; i < s.Length; i++)
        {
            var substring = s[i..];

            foreach (var key in map.Keys)
                if (substring.StartsWith(key))
                    return map[key];
        }
        throw new Exception("No match");
    }

    char LastMatch(string s, DigitMap map) => FirstMatch(Reverse(s), map.ToDictionary(p => Reverse(p.Key), p => p.Value));

    private static string Reverse(string line)
    {
        return new string(line.Reverse().ToArray());
    }
}
