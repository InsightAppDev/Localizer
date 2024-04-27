using Xunit;

namespace Insight.Localizer.Tests;

public sealed class BlockTests
{
    private readonly Block _block;

    public BlockTests()
    {
        _block = new Block("messages");
    }

    [Fact]
    public void Get_throws_MissingLocalizationException_when_key_is_missing()
    {
        Assert.Throws<MissingLocalizationException>(() => _block.Get("ru-ru", "Hello"));
    }
}