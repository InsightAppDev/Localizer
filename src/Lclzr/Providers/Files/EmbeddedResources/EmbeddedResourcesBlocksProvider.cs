using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Lclzr.Exceptions;
using Newtonsoft.Json.Linq;

namespace Lclzr.Providers.Files.EmbeddedResources
{
    internal class EmbeddedResourcesBlocksProvider : BlocksProvider
    {
        private static readonly Regex OneFilePerCultureNameRegex =
            new Regex(@"lclzr.([A-z0-9_-]{1,})\.([a-z\-]{2,})\.json$", RegexOptions.Compiled);

        private static readonly Regex OneFilePerMultipleCulturesNameRegex =
            new Regex(@"lclzr.([A-z0-9_-]{1,})\.json$", RegexOptions.Compiled);

        private readonly EmbeddedResourcesBlocksProviderOptions _options;
        
        private bool _initialized;

        public EmbeddedResourcesBlocksProvider(EmbeddedResourcesBlocksProviderOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public override async Task<IReadOnlyCollection<Block>> GetBlocks()
        {
            if (!_initialized)
            {
                await Initialize();
                _initialized = true;
            }
            
            return await base.GetBlocks();
        }

        private async Task Initialize()
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
                                }
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
    }
}