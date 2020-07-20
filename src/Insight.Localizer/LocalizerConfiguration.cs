namespace Insight.Localizer
{
    public sealed class LocalizerConfiguration
    {
        public string Path { get; set; }

        /// <summary>
        /// File pattern. Example 'message' will read all files started with 'message.' like 'message.en-us.json', etc. If null reads all files in directory
        /// </summary>
        public string Pattern { get; set; }

        public bool ReadNestedFolders { get; set; }
    }
}