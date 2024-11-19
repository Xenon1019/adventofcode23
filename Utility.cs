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
        continue;
      if (line == null) break;
      lines.Add(line);
    };
    reader.Close();
    if (lines.Count == 0)
      throw new Exception("Input file is empty.");
    return lines.ToArray();
  }
}
