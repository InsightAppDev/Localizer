using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Newtonsoft.Json.Linq;

namespace Insight.Localizer
{
    public sealed class Localizer : ILocalizer
    {
        private static IDictionary<string, Block> _blocks;

        private static readonly AsyncLocal<string?> _currentCulture = new AsyncLocal<string?>();

        public static string? CurrentCulture
        {
            get => _currentCulture.Value;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException(nameof(value));
                }

                _currentCulture.Value = value.ToLower();
            }
        }

        public IReadOnlyCollection<string> AvailableBlockNames => new Lazy<IReadOnlyCollection<string>>(
                () => Blocks
                    .Select(x => x.Key)
                    .ToList())
            .Value;

        public IDictionary<string, Block> Blocks => _blocks;

        public static void Initialize(LocalizerConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            _blocks = new Dictionary<string, Block>(StringComparer.InvariantCultureIgnoreCase);
            Build(configuration);
        }

        public string Get(string block, string key)
        {
            return this[block].Get(_currentCulture.Value, key);
        }

        public string GetAny(string block, string key)
        {
            return this[block].Get(LocalizerConstants.AnyCultureKey, key);
        }

        public string Get(string culture, string block, string key)
        {
            return this[block].Get(culture, key);
        }

        private Block this[string name] =>
            Blocks.TryGetValue(name, out var value)
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
                var localeRegex = new Regex(@"^(.{1,})\.(.{2,})\.json$");
                var filename = Path.GetFileName(file);
                var match = localeRegex.Match(filename);
                if (match.Success)
                {
                    var blockName = match.Groups[1].Value;
                    var cultureString = match.Groups[^1]?.Value;

                    var json = File.ReadAllText(file);
                    var jObject = JObject.Parse(json);
                    var blockContent = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

                    foreach (var (key, value) in jObject)
                        blockContent.Add(key, value.ToString());

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