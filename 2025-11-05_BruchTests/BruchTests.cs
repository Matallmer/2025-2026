using System;
using Xunit;

public class BruchTests
{
    [Theory]
    [InlineData("1 1/4", "0 1/4", "ich bin ein bruch: 1 1/2")]
    [InlineData("1 2/4", "0 2/4", "ich bin ein bruch: 2")]
    [InlineData("0 1/3", "0 1/6", "ich bin ein bruch: 0 1/2")]
    public void Addiere_GibtGekuerztesErgebnisZurueck(string a, string b, string expected)
    {
        Bruch b1 = new Bruch(a);
        Bruch b2 = new Bruch(b);
        Assert.Equal(expected, b1.addiere(b2).toString());
    }

    [Theory]
    [InlineData("")]
    public void Konstruktor_Leer_WirftFormatException(string input)
    {
        Assert.Throws<FormatException>(() => new Bruch(input));
    }

    [Theory]
    [InlineData("1 1/0")]
    public void Konstruktor_NennerNull_WirftFormatException(string input)
    {
        Assert.Throws<FormatException>(() => new Bruch(input));
    }

    [Theory]
    [InlineData("a 1/2")]
    public void Konstruktor_UngueltigeZahl_WirftFormatException(string input)
    {
        Assert.Throws<FormatException>(() => new Bruch(input));
    }

    [Theory]
    [InlineData("1/2")]
    public void Konstruktor_FalschesFormat_WirftFormatException(string input)
    {
        Assert.Throws<FormatException>(() => new Bruch(input));
    }
}
