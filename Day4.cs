using System.Text.RegularExpressions;

public class Day4
{
  public static int parse(string line)
  {
    string numberPattern = @":(?:\s*(\d+)\s*)+\|(?:\s*(\d+)\s*)+$";

    Match match = Regex.Match(line, numberPattern);

    HashSet<int> lotteryNumbers = new HashSet<int>(match.Groups[1].Captures.Count);
    foreach (Capture lotteryNumberCapture in match.Groups[1].Captures)
      lotteryNumbers.Add(int.Parse(lotteryNumberCapture.ToString()));
    int count = 0;
    foreach (Capture yourNumberCapture in match.Groups[2].Captures)
    {
      if (lotteryNumbers.Contains(int.Parse(yourNumberCapture.ToString())))
        count++;
    }
    return count;
  }


  public static void Main(string[] args)
  {
    string[] lines = Utility.GetLinesFromFile("./input.txt");
    int numberOfCards = lines.Length;
    int[] cardsCount = new int[numberOfCards];
    Array.Fill(cardsCount, 1);

    for (int i = 0; i < lines.Length; i++)
    {
      int copyCount = parse(lines[i]);
      for (int j = i + 1; j < i + 1 + copyCount && j < lines.Length; j++)
      {
        cardsCount[j] += cardsCount[i];
      }
    }
    Console.WriteLine(cardsCount.Sum());
  }
}
