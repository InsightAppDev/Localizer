namespace Insight.Localizer
{
    public sealed class LocalizerOptions
    {
        public const string DefaultResourceEndWith = ".lclzr.json";

        public string Path { get; set; }

        public bool ReadNestedFolders { get; set; }
        
        public bool OneLanguageInFile { get; set; } = true;

        public bool ReadFromFiles { get; set; } = true;

        /// <summary>
        /// File pattern. Example 'message' will read all files started with 'message.' like 'message.en-us.json', etc. If null reads all files in directory
        /// </summary>
        public string FileEndsWith { get; set; } = DefaultResourceEndWith;

        public bool ReadFromEmbedded { get; set; } = false;

        public string EmbeddedEndWith { get; set; } = DefaultResourceEndWith;
    }
}