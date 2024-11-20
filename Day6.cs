public class Day6
{
  public static void Main(string[] args)
  {
    string[] lines = Utility.GetLinesFromFile("./input.txt");
    long result = 1;
    string[] timeSplit = lines[0].Split(new char[] { ' ', ':' }, StringSplitOptions.RemoveEmptyEntries);
    string[] distanceSplit = lines[1].Split(new char[] { ' ', ':' }, StringSplitOptions.RemoveEmptyEntries);
    if (timeSplit.Length != distanceSplit.Length)
      throw new Exception("Invalid input");
    int count = timeSplit.Length - 1;
    long num;
    for (int index = 0; index < count; index++)
    {
      long t = long.Parse(timeSplit[index + 1]);
      long d = long.Parse(distanceSplit[index + 1]);
      double x1 = 0.5 * (t - double.Sqrt(t * t - 4 * d));
      double x2 = 0.5 * (t + double.Sqrt(t * t - 4 * d));
      num = 0;
      if (double.IsInteger(x1)) num--;
      if (double.IsInteger(x2)) num--;
      num += 1 + (int)(double.Floor(x2) - double.Ceiling(x1));
      result *= num;
    }
    Console.WriteLine(result);
  }
}
