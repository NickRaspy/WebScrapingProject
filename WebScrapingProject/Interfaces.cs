using HtmlAgilityPack;

namespace WebScrapingProject
{
    public interface IHtmlParser
    {
        T Parse<T>(string html) where T : IHtmlParsable, new();
    }

    public interface IHtmlParsable
    {
        void ParseFromHtml(HtmlDocument doc);
    }
}
