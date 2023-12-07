
namespace AdventOfCode.Year2023;

public class Day07 : Solution
{
    public override string Example => @"32T3K 765
T55J5 684
KK677 28
KTJJT 220
QQQJA 483";

    public override Answer One(string input)
    {
        var hands = input.Lines().Where(IsNotBlank).Select(h => ParseHand(h, withJokers: false)).ToList();

        var ranked = hands.OrderBy(h => h.Strength).ToList();

        return ranked.Select((h, i) => (i + 1) * h.Bid).Sum();
    }

    record Hand(string Original, string Strength, long Bid, string Sorted);

    Hand ParseHand(string original, bool withJokers)
    {
        var parts = original.Trim().Split(' ');
        var cards = parts[0].Trim();
        var strength = withJokers
            ? JokerHandType(cards) + "|" + cards.Replace('T', 'a').Replace('J', '1').Replace('Q', 'c').Replace('K', 'd').Replace('A', 'e')
            : HandType(cards) + "|" + cards.Replace('T', 'a').Replace('J', 'b').Replace('Q', 'c').Replace('K', 'd').Replace('A', 'e');
        return new Hand(parts[0], strength, parts[1].Long(), new string(parts[0].OrderBy(c => c).ToArray()));
    }

    string HandType(string cards)
    {
        var g = cards.GroupBy(c => c).Select(g => g.Count()).OrderByDescending(c => c).ToList();

        if (g[0] == 5)
            return "7";
        if (g[0] == 4)
            return "6";
        if (g[0] == 3 && g[1] == 2)
            return "5";
        if (g[0] == 3)
            return "4";
        if (g[0] == 2 && g[1] == 2)
            return "3";
        if (g[0] == 2)
            return "2";

        return "1";
    }

    string JokerHandType(string cards)
    {
        var jokerCount = cards.Count(c => c == 'J');
        var otherCards = cards.Replace("J", "");

        if (jokerCount == 5)
            return "7";

        return otherCards.Select(c => HandType(otherCards + new string(c, jokerCount))).OrderBy(r => r).Last();
    }

    public override Answer Two(string input)
    {
        var hands = input.Lines().Where(IsNotBlank).Select(h => ParseHand(h, withJokers: true)).ToList();

        var ranked = hands.OrderBy(h => h.Strength).ToList();

        return ranked.Select((h, i) => (i + 1) * h.Bid).Sum();
    }
}
