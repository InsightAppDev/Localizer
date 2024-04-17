using System.Collections.Generic;

namespace Insight.Localizer
{
    public interface ILocalizer
    {
        /// <summary>
        /// Curent culture of the localizer.
        /// </summary>
        public static string? CurrentCulture { get; set; }

        /// <summary>
        /// Loaded blocks
        /// </summary>
        IDictionary<string, Block> Blocks { get; }

        /// <summary>
        /// Available block names
        /// </summary>
        IReadOnlyCollection<string> AvailableBlockNames { get; }

        /// <summary>
        /// Get value by block-key and injected culture
        /// </summary>
        /// <param name="block">Block name</param>
        /// <param name="key">Key</param>
        string Get(string block, string key);

        /// <summary>
        /// Get value by block-key for any culture
        /// </summary>
        /// <param name="block">Block name</param>
        /// <param name="key">Key</param>
        /// <returns></returns>
        string GetAny(string block, string key);

        /// <summary>
        /// Get value by culture-block-key for any culture
        /// </summary>
        /// <param name="culture">Culture</param>
        /// <param name="block">Block name</param>
        /// <param name="key">Key</param>
        /// <returns></returns>
        string Get(string culture, string block, string key);
    }
}