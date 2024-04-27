using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Lclzr.Exceptions;
using Lclzr.Infrastructure;
using Newtonsoft.Json.Linq;

namespace Lclzr.Providers.Files.EmbeddedResources
{
    internal class EmbeddedResourcesBlocksProvider : BlocksProvider, IInitializable
    {
        private static readonly Regex OneFilePerCultureNameRegex =
            new Regex(@"lclzr.([A-z0-9_-]{1,})\.([a-z\-]{2,})\.json$", RegexOptions.Compiled);

        private static readonly Regex OneFilePerMultipleCulturesNameRegex =
            new Regex(@"lclzr.([A-z0-9_-]{1,})\.json$", RegexOptions.Compiled);

        private readonly EmbeddedResourcesBlocksProviderOptions _options;

        public EmbeddedResourcesBlocksProvider(EmbeddedResourcesBlocksProviderOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task Initialize()
        {
            var encoding = Encoding.GetEncoding(_options.ResourceEncodingWebName);
            foreach (var assembly in _options.Assemblies.Select(Assembly.Load))
            {
                var resourceNames = assembly.GetManifestResourceNames();

                foreach (var resourceName in resourceNames)
                {
                    try
                    {
                        using (var resourceStream = assembly.GetManifestResourceStream(resourceName))
                        {
                            if (resourceStream == null)
                                // TODO: Log
                                continue;

                            using (var streamReader = new StreamReader(resourceStream, encoding))
                            {
                                var content = await streamReader.ReadToEndAsync();

                                var oneFilePerCultureMatch = OneFilePerCultureNameRegex.Match(resourceName);
                                if (oneFilePerCultureMatch.Success)
                                {
                                    var block = oneFilePerCultureMatch.Groups[1].Value;
                                    var culture = oneFilePerCultureMatch.Groups[2].Value;
                                    var info = new BlockCultureData(block, culture, content);

                                    await InitializeBlockCulture(in info);
                                    // TODO: Write log

                                    continue;
                                }


                                var oneFilePerMultipleCulturesMatch =
                                    OneFilePerMultipleCulturesNameRegex.Match(resourceName);
                                if (oneFilePerMultipleCulturesMatch.Success)
                                {
                                    var block = oneFilePerMultipleCulturesMatch.Groups[1].Value;
                                    var json = content;
                                    var jObject = JObject.Parse(json);
                                    foreach (var (culture, blockJToken) in jObject)
                                    {
                                        var info = new BlockCultureData(block, culture, blockJToken.ToString());
                                        await InitializeBlockCulture(in info);
                                    }

                                    continue;
                                }
                                
                                // TODO: Log
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new LocalizerException($"Failed to process embedded resource: {resourceName}", ex);
                    }
                }
            }
        }

        public override IReadOnlyCollection<Block> GetBlocks()
        {
            return Blocks.Values.ToArray();
        }
    }
}