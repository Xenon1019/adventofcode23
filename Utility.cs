public class Utility
{
  public static string[] GetLinesFromFile(string path)
  {
    StreamReader reader = new StreamReader(path);
    List<string> lines = new List<string>();
    string? line;
    while (true)
    {
      line = reader.ReadLine();
      if (line == String.Empty)
        throw new Exception("Empty line found in input.");
      if (line == null) break;
      lines.Add(line);
    };
    if (lines.Count == 0)
      throw new Exception("Input file is empty.");
    return lines.ToArray();
  }
}
