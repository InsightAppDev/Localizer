namespace Insight.Localizer
{
    public interface ILocalizer<T> : ILocalizer where T : class
    {
        /// <summary>
        /// Get ket from context - using nameof(T) as block name
        /// </summary>
        string Get(string key);
    }
}