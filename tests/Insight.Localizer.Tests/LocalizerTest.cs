using System;
using System.Linq;
using Xunit;

namespace Insight.Localizer.Tests
{
    public sealed class LocalizerTest
    {
        private static LocalizerConfiguration Configuration => new LocalizerConfiguration
        {
            Path = "Resources",
            ReadNestedFolders = true
        };

        public LocalizerTest()
        {
            Localizer.Initialize(Configuration);
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
            AssertLocalizer(localizer, 3);
        }

        [Fact]
        public void Should_get_available_block_names_from_all_Files()
        {
            var localizer = new Localizer();
            AssertLocalizer(localizer, 3);

            var names = localizer.AvailableBlockNames;
            Assert.NotNull(names);
            Assert.NotEmpty(names);
            Assert.Equal(3, names.Count);
            Assert.NotNull(names.FirstOrDefault(x => x.Equals("test", StringComparison.InvariantCultureIgnoreCase)));
            Assert.NotNull(names.FirstOrDefault(x =>
                x.Equals("messages", StringComparison.InvariantCultureIgnoreCase)));
            Assert.NotNull(names.FirstOrDefault(x =>
                x.Equals("language", StringComparison.InvariantCultureIgnoreCase)));
        }

        [Fact]
        public void Should_read_files_by_pattern()
        {
            var config = Configuration;
            config.Pattern = "test";
            Localizer.Initialize(config);
            var localizer = new Localizer();
            AssertLocalizer(localizer, 1);
        }

        [Fact]
        public void Should_get_value_in_all_languages()
        {
            var localizer = new Localizer();
            AssertLocalizer(localizer, 3);

            var en = localizer.Get("en-us", "test", "Hello");
            var ru = localizer.Get("test", "Hello");

            Assert.Equal("Hi", en, StringComparer.InvariantCultureIgnoreCase);
            Assert.Equal("Привет", ru, StringComparer.InvariantCultureIgnoreCase);
        }

        [Fact]
        public void Should_get_any_value()
        {
            var localizer = new Localizer();
            AssertLocalizer(localizer, 3);

            var russianLanguage = localizer.GetAny("language", "Russian");
            var englishLanguage = localizer.GetAny("language", "English");

            Assert.Equal("Русский", russianLanguage, StringComparer.InvariantCultureIgnoreCase);
            Assert.Equal("English", englishLanguage, StringComparer.InvariantCultureIgnoreCase);
        }

        [Fact]
        public void Should_change_current_culture()
        {
            var localizer = new Localizer();
            AssertLocalizer(localizer, 3);

            Assert.Equal("ru-ru", Localizer.CurrentCulture, StringComparer.InvariantCultureIgnoreCase);
            Localizer.CurrentCulture = "en-us";
            Assert.Equal("en-us", Localizer.CurrentCulture, StringComparer.InvariantCultureIgnoreCase);
        }

        [Fact]
        public void Should_throw_ANE_on_set_culture_if_culture_is_null()
        {
            var localizer = new Localizer();
            AssertLocalizer(localizer, 3);

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