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
    return (count == 0) ? 0 : (1 << (count - 1));
  }


  public static void Main(string[] args)
  {
    string[] lines = Utility.GetLinesFromFile("./input.txt");
    int answer = 0;
    foreach (string line in lines)
    {
      answer += parse(line);
    }
    Console.WriteLine(answer);
  }
}
