using HtmlAgilityPack;
using Newtonsoft.Json;
using NUnit.Framework;

namespace WebScrapingProject
{
    [TestFixture]
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

        [Test]
        public void CleanText_EmptyInput_ReturnsEmpty()
        {
            string result = Utilities.CleanText("");
            Assert.That(result, Is.EqualTo(""));
        }

        [Test]
        public void CleanText_NoPartsToClean_ReturnsOriginal()
        {
            string input = "Test string";
            string result = Utilities.CleanText(input);
            Assert.That(result, Is.EqualTo(input));
        }

        [Test]
        public void CleanText_ReplaceWithSpace()
        {
            string input = "a-b-c";
            string result = Utilities.CleanText(input, true, "-");
            Assert.That(result, Is.EqualTo("a b c"));
        }
    }

    [TestFixture]
    public class NewsTests
    {
        [Test]
        public void News_ParseFromHtml_ParsesValidNewsItems()
        {
            string html = @"<ul class='news-list'>
                <li class='news-item'>
                    <div class='news-body'>
                        <time datetime='2025-06-22'>22 June 2025</time>
                        <h4 class='news-title'><a href='/news/test'>Test News</a></h4>
                    </div>
                </li>
            </ul>";
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var news = new News();
            news.ParseFromHtml(doc);
            Assert.That(news.newsData.Count, Is.EqualTo(1));
            Assert.That(news.newsData[0].Title, Is.EqualTo("Test News"));
            Assert.That(news.newsData[0].URL, Is.EqualTo("https://brokennews.net/news/test"));
            Assert.That(news.newsData[0].DateTime, Is.EqualTo(new DateTime(2025, 6, 22)));
        }

        [Test]
        public void News_ParseFromHtml_IgnoresInvalidItems()
        {
            string html = @"<ul class='news-list'>
                <li class='news-item'>
                    <div class='news-body'>
                        <h4 class='news-title'><a href=''></a></h4>
                    </div>
                </li>
            </ul>";
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var news = new News();
            news.ParseFromHtml(doc);
            Assert.That(news.newsData, Is.Empty);
        }
    }

    [TestFixture]
    public class HtmlParsableProviderTests
    {
        [Test]
        public void GetAvailableParsables_ReturnsKeys()
        {
            var dict = new Dictionary<string, Func<IHtmlParsable>>
            {
                { "Новости", () => new News() },
                { "Test", () => new News() }
            };
            var provider = new HtmlParsableProvider(dict);
            var keys = provider.GetAvailableParsables().ToList();
            Assert.That(keys, Does.Contain("Новости"));
            Assert.That(keys, Does.Contain("Test"));
        }

        [Test]
        public void CreateParsable_ValidKey_ReturnsInstance()
        {
            var provider = new HtmlParsableProvider(new Dictionary<string, Func<IHtmlParsable>>
            {
                { "Новости", () => new News() }
            });
            var parsable = provider.CreateParsable("Новости");
            Assert.That(parsable, Is.InstanceOf<News>());
        }

        [Test]
        public void CreateParsable_InvalidKey_Throws()
        {
            var provider = new HtmlParsableProvider(new Dictionary<string, Func<IHtmlParsable>>());
            Assert.Throws<ArgumentException>(() => provider.CreateParsable("NotExist"));
        }
    }

    [TestFixture]
    public class HtmlParsingTests
    {
        [Test]
        public void Parse_CorruptedNewsHtml_EqualsExpectedJson()
        {
            string htmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "HTML", "corrupted-news.html");
            string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "HTML", "clean-news_test.json");
            Assert.That(File.Exists(htmlPath), Is.True, "No HTML file to test on");
            Assert.That(File.Exists(jsonPath), Is.True, "No JSON file to compare with");

            string htmlContent = File.ReadAllText(htmlPath);
            string jsonContent = File.ReadAllText(jsonPath);

            var doc = new HtmlDocument();
            doc.LoadHtml(htmlContent);

            var expected = JsonConvert.DeserializeObject<List<News.Data>>(jsonContent);
            Assert.That(expected, Is.Not.Null);

            var news = new News();
            news.ParseFromHtml(doc, _ => { });
            Assert.That(news.newsData, Is.Not.Null);

            Assert.That(news.newsData.Count, Is.EqualTo(expected.Count));
            for (int i = 0; i < news.newsData.Count; i++)
            {
                Assert.That(news.newsData[i].Title, Is.EqualTo(expected[i].Title));
                Assert.That(news.newsData[i].URL, Is.EqualTo(expected[i].URL));
                Assert.That(news.newsData[i].DateTime, Is.EqualTo(expected[i].DateTime));
            }
        }
    }
}