using Microsoft.Extensions.DependencyInjection;
using System;

namespace WebScrapingProject
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();

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