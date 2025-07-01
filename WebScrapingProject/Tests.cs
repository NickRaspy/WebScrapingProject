using HtmlAgilityPack;
using Moq;
using NUnit.Framework;

namespace WebScrapingProject
{
    public class UtilitiesTests
    {
        [Test]
        public void CleanText_RemovesHtmlEntities()
        {
            string expected = "Economic Crisis 2025";
            string input = "Economic&nbsp;Crisis&quot;2025";
            string result = Utilities.CleanText(input, true, "&nbsp;", "&quot;");
            Assert.That(result, Is.EqualTo(expected));
        }
    }

    [TestFixture]
    public class HTMLTests
    {
        private string htmlFilePath = string.Empty;

        private string content = string.Empty;

        [SetUp]
        public void SetUp()
        {
            htmlFilePath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "HTML", "corrupted-news.html"
            );

            Assert.That(File.Exists(htmlFilePath), Is.True, "No HTML file to test on");

            content = File.ReadAllText(htmlFilePath);
            Assert.That(!string.IsNullOrEmpty(content), "HTML content is empty");
        }


        [Test]
        public void HAP_ReturnsClassAttributes()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(content);

            var nodes = doc.DocumentNode.SelectNodes("//li[@class='news-item']");
            Assert.That(nodes, Is.Not.Null.And.Not.Empty, "HTML without required class (news-item)");

            var attributes = nodes[0].ChildNodes[1].ChildAttributes("class");
            Assert.That(attributes, Is.Not.Null.And.Not.Empty, "No attributes in this class");
        }

        [Test]
        public void Parsables_TryParse()
        {
/*            var parser = new Parser();

            var parsed = parser.Parse<News>(content);

            Assert.That(parsed.newsData.Count, Is.Not.Zero, "There is nothing here");*/
        }
    }
}
