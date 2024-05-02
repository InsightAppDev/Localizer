using System;
using System.IO;
using System.Linq;
using Lclzr.Providers.Files.RawFiles;
using Lclzr.Tests.Infrastructure;
using Xunit;

namespace Lclzr.Tests;

public sealed class LocalizerMultipleLanguagesInFileTests
{
    private static RawFilesBlocksProviderOptions _options = new()
    {
        Path = "Resources" + Path.DirectorySeparatorChar + "MultipleLanguagesInFile",
    };

    private readonly ILocalizer _localizer;

    public LocalizerMultipleLanguagesInFileTests()
    {
        var sut = new LocalizerWithRawFilesProviderSut(_options);
        _localizer = sut.Localizer;
    }

    [Fact]
    public void Ctor_initializes_block_named_multiple()
    {
        Assert.Single(_localizer.AvailableBlockNames);
        Assert.Equal("multiple", _localizer.AvailableBlockNames.First(), StringComparer.OrdinalIgnoreCase);
    }
    
    [Fact]
    public void Ctor_initializes_block_named_multiple_with_two_cultures_and_hello_key()
    {
        Assert.Equal("Привет", _localizer.GetByCulture("ru", "multiple", "Hello"), StringComparer.OrdinalIgnoreCase);
        Assert.Equal("Hi", _localizer.GetByCulture("en", "multiple", "Hello"), StringComparer.OrdinalIgnoreCase);
    }
}