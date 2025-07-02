using HtmlAgilityPack;

namespace WebScrapingProject
{
    public class News : IHtmlParsable
    {
        public List<Data> newsData = [];

        public void ParseFromHtml(HtmlDocument doc, Action<string>? log = null)
        {
            newsData.Clear();
            var newsNodes = doc.DocumentNode.SelectNodes("//li[@class='news-item']");
            if (newsNodes == null)
            {
                log?.Invoke("[ERROR] Не найдено ни одной новости (news-item)");
                return;
            }

            foreach (var newsNode in newsNodes)
            {
                var bodyNode = newsNode.SelectSingleNode(".//div[@class='news-body']");
                if (bodyNode == null)
                {
                    log?.Invoke("[SKIP] Пропущен блок: нет news-body");
                    continue;
                }
                var timeNode = bodyNode.SelectSingleNode(".//time");
                DateTime dateTime;
                if (timeNode != null)
                {
                    var rawDateTime = timeNode.GetAttributeValue("datetime", "");
                    if (!DateTime.TryParse(rawDateTime, out dateTime))
                    {
                        log?.Invoke("[SKIP] Пропущен блок: некорректная дата");
                        continue;
                    }
                }
                else
                {
                    log?.Invoke("[SKIP] Пропущен блок: нет даты");
                    continue;
                }
                var titleNode = bodyNode.SelectSingleNode(".//h4[@class='news-title']") ??
                                bodyNode.SelectSingleNode(".//p[@class='news-title']/h4");
                if (titleNode == null)
                {
                    log?.Invoke("[SKIP] Пропущен блок: нет заголовка");
                    continue;
                }
                var urlNode = titleNode.SelectSingleNode(".//a[@href]");
                if (urlNode == null)
                {
                    log?.Invoke("[SKIP] Пропущен блок: нет ссылки");
                    continue;
                }
                var url = urlNode.GetAttributeValue("href", "");
                url = Utilities.CleanText(url, "\r\n");
                if (!string.IsNullOrWhiteSpace(url) && !url.StartsWith("http"))
                {
                    url = "https://brokennews.net" + url;
                }
                var title = Utilities.CleanText(urlNode.InnerText, true, "&nbsp;", "&quot;");
                if (string.IsNullOrWhiteSpace(url) || string.IsNullOrWhiteSpace(title))
                {
                    log?.Invoke("[SKIP] Пропущен блок: пустой url или заголовок");
                    continue;
                }
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