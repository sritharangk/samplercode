
using sampler;
using Xunit;
using static sampler.Sampler;

public class SamplerTests {

    // Function - RelativeToCommonBase
    [Fact]
    public void Test_CommonBasePaths()
    {
        // Case: Same base directories, different subfolders
        string path1 = "/home/daniel/memes";
        string path2 = "/home/daniel/work";

        string result = RelativeToCommonBase(path1, path2);
        Assert.Equal("/home/daniel", result);
    }

    [Fact]
    public void Test_IdenticalPaths()
    {
        // Case: Identical paths
        string path1 = "/home/daniel/memes";
        string path2 = "/home/daniel/memes";

        string result = RelativeToCommonBase(path1, path2);
        Assert.Equal("/home/daniel/memes", result);
    }

    [Fact]
    public void Test_NoCommonBase()
    {
        // Case: No common base directories
        string path1 = "/home/daniel/memes";
        string path2 = "/var/logs";

        string result = RelativeToCommonBase(path1, path2);
        Assert.Equal("", result); // No common base, empty string expected
    }

    [Fact]
    public void Test_OnePathIsRoot()
    {
        // Case: One path is the root directory
        string path1 = "/home/daniel/memes";
        string path2 = "/";

        string result = RelativeToCommonBase(path1, path2);
        Assert.Equal("", result); // No common base because root is involved
    }

    [Fact]
    public void Test_EmptyPaths()
    {
        // Case: Both paths are empty strings
        string path1 = "";
        string path2 = "";

        string result = RelativeToCommonBase(path1, path2);
        Assert.Equal("", result); // Empty paths
    }

    [Fact]
    public void Test_RootPathOnly()
    {
        // Case: One of the paths is root and other is a directory
        string path1 = "/";
        string path2 = "/home/daniel/memes";

        string result = RelativeToCommonBase(path1, path2);
        Assert.Equal("", result); // No common base, root
    }

    // Function - ClosestWord
    [Fact]
    public void ClosestWord_EmptyParameters_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => ClosestWord("", []));
        Assert.Throws<ArgumentException>(() => ClosestWord("", ["hello"]));
        Assert.Throws<ArgumentException>(() => ClosestWord("hello", []));
    }

    [Fact]
    public void ClosestWord_SinglePossibility_ReturnsSinglePossibility()
    {
        Assert.Equal("hello", ClosestWord("hello", ["hello"]));
    }

    [Fact]
    public void ClosestWord_ExactMatch_ReturnsExactMatch()
    {
        Assert.Equal("hello", ClosestWord("hello", ["hello", "world"]));
    }

    [Fact]
    public void ClosestWord_ClosestMatch_ReturnsClosestMatch()
    {
        Assert.Equal("hello", ClosestWord("helo", ["hello", "world"]));
    }

    [Fact]
    public void ClosestWord_MultipleClosestMatches_ReturnsOneOfClosestMatches()
    {
        Assert.Equal("hello", ClosestWord("helo", ["hello", "world", "hllo"]));
    }

    // Function - SpeedAtTime
    [Fact]
    public void Test_SpeedAtTime_OnPoint1()
    {
        PointInTime[] path = new[]
        {
            new PointInTime(0, 0, 1000),
            new PointInTime(3, 4, 1010),
            new PointInTime(6, 8, 1020)
        };

        double atTime = 0; // At the first timestamp
        double result = SpeedAtTime(atTime, path);

        Assert.Equal(0.5, result); // Speed between (0,0) and (3,4) is 0.5 units/sec
    }

    [Fact]
    public void Test_SpeedAtTime_BetweenPoints()
    {
        PointInTime[] path = new[]
        {
            new PointInTime(0, 0, 1000),
            new PointInTime(3, 4, 1010),
            new PointInTime(6, 8, 1020)
        };

        double atTime = 5; // 5 seconds after the first timestamp (1005)
        double result = SpeedAtTime(atTime, path);

        Assert.Equal(0.5, result); // Still on the same segment, speed is 0.5 units/sec
    }

    [Fact]
    public void Test_SpeedAtTime_ExactOnPoint2()
    {
        PointInTime[] path = new[]
        {
            new PointInTime(0, 0, 1000),
            new PointInTime(3, 4, 1010),
            new PointInTime(6, 8, 1020)
        };

        double atTime = 10; // Exactly at timestamp 1010
        double result = SpeedAtTime(atTime, path);

        Assert.Equal(0.5, result); // Speed on the next segment between (3,4) and (6,8) is also 0.5 units/sec
    }

    [Fact]
    public void Test_SpeedAtTime_AfterLastPoint()
    {
        PointInTime[] path = new[]
        {
            new PointInTime(0, 0, 1000),
            new PointInTime(3, 4, 1010),
            new PointInTime(6, 8, 1020)
        };

        double atTime = 25; // After the last point (timestamp 1025)
        double result = SpeedAtTime(atTime, path);

        Assert.Equal(0, result); // After the last point, no movement, speed is 0
    }

    [Fact]
    public void Test_SpeedAtTime_InvalidPath()
    {
        PointInTime[] path = new PointInTime[] { }; // Empty path

        Assert.Throws<ArgumentException>(() => SpeedAtTime(5, path));
    }

    [Fact]
    public void Test_SpeedAtTime_ShortPath()
    {
        PointInTime[] path = new[]
        {
            new PointInTime(0, 0, 1000),
        };

        Assert.Throws<ArgumentException>(() => SpeedAtTime(5, path)); // Path too short
    }
}