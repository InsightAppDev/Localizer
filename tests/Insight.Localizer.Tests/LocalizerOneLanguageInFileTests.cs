using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Options;
using Xunit;

namespace Insight.Localizer.Tests
{
    [Collection("LocalizerTests")]
    public sealed class LocalizerMultipleLanguagesInFileTests
    {
        private static LocalizerOptions _options = new()
        {
            Path = "Resources" + Path.DirectorySeparatorChar + "MultipleLanguagesInFile",
            OneLanguageInFile = false,
            ReadNestedFolders = true
        };

        public LocalizerMultipleLanguagesInFileTests()
        {
            Localizer.Initialize(_options);
            Localizer.CurrentCulture = "ru";
        }

        [Fact]
        public void Ctor_initializes_block_test_with_two_cultures_and_hello_key_from_single_file()
        {
            var localizer = new Localizer();

            Assert.Equal(1, localizer.AvailableBlockNames.Count);
            Assert.Equal("test", localizer.AvailableBlockNames.First(), StringComparer.OrdinalIgnoreCase);
            Assert.Equal("Привет", localizer.GetByCulture("ru", "test", "Hello"), StringComparer.OrdinalIgnoreCase);
            Assert.Equal("Hi", localizer.GetByCulture("en", "test", "Hello"), StringComparer.OrdinalIgnoreCase);
        }
    }

    [Collection("LocalizerTests")]
    public sealed class LocalizerOneLanguageInFileTests
    {
        private static readonly LocalizerOptions Options = new()
        {
            Path = "Resources" + Path.DirectorySeparatorChar + "OneLanguageInFile",
            FileEndsWith = ".json",
            ReadNestedFolders = true
        };

        public LocalizerOneLanguageInFileTests()
        {
            Localizer.Initialize(Options);
            Localizer.CurrentCulture = "ru-ru";
        }

        [Fact]
        public void Should_throw_ANE_if_configuration_at_Initialize_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => Localizer.Initialize(null));
        }

        [Fact]
        public void Should_throw_ANE_if_name_at_block_ctor_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => new Block(null));
        }

        [Fact]
        public void Should_throw_MissingBlockException()
        {
            var localizer = new Localizer();
            Assert.Throws<MissingBlockException>(() => localizer.Get("there_is_no_block", "Hello"));
        }

        [Fact]
        public void Should_throw_MissingLocalizationException_if_there_is_no_culture()
        {
            Localizer.CurrentCulture = "ke-ke";
            var localizer = new Localizer();
            Assert.Throws<MissingLocalizationException>(() => localizer.Get("messages", "Hello"));
        }

        [Fact]
        public void Should_throw_MissingLocalizationException_if_there_is_no_key()
        {
            var localizer = new Localizer();
            Assert.Throws<MissingLocalizationException>(() => localizer.Get("messages", "there_is_no_localization"));
        }

        [Fact]
        public void Should_read_all_files()
        {
            var localizer = new Localizer();
            AssertLocalizer(localizer, 4);
        }

        [Fact]
        public void Should_get_available_block_names_from_all_Files()
        {
            var localizer = new Localizer();
            AssertLocalizer(localizer, 4);

            var names = localizer.AvailableBlockNames;
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
        public void Should_get_value_in_all_languages()
        {
            var localizer = new Localizer();
            AssertLocalizer(localizer, 4);

            var en = localizer.GetByCulture("en-us", "test", "Hello");
            var ru = localizer.Get("test", "Hello");

            Assert.Equal("Hi", en, StringComparer.OrdinalIgnoreCase);
            Assert.Equal("Привет", ru, StringComparer.OrdinalIgnoreCase);
        }

        [Fact]
        public void Should_get_any_value()
        {
            var localizer = new Localizer();
            AssertLocalizer(localizer, 4);

            var russianLanguage = localizer.GetAny("language", "Russian");
            var englishLanguage = localizer.GetAny("language", "English");

            Assert.Equal("Русский", russianLanguage, StringComparer.OrdinalIgnoreCase);
            Assert.Equal("English", englishLanguage, StringComparer.OrdinalIgnoreCase);
        }

        [Fact]
        public void Should_change_current_culture()
        {
            var localizer = new Localizer();
            AssertLocalizer(localizer, 4);

            Assert.Equal("ru-ru", Localizer.CurrentCulture, StringComparer.OrdinalIgnoreCase);
            Localizer.CurrentCulture = "en-us";
            Assert.Equal("en-us", Localizer.CurrentCulture, StringComparer.OrdinalIgnoreCase);
        }

        [Fact]
        public void Should_throw_ANE_on_set_culture_if_culture_is_null()
        {
            var localizer = new Localizer();
            AssertLocalizer(localizer, 4);

            Assert.Throws<ArgumentNullException>(() => Localizer.CurrentCulture = null);
        }

        private void AssertLocalizer(ILocalizer localizer, int expectedBlocksCount)
        {
            Assert.NotNull(localizer);
            Assert.NotEmpty(localizer.Blocks);
            Assert.Equal(expectedBlocksCount, localizer.Blocks.Count);
        }
    }
}