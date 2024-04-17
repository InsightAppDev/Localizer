namespace Insight.Localizer
{
    public class Localizer<T> : Localizer, ILocalizer<T> where T : class
    {
        public string Get(string key)
        {
            var block = typeof(T).Name;
            return Get(block, key);
        }
    }
}