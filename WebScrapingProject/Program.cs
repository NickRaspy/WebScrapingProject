using Microsoft.Extensions.DependencyInjection;

namespace WebScrapingProject
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var services = new ServiceCollection();

            services.AddSingleton<ILogger, FileLogger>();
            services.AddSingleton<IHtmlParser, Parser>();
            services.AddSingleton(new Dictionary<string, Func<IHtmlParsable>>
                {
                    { "Новости", () => new News() }
                });
            services.AddSingleton<IHtmlParsableProvider, HtmlParsableProvider>();
            services.AddSingleton<IHtmlMaster, HtmlMaster>();
            services.AddSingleton<IMenu, Menu>();

            using var serviceProvider = services.BuildServiceProvider();
            serviceProvider.GetService<IMenu>()?.Show();
        }
    }
}