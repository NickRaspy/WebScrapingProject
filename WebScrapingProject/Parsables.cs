using HtmlAgilityPack;

namespace WebScrapingProject
{
    public class News : IHtmlParsable
    {
        public List<Data> newsData = [];

        public void ParseFromHtml(HtmlDocument doc)
        {
            var newsNodes = doc.DocumentNode.SelectNodes("//li[@class='news-item']");
            if (newsNodes == null) return;

            foreach (var newsNode in newsNodes)
            {
                //get body

                var bodyNode = newsNode.SelectSingleNode(".//div[@class='news-body']");
                if (bodyNode == null) continue;

                //get time

                var timeNode = bodyNode.SelectSingleNode(".//time");
                DateTime dateTime;

                if (timeNode != null)
                {
                    var rawDateTime = timeNode.GetAttributeValue("datetime", "");
                    if (!DateTime.TryParse(rawDateTime, out dateTime)) continue;
                }
                else
                {
                    continue;
                }

                //get title and url

                var titleNode = bodyNode.SelectSingleNode(".//h4[@class='news-title']") ??
                                bodyNode.SelectSingleNode(".//p[@class='news-title']/h4");
                if (titleNode == null) continue;

                var urlNode = titleNode.SelectSingleNode(".//a[@href]");
                if (urlNode == null) continue;

                var url = urlNode.GetAttributeValue("href", "");

                url = Utilities.CleanText(url, "\r\n");

                var title = Utilities.CleanText(urlNode.InnerText, true, "&nbsp;", "&quot;");

                if (string.IsNullOrWhiteSpace(url) || string.IsNullOrWhiteSpace(title)) continue;

                newsData.Add(new(title, url, dateTime));
            }
        }

        [Serializable]
        public struct Data(string title, string url, DateTime dateTime)
        {
            public string? Title { get; private set; } = title;
            public string? URL { get; private set; } = url;
            public DateTime DateTime { get; private set; } = dateTime;
        }
    }
}
