using System;
using System.IO;
using System.Linq;
using Insight.Localizer.Providers.Files.RawFiles;
using Xunit;

namespace Insight.Localizer.Tests;

public sealed class LocalizerOneLanguageInFileTests
{
    private static RawFilesBlocksProviderOptions _options = new()
    {
        Path = "Resources" + Path.DirectorySeparatorChar + "OneLanguageInFile",
        ReadNestedFolders = true
    };

    private readonly ILocalizer _localizer;

    public LocalizerOneLanguageInFileTests()
    {
        var sut = new LocalizerWithRawFilesProviderSut(_options);
        _localizer = sut.Localizer;
        _localizer.CurrentCulture = new LocalizerCulture("ru-ru");
    }

    [Fact]
    public void AvailableBlockNames_returns_all_block_names()
    {
        var names = _localizer.AvailableBlockNames;
        Assert.NotNull(names);
        Assert.NotEmpty(names);
        Assert.Equal(4, names.Count);
        Assert.NotNull(names.FirstOrDefault(x => x.Equals("test", StringComparison.OrdinalIgnoreCase)));
        Assert.NotNull(names.FirstOrDefault(x =>
            x.Equals("messages", StringComparison.OrdinalIgnoreCase)));
        Assert.NotNull(names.FirstOrDefault(x =>
            x.Equals("language", StringComparison.OrdinalIgnoreCase)));
    }

    [Fact]
    public void Get_returns_value_for_current_culture()
    {
        var value = _localizer.Get("test", "Hello");

        Assert.Equal("Привет", value, StringComparer.OrdinalIgnoreCase);
    }
}