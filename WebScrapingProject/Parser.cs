using HtmlAgilityPack;

namespace WebScrapingProject
{
    public class Parser : IHtmlParser
    {
        public T Parse<T>(string html) where T : IHtmlParsable, new()
        {
            HtmlDocument doc = new();
            doc.LoadHtml(html);

            var parsable = new T();
            parsable.ParseFromHtml(doc);

            return parsable;
        }
    }
}
