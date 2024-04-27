using System.Collections.Generic;

namespace Lclzr.Registries
{
    public interface ILocalizerRegistry
    {
        /// <summary>
        /// Loaded blocks
        /// </summary>
        IReadOnlyDictionary<string, Block> Blocks { get; }

        /// <summary>
        /// Available block names
        /// </summary>
        IReadOnlyCollection<string> AvailableBlockNames { get; }

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
        string GetByCulture(string culture, string block, string key);
    }
}