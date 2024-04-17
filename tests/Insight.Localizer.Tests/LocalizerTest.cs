using System;
using System.Linq;
using Xunit;

namespace Insight.Localizer.Tests
{
    public sealed class GenericLocalizerTests
    {
        private readonly Localizer<GenericLocalizerTests> _localizer;

        private static LocalizerOptions Options => new LocalizerOptions
        {
            Path = "Resources",
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

    public sealed class LocalizerTest
    {
        private static LocalizerOptions Options => new LocalizerOptions
        {
            Path = "Resources",
            ReadNestedFolders = true
        };

        public LocalizerTest()
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

            var en = localizer.Get("en-us", "test", "Hello");
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