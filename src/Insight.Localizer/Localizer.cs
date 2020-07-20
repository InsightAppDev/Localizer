using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace Insight.Localizer
{
    public sealed class Localizer : ILocalizer
    {
        private static IDictionary<string, Block> _blocks;

        private readonly ICurrentCulture _currentCulture;

        public IReadOnlyCollection<string> AvailableBlockNames => Blocks
            .Select(x => x.Key)
            .ToList();

        public IDictionary<string, Block> Blocks => _blocks;

        public static void Initialize(LocalizerConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            _blocks = new Dictionary<string, Block>();
            Build(configuration);
        }

        /// <summary>
        /// Ctor with CurrentCulture
        /// </summary>
        /// <param name="currentCulture"><see cref="ICurrentCulture"/></param>
        public Localizer(ICurrentCulture currentCulture)
        {
            if (currentCulture == null)
                throw new ArgumentNullException(nameof(currentCulture));

            _currentCulture = currentCulture;
        }

        public string Get(string block, string key)
        {
            return this[block.ToLower()].Get(_currentCulture.Value.ToLower(), key.ToLower());
        }

        public string Get(string culture, string block, string key)
        {
            return this[block.ToLower()].Get(culture.ToLower(), key.ToLower());
        }

        private Block this[string name] =>
            Blocks.TryGetValue(name.ToLower(), out var value)
                ? value
                : throw new MissingBlockException($"Block `{name}` missing");

        private static void Build(LocalizerConfiguration configuration)
        {
            var pattern = string.IsNullOrWhiteSpace(configuration.Pattern)
                ? "*.json"
                : $"{configuration.Pattern}.*.json";
            var searchOption = configuration.ReadNestedFolders
                ? SearchOption.AllDirectories
                : SearchOption.TopDirectoryOnly;

            var files = Directory.GetFiles(configuration.Path, pattern, searchOption);
            foreach (var file in files)
            {
                var localeRegex = new Regex("^(.*).([A-Za-z]{2}-[A-Za-z]{2}).json$");
                var filename = Path.GetFileName(file);
                var match = localeRegex.Match(filename);
                if (match.Success)
                {
                    var blockName = match.Groups[1].Value.ToLower();
                    var cultureString = match.Groups[^1]?.Value.ToLower();

                    if (string.IsNullOrWhiteSpace(blockName))
                        throw new ArgumentNullException(nameof(blockName));

                    if (string.IsNullOrWhiteSpace(cultureString))
                        throw new ArgumentNullException(nameof(cultureString));

                    var json = File.ReadAllText(file);
                    var jObject = JObject.Parse(json);
                    var blockContent = new Dictionary<string, string>();

                    foreach (var (key, value) in jObject)
                        blockContent.Add(key.ToLower(), value.ToString());

                    if (!_blocks.ContainsKey(blockName))
                    {
                        var block = new Block(blockName);
                        _blocks.Add(block.Name, block);
                    }

                    _blocks[blockName].Add(cultureString, blockContent);
                }
            }
        }
    }
}