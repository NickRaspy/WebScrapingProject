using HtmlAgilityPack;

namespace WebScrapingProject
{
    public class News : IHtmlParsable
    {
        public string? Title { get; set; }
        public string? URL { get; set; }
        public DateTime DateTime { get; set; }

        public void ParseFromHtml(HtmlDocument doc)
        {
            var newsNodes = doc.DocumentNode.SelectNodes("//news-item");

            foreach (var newsNode in newsNodes)
            {

            }
        }
    }
}
