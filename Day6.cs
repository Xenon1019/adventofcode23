public class Day6
{
  public static void Main(string[] args)
  {
    string[] lines = Utility.GetLinesFromFile("./input.txt");
    string[] timeSplit = lines[0].Split(new char[] { ' ', ':' }, StringSplitOptions.RemoveEmptyEntries);
    string[] distanceSplit = lines[1].Split(new char[] { ' ', ':' }, StringSplitOptions.RemoveEmptyEntries);
    if (timeSplit.Length != distanceSplit.Length)
      throw new Exception("Invalid input");
    int count = timeSplit.Length - 1;
    string timeStr = "", distanceStr = "";
    for (int index = 0; index < count; index++)
    {
      timeStr += timeSplit[index + 1];
      distanceStr += distanceSplit[index + 1];
    }
    long t = long.Parse(timeStr);
    long d = long.Parse(distanceStr);
    double x1 = 0.5 * (t - double.Sqrt(t * t - 4 * d));
    double x2 = 0.5 * (t + double.Sqrt(t * t - 4 * d));
    long result = 0;
    if (double.IsInteger(x1)) result--;
    if (double.IsInteger(x2)) result--;
    result += 1 + (int)(double.Floor(x2) - double.Ceiling(x1));
    Console.WriteLine(result);
  }
}
