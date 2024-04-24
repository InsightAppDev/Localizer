using Xunit;

namespace Insight.Localizer.Tests;

[Collection("LocalizerTests")]
public sealed class GenericLocalizerTests
{
    private readonly Localizer<GenericLocalizerTests> _localizer;

    private static LocalizerOptions Options => new()
    {
        Path = "Resources",
        FileEndsWith = ".json",
        ReadNestedFolders = true
    };

    public GenericLocalizerTests()
    {
        Localizer.Initialize(Options);
        Localizer.CurrentCulture = "ru-ru";
        _localizer = new Localizer<GenericLocalizerTests>();
    }

    [Fact]
    public void Get_returns_value_based_on_generic_argument_name()
    {
        var value = _localizer.Get("test");
        Assert.Equal("Hello!", value);
    }
}