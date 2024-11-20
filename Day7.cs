public class Day7
{
  public enum HandType
  {
    HighCard = 0,
    OnePair = 1,
    TwoPair = 2,
    ThreeOfKind = 3,
    FullHouse = 4,
    FourOfKind = 5,
    FiveOfKind = 6
  }

  public static char[] Cards = { 'J', '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'Q', 'K', 'A' };

  public static HandType getHandType(string hand)
  {
    int[] counts = new int[Cards.Length];
    Array.Fill(counts, 0);
    foreach (char card in hand)
    {
      int index = 0;
      while (Cards[index] != card)
        index++;
      counts[index]++;
    }
    int maxIndex = 1;
    for (int i = 1; i < counts.Length; i++)
      if (counts[maxIndex] < counts[i])
        maxIndex = i;
    counts[maxIndex] += counts[0];
    counts[0] = 0;
    counts = counts.Where(count => count > 0).ToArray();
    Array.Sort(counts);
    hand = counts.Aggregate("", (aggregate, elem) => elem.ToString() + aggregate);
    switch (hand)
    {
      case "11111":
        return HandType.HighCard;
      case "2111":
        return HandType.OnePair;
      case "221":
        return HandType.TwoPair;
      case "311":
        return HandType.ThreeOfKind;
      case "32":
        return HandType.FullHouse;
      case "41":
        return HandType.FourOfKind;
      case "5":
        return HandType.FiveOfKind;
      default:
        throw new Exception($"Something went wrong.Invalid hand {hand}");
    }
  }

  private struct Hand : IComparable
  {
    public string hand { get; }
    public long winning { get; }

    public Hand(string h, long win)
    {
      hand = h;
      winning = win;
    }

    public int CompareTo(object? h)
    {
      if (h == null) return 0;
      Hand otherHand = (Hand)h, thisHand = (Hand)this;
      HandType type1 = getHandType(this.hand), type2 = getHandType(otherHand.hand);
      if (type1 != type2)
        return (int)type1 - (int)type2;
      for (int i = 0; i < 5; i++)
      {
        if (thisHand.hand[i] == otherHand.hand[i]) continue;
        return Array.FindIndex(Cards, (ch) => ch == thisHand.hand[i]) -
               Array.FindIndex(Cards, (ch) => ch == otherHand.hand[i]);
      }
      return 0;
    }
  }

  public static void Main(string[] args)
  {
    string[] lines = Utility.GetLinesFromFile("./input.txt");
    Hand[] hands = new Hand[lines.Length];
    for (int index = 0; index < lines.Length; index++)
    {
      string[] tokens = lines[index].Split(' ', StringSplitOptions.RemoveEmptyEntries);
      string hand = tokens[0];
      long winnings = long.Parse(tokens[1]);
      hands[index] = new Hand(hand, winnings);
    }
    Array.Sort(hands);
    long result = 0;
    Console.WriteLine(hands.Length);
    for (int i = 0; i < hands.Length; i++)
      result += hands[i].winning * (i + 1);
    Console.WriteLine(result);
  }
}
