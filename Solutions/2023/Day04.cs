namespace AdventOfCode.Year2023;

public class Day04 : Solution
{
    public override object One(string input)
        => input
            .Lines()
            .Where(IsNotBlank)
            .Select(ParseCard)
            .Sum(c => (int)Pow(2, c.MatchCount - 1));

    public override object Two(string input)
    {
        var cards = input.Lines().Where(IsNotBlank).Select(ParseCard).ToList();

        var t = cards.Count;

        var candidates = new Stack<Card>(cards);

        while (candidates.Count > 0)
        {
            var card = candidates.Pop();

            t += card.MatchCount;

            for (int i = 0; i < card.MatchCount; i++)
            {
                candidates.Push(cards[card.Number + i]);
            }
        }

        return t;
    }

    record Card(int Number, int MatchCount);

    Card ParseCard(string card)
    {
        var numberParts = card.Split(':')[1].Split('|');
        var winners = new HashSet<int>(numberParts[0].Ints());
        var have = new HashSet<int>(numberParts[1].Ints());

        return new Card
        (
            Number: card.Ints()[0],
            MatchCount: have.Intersect(winners).Count()
        );
    }
}