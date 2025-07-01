using HtmlAgilityPack;

namespace WebScrapingProject
{
    public interface IHtmlParser
    {
        void Parse(IHtmlParsable parsable, string html);
    }

    public interface IHtmlParsable
    {
        void ParseFromHtml(HtmlDocument doc);
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
}
