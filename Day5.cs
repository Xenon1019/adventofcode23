//
//
//       ****** I know this code is messy, ubly and horrible.********
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

  public static IList getList(string token)
  {
    switch (token)
    {
      case "seeds":
        return seeds;
      case "seed-to-soil":
        return seedToSoil;
      case "soil-to-fertilizer":
        return soilToFertiliser;
      case "fertilizer-to-water":
        return fertiliserToWater;
      case "water-to-light":
        return waterToLight;
      case "light-to-temperature":
        return lightToTemprature;
      case "temperature-to-humidity":
        return tempratureToHumidity;
      case "humidity-to-location":
        return humidityToLocation;
      default:
        throw new ArgumentException($"{token} not recognized");
    }
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
        throw new Exception($"You are trying to mapo {number} that is outside the range ({Min},{Max})");
      return MapTo + number - Min;
    }

    public int CompareTo(object? val)
    {
      if (val is long)
      {
        long number = (long)val;
        if (number < Min) return 1;
        else if (number > Max) return -1;
        else return 0;
      }
      else if (val is Range)
      {
        Range range = (Range)val;
        if (Max < range.Min) return -1;
        else if (Min > range.Max) return 1;
        else return 0;
      }
      throw new ArgumentException("Value is not of type long or Range");
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
        string[] tokens = line.Split(new char[] { ':', ' ' }, StringSplitOptions.RemoveEmptyEntries);
        lastCategory = tokens[0];
        if (lastCategory == "seeds")
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
        List<Range> list = getList(lastCategory) as List<Range>;
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

  public static long map(long seed)
  {
    int result = seedToSoil.BinarySearch(new Range(seed, seed, 1));
    long soil;
    if (result < 0)
      soil = seed;
    else
      soil = seedToSoil[result].Map(seed);
    result = soilToFertiliser.BinarySearch(new Range(soil, soil, 1));
    long fertiliser;
    if (result < 0)
      fertiliser = soil;
    else fertiliser = soilToFertiliser[result].Map(soil);
    result = fertiliserToWater.BinarySearch(new Range(fertiliser, fertiliser, 1));
    long water;
    if (result < 0)
      water = fertiliser;
    else water = fertiliserToWater[result].Map(fertiliser);
    result = waterToLight.BinarySearch(new Range(water, water, 1));
    long light;
    if (result < 0)
      light = water;
    else light = waterToLight[result].Map(water);
    result = lightToTemprature.BinarySearch(new Range(light, light, 1));
    long temp;
    if (result < 0)
      temp = light;
    else temp = lightToTemprature[result].Map(light);
    result = tempratureToHumidity.BinarySearch(new Range(temp, temp, 1));
    long humidity;
    if (result < 0)
      humidity = temp;
    else humidity = tempratureToHumidity[result].Map(temp);
    result = humidityToLocation.BinarySearch(new Range(humidity, humidity, 1));
    if (result < 0)
      return humidity;
    else return humidityToLocation[result].Map(humidity);
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
    seedToSoil.Sort();
    soilToFertiliser.Sort();
    fertiliserToWater.Sort();
    waterToLight.Sort();
    lightToTemprature.Sort();
    tempratureToHumidity.Sort();
    humidityToLocation.Sort();

    long minLocation = long.MaxValue;
    int count = 1;
    foreach (Range seedRange in seeds)
    {
      for (long seed = seedRange.Min; seed < seedRange.Max; seed++)
        minLocation = long.Min(minLocation, map(seed));
      Console.WriteLine($"Seed Chunk {count++}/{seeds.Count} done.");
    }
    Console.WriteLine(minLocation);
  }
}
