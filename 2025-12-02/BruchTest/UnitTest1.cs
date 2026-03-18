using Xunit;
using Bruch;

namespace BruchTest;

public class UnitTest1
{
    [Theory]
    [InlineData("1 1/2", "1 1/2")]
    [InlineData("4 0/1", "4")]
    [InlineData("2 1/2", "2 1/2")]
    [InlineData("0 3/2", "1 1/2")]
    [InlineData("0 2/4", "0 1/2")]
    [InlineData("1 6/8", "1 3/4")]
    public void Constructor_String_ProducesExpectedToString(string input, string expected)
    {
        var b = new Bruch.Bruch(input);
        Assert.Equal(expected, b.ToString());
    }

    [Fact]
    public void Constructor_EmptyString_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new Bruch.Bruch("") );
    }

    [Fact]
    public void Constructor_Null_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new Bruch.Bruch(null!));
    }

    [Fact]
    public void Constructor_NennerZero_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new Bruch.Bruch("1 1/0"));
    }

    [Fact]
    public void IntConstructor_NennerZero_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new Bruch.Bruch(1, 1, 0));
    }

    [Fact]
    public void Addition_Works_ForFractionsAndMixedNumbers()
    {
        var a = new Bruch.Bruch("0 1/2");
        var b = new Bruch.Bruch("0 1/4");
        var sum1 = a.Addiere(b);
        Assert.Equal("0 3/4", sum1.ToString());

        var c = new Bruch.Bruch("1 1/2");
        var d = new Bruch.Bruch("2 1/2");
        Assert.Equal("4", c.Addiere(d).ToString());
    }
}
