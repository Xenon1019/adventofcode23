using System.Text.RegularExpressions;

public class Day1
{
  private static string pattern = "1|2|3|4|5|6|7|8|9|one|two|three|four|five|six|seven|eight|nine|zero";
  private static string revPattern = reverse(pattern);
  public static string[] digits = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

  public static string[] getLines(int numLines)
  {
    string[] lines = new string[numLines];
    for (int i = 0; i < numLines; i++)
    {
      string? line = Console.ReadLine();
      if (line != null)
        lines[i] = line;
    }
    return lines;
  }

  public static int getDigitFromMatch(Match match, bool isReversed)
  {
    string matchString = match.Value;
    if (matchString.Length == 1)
      return matchString[0] - '0';
    else
    {
      if (isReversed)
        matchString = reverse(matchString);
      for (int i = 0; i < digits.Length; i++)
        if (digits[i] == matchString)
          return i;
    }
    return -1;
  }

  public static string reverse(string str)
  {
    char[] strArray = str.ToCharArray();
    Array.Reverse(strArray);
    return new string(strArray);
  }

  public static int parseLine(string line)
  {
    if (revPattern == null) return -1;

    Match match = Regex.Match(line, pattern);
    Match revMatch = Regex.Match(reverse(line), revPattern);

    int firstDigit, secondDigit;
    firstDigit = getDigitFromMatch(match, false);
    secondDigit = getDigitFromMatch(revMatch, true);
    /*Console.WriteLine($"{line} -> {firstDigit}{secondDigit}");*/
    return 10 * firstDigit + secondDigit;
  }

  static public string[] readFromFile(string path)
  {
    StreamReader reader = new StreamReader(path);
    List<string> lines = new List<string>();
    string? line;
    while (true)
    {
      line = reader.ReadLine();
      if (line == String.Empty)
        continue;
      if (line == null) break;
      lines.Add(line);
    };
    return lines.ToArray();
  }

  public static int Main()
  {
    int result = 0;
    string[] lines = readFromFile("./input.txt");
    Console.WriteLine($"{lines.Length} lines read from input.txt");
    foreach (string? line in lines)
    {
      if (line == null) return -1;
      int number = parseLine(line);
      result += number;
    }
    Console.WriteLine(result);
    return 0;
  }
}
