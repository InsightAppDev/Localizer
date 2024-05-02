using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lclzr.Exceptions;
using Lclzr.Registries;
using Lclzr.Tests.Infrastructure;
using Xunit;

namespace Lclzr.Tests;

public sealed class LocalizerRegistryTests
{
    [Fact]
    public void Get_throws_MissingBlockException_when_block_is_missing()
    {
        Assert.Throws<MissingBlockException>(() =>
            new LocalizerRegistry().GetByCulture("ru-ru", "there_is_no_block", "Hello"));
    }

    [Fact]
    public async Task Get_any_returns_correct_value()
    {
        var languageBlock = new Block("language");
        languageBlock.Add(LocalizerConstants.AnyCultureKey, new Dictionary<string, string>
        {
            {"Russian", "Русский"},
            {"English", "English"}
        });

        var testProvider = new TestBlocksProvider(languageBlock);
        var registry = new LocalizerRegistry(testProvider);
        await registry.Initialize();
        
        var russianLanguage = registry.GetAny("language", "Russian");
        var englishLanguage = registry.GetAny("language", "English");

        Assert.Equal("Русский", russianLanguage, StringComparer.OrdinalIgnoreCase);
        Assert.Equal("English", englishLanguage, StringComparer.OrdinalIgnoreCase);
    }
}