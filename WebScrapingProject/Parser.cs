using HtmlAgilityPack;

namespace WebScrapingProject
{
    public class Parser : IHtmlParser
    {
        public void Parse(IHtmlParsable parsable, string html)
        {
            HtmlDocument doc = new();
            doc.LoadHtml(html);
            parsable.ParseFromHtml(doc);
        }
    }
}
