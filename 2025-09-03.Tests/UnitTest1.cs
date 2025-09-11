namespace _2025_09_03.Tests;

public class FractionTests
{
    [Fact]
    public void TestAddition()
    {
        
        Fraction f1 = Fraction.Parse("1 1/2");
        Fraction f2 = Fraction.Parse("1/4");
        string expected = "1 3/4";

        
        Fraction result = f1 + f2;
        string actual = result.ToMixedString();

        
        Assert.Equal(expected, actual);
    }
}
