using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Lclzr.Exceptions;
using Newtonsoft.Json.Linq;

namespace Lclzr.Providers.Files.RawFiles
{
    internal class RawFilesBlocksProvider : BlocksProvider
    {
        private static readonly Regex OneFilePerCultureNameRegex =
            new Regex(@"^lclzr.([A-z0-9_-]{1,})\.([a-z\-]{2,})\.json$", RegexOptions.Compiled);

        private static readonly Regex OneFilePerMultipleCulturesNameRegex =
            new Regex(@"^lclzr.([A-z0-9_-]{1,})\.json$", RegexOptions.Compiled);

        private readonly RawFilesBlocksProviderOptions _options;

        private bool _initialied;
        
        public RawFilesBlocksProvider(RawFilesBlocksProviderOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public override async Task<IReadOnlyCollection<Block>> GetBlocks()
        {
            if (!_initialied)
            {
                await Initialize();
                _initialied = true;
            }
            
            return await base.GetBlocks();
        }

        private async Task Initialize()
        {
            var searchOption = _options.ReadNestedFolders
                ? SearchOption.AllDirectories
                : SearchOption.TopDirectoryOnly;

            var files = Directory.GetFiles(_options.Path, "*", searchOption);

            foreach (var file in files)
            {
                try
                {
                    var filename = Path.GetFileName(file);
                    string? content = null;
                    var oneFilePerMultipleLanguagesMatch = OneFilePerMultipleCulturesNameRegex.Match(filename);
                    if (oneFilePerMultipleLanguagesMatch.Success)
                    {
                        content = await File.ReadAllTextAsync(file);
                        var blockName = oneFilePerMultipleLanguagesMatch.Groups[1].Value;
                        var jObject = JObject.Parse(content);
                        foreach (var (culture, blockJToken) in jObject)
                        {
                            var info = new BlockCultureData(blockName, culture, blockJToken.ToString());
                            await InitializeBlockCulture(in info);
                        }

                        continue;
                    }

                    var oneFilePerLanguageMatch = OneFilePerCultureNameRegex.Match(filename);
                    if (oneFilePerLanguageMatch.Success)
                    {
                        content = await File.ReadAllTextAsync(file);
                        var block = oneFilePerLanguageMatch.Groups[1].Value;
                        var culture = oneFilePerLanguageMatch.Groups[2].Value;
                        var info = new BlockCultureData(block, culture, content);

                        await InitializeBlockCulture(in info);
                    }
                }
                catch (Exception ex)
                {
                    throw new LocalizerException($"Failed to process file: {file}", ex);
                }
            }
        }
    }
}