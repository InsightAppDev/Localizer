using System.Text;

namespace Lclzr.Providers.Files.EmbeddedResources
{
    public class EmbeddedResourcesBlocksProviderOptions
    {
        public string ResourceEncodingWebName { get; set; } = Encoding.UTF8.WebName;

        public string[] Assemblies { get; set; }
    }
}