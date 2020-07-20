using System;
using System.Linq;
using Xunit;

namespace Insight.Localizer.Tests
{
    public sealed class LocalizerTest
    {
        private readonly ICurrentCulture _culture = new CurrentCulture("ru-ru");

        private static LocalizerConfiguration Configuration => new LocalizerConfiguration
        {
            Path = "Resources"
        };

        public LocalizerTest()
        {
            Localizer.Initialize(Configuration);
        }

        [Fact]
        public void CtorShouldThrowEx()
        {
            Assert.Throws<ArgumentNullException>(() => new Localizer(null));
        }

        [Fact]
        public void ShouldReadAllFiles()
        {
            var localizer = new Localizer(_culture);
            Assert.NotNull(localizer);
            Assert.NotEmpty(localizer.Blocks);
            Assert.Equal(2, localizer.Blocks.Count);
        }

        [Fact]
        public void ShouldGetAvailableBlockNamesFromAllFiles()
        {
            var localizer = new Localizer(_culture);
            Assert.NotNull(localizer);
            Assert.NotEmpty(localizer.Blocks);
            Assert.Equal(2, localizer.Blocks.Count);

            var names = localizer.AvailableBlockNames;
            Assert.NotNull(names);
            Assert.NotEmpty(names);
            Assert.Equal(2, names.Count);
            Assert.NotNull(names.FirstOrDefault(x => x.Equals("test")));
            Assert.NotNull(names.FirstOrDefault(x => x.Equals("messages")));
        }

        [Fact]
        public void ShouldReadFilesByPattern()
        {
            var config = Configuration;
            config.Pattern = "test";
            Localizer.Initialize(config);
            var localizer = new Localizer(_culture);
            Assert.NotNull(localizer);
            Assert.NotEmpty(localizer.Blocks);
            Assert.Single(localizer.Blocks);
        }

        [Fact]
        public void ShouldGetValueInAllLanguages()
        {
            var localizer = new Localizer(_culture);
            Assert.NotNull(localizer);
            Assert.NotEmpty(localizer.Blocks);
            Assert.Equal(2, localizer.Blocks.Count);

            var en = localizer.Get("en-us", "test", "Hello");
            Assert.Equal("Hi", en);

            var ru = localizer.Get("test", "Hello");
            Assert.Equal("Привет", ru);
        }
    }
}