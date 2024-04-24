namespace Insight.Localizer
{
    public interface ILocalizer<T> : ILocalizer where T : class
    {
        /// <summary>
        /// Get value by key from context - using nameof(T) as block name
        /// </summary>
        string Get(string key);

        /// <summary>
        /// Get any value by key from context for any culture - using nameof(T) as block name
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns></returns>
        string GetAny(string key);
        
        /// <summary>
        /// Get value from context - using nameof(T) as block name
        /// </summary>
        string GetByCulture(string culture, string key);
    }
}