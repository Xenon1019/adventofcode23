public class Day3
{
  public struct Location
  {
    public int Row { get; set; }
    public int Col { get; set; }

    public Location(int row, int col)
    {
      Row = row;
      Col = col;
    }

    public bool isValid(int maxRows, int maxCols)
    {
      return Row >= maxRows || Col >= maxCols;
    }

    public static Location operator +(Location loc1, Location loc2)
    {
      return new Location(loc1.Row + loc2.Row, loc1.Col + loc2.Col);
    }
  }

  public static Location Up = new Location(-1, 0);
  public static Location Down = new Location(1, 0);
  public static Location Left = new Location(0, -1);
  public static Location Right = new Location(0, 1);

  public static Location[] Directions = new Location[] { Up, Right, Left, Down, Up + Left, Up + Right, Down + Left, Down + Right };

  public static bool inputIsValid(string[] lines)
  {
    if (lines.Length == 0)
      return false;
    int cols = lines[0].Length;
    foreach (string line in lines)
      if (line.Length != cols)
        return false;
    return true;
  }

  public static int parseNumberStartingFrom(string[] lines, Location loc)
  {
    string num = "";
    while (loc.Col < lines[0].Length && char.IsDigit(lines[loc.Row][loc.Col]))
    {
      num += lines[loc.Row][loc.Col];
      loc.Col++;
    }
    return int.Parse(num);
  }

  private static Location? addNumberBegining(string[] lines, Location loc)
  {
    if (loc.Row < 0 || loc.Row >= lines.Length || loc.Col < 0 || loc.Col >= lines[0].Length)
      return null;
    if (!char.IsDigit(lines[loc.Row][loc.Col]))
      return null;

    Location beginning = loc;

    while (loc.Col >= 0 && char.IsDigit(lines[loc.Row][loc.Col]))
    {
      beginning = loc;
      loc.Col--;
    }
    return beginning;
  }

  public static void Main(string[] args)
  {
    string[] lines = Utility.GetLinesFromFile("./input.txt");
    if (!inputIsValid(lines))
      throw new Exception("Input invalidation Failed");

    int numRows, numCols;
    numRows = lines.Length;
    numCols = lines[0].Length;


    List<Location> symbols = new List<Location>();
    HashSet<Location> numbers = new HashSet<Location>();


    for (int row = 0; row < numRows; row++)
    {
      for (int col = 0; col < numCols; col++)
      {
        if (char.IsDigit(lines[row][col]) || lines[row][col] == '.')
          continue;
        symbols.Add(new Location(row, col));
      }
    }

    long result = 0;
    foreach (Location loc in symbols)
    {
      if (lines[loc.Row][loc.Col] != '*')
        continue;
      List<Location> adjacentNumbersFound = new List<Location>();
      foreach (Location direction in Directions)
      {
        Location newLoc = loc + direction;
        Location? numberFound = addNumberBegining(lines, newLoc);
        if (numberFound == null)
          continue;
        if (adjacentNumbersFound.Contains((Location)numberFound))
          continue;
        adjacentNumbersFound.Add((Location)numberFound);
      }
      if (adjacentNumbersFound.Count == 2)
      {
        long num1 = parseNumberStartingFrom(lines, adjacentNumbersFound[0]);
        long num2 = parseNumberStartingFrom(lines, adjacentNumbersFound[1]);
        result += num1 * num2;
      }
    }
    Console.WriteLine(result);
  }
}
