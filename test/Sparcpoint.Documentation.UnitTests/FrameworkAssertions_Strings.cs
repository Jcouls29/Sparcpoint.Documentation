using NUnit.Framework;

namespace Sparcpoint.Documentation.UnitTests;
public class FrameworkAssertions_Strings
{
    [Theory]
    [TestCase(null, null)]
    [TestCase("", "")]
    [TestCase("ABC123", "ABC123")]
    public void StringEqualsEachOther(string left, string right)
    {
        Assert.True(string.Equals(left, right));
    }

    [Theory]
    [TestCase(null, "")]
    [TestCase("", null)]
    [TestCase("", "ABC123")]
    [TestCase("ABC123", "")]
    public void StringDoesNotEqualEachOther(string left, string right)
    {
        Assert.False(string.Equals(left, right));
    }
}