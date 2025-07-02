namespace WebScrapingProject
{
    public class HtmlParsableProvider(Dictionary<string, Func<IHtmlParsable>> factories) : IHtmlParsableProvider
    {
        private readonly Dictionary<string, Func<IHtmlParsable>> _factories = factories;

        public IEnumerable<string> GetAvailableParsables() => _factories.Keys;

        public IHtmlParsable CreateParsable(string key)
        {
            if (_factories.TryGetValue(key, out var factory))
                return factory();
            throw new ArgumentException($"Парсер с ключом '{key}' не найден");
        }
    }
}