using System.Text.RegularExpressions;


class Day2
{
  private static string[] colors = { "red", "green", "blue" };

  public static int getIdFromMatch(Match match)
  {
    if (!match.Success) return -1;
    return int.Parse(match.Groups[1].Captures[0].Value);
  }

  public static (int count, string color) getColorDataFromMatch(Match match)
  {
    if (!match.Success || match.Groups.Count != 3)
    {
      Console.WriteLine("Match does not have 2 groups count.");
      return (-1, "");
    }
    int numBalls = int.Parse(match.Groups[1].Captures[0].Value);
    string color = match.Groups[2].Captures[0].Value;
    return (numBalls, color);
  }

  public static int parseLine(string line)
  {
    string gameRegexPat = @"Game\s+(\d+)\s*:";
    string colrosRegexPat = @"(\d+)\s+(red|green|blue)";

    int gameId = getIdFromMatch(Regex.Match(line, gameRegexPat));
    if (gameId == -1)
    {
      Console.WriteLine("Something went wrong while matching for game id.");
      return 0;
    }

    MatchCollection colorMatches = Regex.Matches(line, colrosRegexPat);
    if (colorMatches.Count == 0)
    {
      Console.WriteLine($"Something went wrong while matching for colors for game id {gameId}.");
      return 0;
    }
    int[] maxBallCounts = { 0, 0, 0 };
    foreach (Match match in colorMatches)
    {
      var color = getColorDataFromMatch(match);
      for (int i = 0; i < colors.Length; i++)
      {
        if (color.color == colors[i] && maxBallCounts[i] < color.count)
          maxBallCounts[i] = color.count;
      }
    }
    if (maxBallCounts[0] > 12 || maxBallCounts[1] > 13 || maxBallCounts[2] > 14)
      return 0;
    else return gameId;
  }
  public static void Main(string[] argsv)
  {
    int result = 0;
    string[] lines = Day1.readFromFile("./input.txt");
    Console.WriteLine($"{lines.Length} lines read from input.txt");
    foreach (string line in lines)
    {
      if (line == null) return;
      result += parseLine(line);
    }
    Console.WriteLine(result);
  }
}
