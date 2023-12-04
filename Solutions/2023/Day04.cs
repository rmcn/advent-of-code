using System.Data.Common;
using MoreLinq.Extensions;

namespace AdventOfCode.Year2023;

public class Day04 : Solution
{
    public override string Example => @"";

    public override object One(string input)
    {
        var cards = input.Lines().Where(IsNotBlank).Select(c => new Card(c)).ToList();
        return cards.Sum(c => (int)Pow(2, c.MatchCount - 1));
    }

    public override object Two(string input)
    {
        var cards = input.Lines().Where(IsNotBlank).Select(c => new Card(c)).ToList();

        var t = cards.Count;

        var candidates = new Stack<Card>(cards);

        int c = 0;

        while (candidates.Count > 0)
        {
            var card = candidates.Pop();

            var isWin = card.MatchCount > 0;

            if (isWin)
            {
                t += card.MatchCount;

                for (int i = 0; i < card.MatchCount; i++)
                {
                    candidates.Push(cards[card.Index + i]);
                }
            }
        }

        return t;
    }

    class Card
    {
        public Card(string card)
        {
            var game = card.Split(':');

            Index = game[0].Int();

            var nums = game[1].Split('|');
            var win = new HashSet<int>(nums[0].Ints());
            var got = new HashSet<int>(nums[1].Ints());
            MatchCount = win.Intersect(got).Count();
        }

        public int Index { get; set; }
        public int MatchCount { get; set; }
    }
}
