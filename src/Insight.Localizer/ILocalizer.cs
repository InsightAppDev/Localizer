using System.Collections.Generic;

namespace Insight.Localizer
{
    public interface ILocalizer
    {
        IDictionary<string, Block> Blocks { get; }

        IReadOnlyCollection<string> AvailableBlockNames { get; }

        string Get(string block, string key);
        
        string Get(string culture, string block, string key);
    }
}