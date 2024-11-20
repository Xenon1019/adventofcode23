//
//
//       ****** I know this code is messy, ugly and horrible.********
//       ****** I could use a lot more adjectives to describe it.*********
//       ****** But currently this is the best I can do. *********
//       ****** May revisit this at some time!!**************
//
//
//
//
using System.Collections;

public class Day5
{
  public static List<Range> seeds = new List<Range>();
  public static List<Range> seedToSoil = new List<Range>();
  public static List<Range> soilToFertiliser = new List<Range>();
  public static List<Range> fertiliserToWater = new List<Range>();
  public static List<Range> waterToLight = new List<Range>();
  public static List<Range> lightToTemprature = new List<Range>();
  public static List<Range> tempratureToHumidity = new List<Range>();
  public static List<Range> humidityToLocation = new List<Range>();

  public static List<Range>[] maps = { seedToSoil, soilToFertiliser, fertiliserToWater,
                                       waterToLight, lightToTemprature, tempratureToHumidity,
                                       humidityToLocation };

  public static int getMapIndex(string token)
  {
    string[] mapNames = {"seed-to-soil", "soil-to-fertilizer", "fertilizer-to-water",
                         "water-to-light", "light-to-temperature", "temperature-to-humidity",
                         "humidity-to-location"};
    for (int index = 0; index < mapNames.Length; index++)
      if (token == mapNames[index])
        return index;
    throw new Exception($"Invalid map token {token}");
  }

  public class Range : IComparable
  {
    public long Min { get; }
    public long Max { get; }
    public long MapTo { get; }

    public Range(long min, long max, long mapTo)
    {
      if (max < min)
        throw new Exception($"Invalid range constructed ({min},{max}).");
      Min = min;
      Max = max;
      MapTo = mapTo;
    }

    public long Map(long number)
    {
      if (number < Min || number > Max)
        throw new Exception($"You are trying to map {number} that is outside the range ({Min},{Max})");
      return MapTo + number - Min;
    }

    public int CompareTo(object? val)
    {
      if (val is Range)
      {
        Range range = (Range)val;
        if (Max < range.Min) return -1;
        else if (Min > range.Max) return 1;
        else return 0;
      }
      throw new ArgumentException("Value is not of type or Range");
    }
  }

  static void parseInput(string[] lines)
  {
    string? lastCategory = null;
    for (int i = 0; i < lines.Length; i++)
    {
      string line = lines[i];
      if (line.Contains(':'))
      {
        string[] tokens = line.Split(new char[] { ':', ' ' },
                            StringSplitOptions.RemoveEmptyEntries);
        lastCategory = tokens[0];
        if (lastCategory != "seeds") continue;
        {
          for (int j = 1; j < tokens.Length; j += 2)
          {
            long min = long.Parse(tokens[j]), size = long.Parse(tokens[j + 1]);
            seeds.Add(new Range(min, size + min - 1, 1));
          }
        }
      }
      else
      {
        List<Range> list = maps[getMapIndex(lastCategory)];
        string[] numbers = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (numbers.Length != 3)
          throw new Exception($"Invalid input line : {line}");
        long mapTo = long.Parse(numbers[0]);
        long mapFrom = long.Parse(numbers[1]);
        long rangeSize = long.Parse(numbers[2]);
        Range rg = new Range(mapFrom, mapFrom + rangeSize - 1, mapTo);
        list.Add(rg);
      }
    }
  }

  public static (long result, long consumed) recursiveMap(long seedStart, int mapIndex)
  {
    if (mapIndex == maps.Length)
      return (result: seedStart, consumed: long.MaxValue);
    List<Range> map = maps[mapIndex];
    int search = map.BinarySearch(new Range(seedStart, seedStart, 1));
    long mapResult, consumedResult;
    if (search < 0)
    {
      search = ~search;
      (mapResult, consumedResult) = recursiveMap(seedStart, mapIndex + 1);
      if (search == map.Count)
        return (result: mapResult, consumed: consumedResult);
      else
        return (result: mapResult, consumed: long.Min(consumedResult, map[search].Min - seedStart));
    }
    long tempMapRes = map[search].Map(seedStart);
    (mapResult, consumedResult) = recursiveMap(tempMapRes, mapIndex + 1);
    return (result: mapResult, consumed: long.Min(consumedResult, map[search].Max - seedStart + 1));
  }

  public static void Main()
  {
    string[] lines = Utility.GetLinesFromFile("./input.txt");
    parseInput(lines);
    var printList = (IList objArray) =>
    {
      foreach (object obj in objArray)
        Console.Write(obj.ToString() + ' ');
      Console.Write('\n');
    };
    foreach (List<Range> ranges in maps)
      ranges.Sort();

    long minLocation = long.MaxValue;
    int count = 1;
    foreach (Range seedRange in seeds)
    {
      long seed = seedRange.Min;
      while (seed < seedRange.Max)
      {
        (long result, long consumed) = recursiveMap(seed, 0);
        if (consumed == 0)
          throw new Exception($"Something went wrong {consumed} consumed for seed {seed}.");
        seed += consumed;
        minLocation = long.Min(minLocation, result);
      }
      Console.WriteLine($"Seed Chunk {count++}/{seeds.Count} done.");
    }
    Console.WriteLine(minLocation);
  }
}
