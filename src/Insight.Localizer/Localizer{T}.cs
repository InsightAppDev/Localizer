namespace Insight.Localizer
{
    public class Localizer<T> : Localizer, ILocalizer<T> where T : class
    {
        public string Get(string key)
        {
            var block = GetBlockName();
            return Get(block, key);
        }

        public string GetAny(string key)
        {
            var block = GetBlockName();
            return GetAny(block, key);
        }

        public string GetByCulture(string culture, string key)
        {
            var block = GetBlockName();
            return GetByCulture(culture, block, key);
        }

        private static string GetBlockName()
        {
            return typeof(T).Name;
        }
    }
}