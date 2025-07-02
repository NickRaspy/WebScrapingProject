using HtmlAgilityPack;

namespace WebScrapingProject
{
    public interface IHtmlParser
    {
        void Parse(IHtmlParsable parsable, string html, Action<string>? log = null);
    }

    public interface IHtmlParsable
    {
        void ParseFromHtml(HtmlDocument doc, Action<string>? log = null);
    }

    public interface IHtmlParsableProvider
    {
        IEnumerable<string> GetAvailableParsables();

        IHtmlParsable CreateParsable(string key);
    }

    public interface IHtmlMaster
    {
        bool ProcessHTML(string parsableName);
    }

    public interface IMenu
    {
        void Show();
    }

    public interface ILogger
    {
        void LogError(string message);
    }
}