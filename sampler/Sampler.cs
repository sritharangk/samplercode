namespace sampler;

public class Sampler
{
    // ---- 2.
    /*
      Write a function that accepts two Paths and returns the portion of the first Path that is not
      common with the second, which is to say portion of the first path starting from where the two
      paths diverged.
  
      For example, RelativeToCommonBase("/home/daniel/memes", "/home/daniel/work") should
      produce "/home/daniel".
    */
    public static String RelativeToCommonBase(String path1, String path2)
    {
       // Split the paths into arrays based on the directory separator
        var path1Parts = path1.Split('/');
        var path2Parts = path2.Split('/');
        
        int minLength = Math.Min(path1Parts.Length, path2Parts.Length);
        int divergenceIndex = 0;

        // Find the point where paths diverge
        for (int i = 0; i < minLength; i++)
        {
            if (path1Parts[i] != path2Parts[i])
            {
                divergenceIndex = i;
                break;
            }
        }

        // If the loop completes without finding a divergence
        if (divergenceIndex == 0 && path1Parts.Length == path2Parts.Length)
        {
            return path1; // No divergence
        }

        // Join the path1 up to the divergence point
        return string.Join("/", path1Parts, 0, divergenceIndex);
    }

    // ---- 2.
    /*
      Write a function that accepts a string as the first parameter, and a
      list of strings as the second parameter, and returns a string from the
      list that is "most like" the first string. The choice of algorithm 
      is yours.
    */
    public static String ClosestWord(String word, String[] possibilities)
    {
        if (string.IsNullOrEmpty(word) || possibilities == null || possibilities.Length == 0)
        {
            throw new ArgumentException("Parameters cannot be null or empty.");
        }

        // Calculate Levenshtein distances for each possibility
        var distances = possibilities.Select(p => (Word: p, Distance: LevenshteinDistance(word, p))).ToList();

        // Find the word with the smallest distance
        var closestWord = distances.OrderBy(d => d.Distance).First();

        return closestWord.Word;
    }

    private static int LevenshteinDistance(string source, string target)
    {
        if (string.IsNullOrEmpty(source))
        {
            return target.Length;
        }

        if (string.IsNullOrEmpty(target))   

        {
            return source.Length;   

        }

        int[,] distances = new int[source.Length + 1, target.Length + 1];

        for (int i = 0; i <= source.Length; i++)
        {
            distances[i, 0] = i;
        }

        for (int i = 0; i <= target.Length; i++)
        {
            distances[0, i] = i;
        }

        for (int i = 1; i <= source.Length; i++)
        {
            for (int j = 1; j <= target.Length; j++)
            {
                int cost = source[i - 1] == target[j - 1] ? 0 : 1;
                distances[i, j] = Math.Min(Math.Min(distances[i - 1, j] + 1, distances[i, j - 1] + 1), distances[i - 1, j - 1] + cost);
            }
        }

        return distances[source.Length, target.Length];
    }

    // ----3.
    /*
      Pretend there is a vehicle traveling along a path. The path is represented
      by a list of x, y points and a unix timestamp at that point 
      (the PointIntime struct).  The vehicle travels
      in straight lines between those points and passes through each point at
      the corresponding timestamp. Given this list of points and timestamps,
      and a time seconds (relative to the first timestamp), write a function
      that returns the instantaneous speed of the fake vehicle at that timestamp.
    */
    public struct PointInTime
    {
        public PointInTime(double x, double y, double timestamp)
        {
            X = x;
            Y = y;
            Timestamp = timestamp;
        }
  
        public double X { get; }
        public double Y { get; }
        public double Timestamp { get; }
  
        public override string ToString() => $"({X}, {Y}, {Timestamp})";
    }

    public static double Distance(PointInTime p1, PointInTime p2)
    {
        return Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));
    }

    public static double SpeedAtTime(double atTime, PointInTime[] path)
    {
        if (path == null || path.Length < 2)
        {
            throw new ArgumentException("Path must contain at least two points.");
        }

        double baseTime = path[0].Timestamp + atTime; // Convert relative time to absolute time

        // Find which segment the vehicle is on at the given time
        for (int i = 1; i < path.Length; i++)
        {
            var prevPoint = path[i - 1];
            var nextPoint = path[i];

            // Check if the baseTime is between the current and next point
            if (baseTime >= prevPoint.Timestamp && baseTime <= nextPoint.Timestamp)
            {
                double distance = Distance(prevPoint, nextPoint);
                double timeInterval = nextPoint.Timestamp - prevPoint.Timestamp;

                // Speed = distance / time interval
                return distance / timeInterval;
            }
        }

        // If the time is after the last timestamp, return 0 or the speed of the last segment
        return 0;
    }

}
