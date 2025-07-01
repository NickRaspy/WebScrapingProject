using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScrapingProject
{
    public class HtmlMaster(IHtmlParser parser, IHtmlParsableProvider parsableProvider) : IHtmlMaster
    {
        private readonly IHtmlParser _parser = parser;
        private readonly IHtmlParsableProvider _parsableProvider = parsableProvider;
        public bool ProcessHTML(string parsableName)
        {
            while (true)
            {
                Console.WriteLine("\nВведите путь к HTML файлу (или введите 0, чтобы вернуться назад): ");
                string? htmlPath = Console.ReadLine();
                if (htmlPath == "0") return false;
                if (string.IsNullOrEmpty(htmlPath) || !File.Exists(htmlPath) || Path.GetExtension(htmlPath) != ".html")
                {
                    Console.WriteLine("\nФайла не существует или не является HTML файлом");
                    continue;
                }

                string html = string.Empty;
                try
                {
                    html = File.ReadAllText(htmlPath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nОшибка при чтении файла: {ex.Message}");
                    continue;
                }

                var parsable = _parsableProvider.CreateParsable(parsableName);

                try
                {
                    _parser.Parse(parsable, html);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nОшибка при парсинге: {ex.Message}");
                    continue;
                }

                var dir = Path.GetDirectoryName(htmlPath) ?? string.Empty;

                if (ParsedDataToJson(parsable, dir, parsableName))
                {
                    Console.WriteLine("\nПарсинг окончен. JSON файл был сохранен в ту же папку, что и HTML");
                    Console.WriteLine("\nЖелаете продолжить? (y/n, default - n) ");
                    if (string.Equals(Console.ReadLine(), "y", StringComparison.CurrentCultureIgnoreCase))
                        continue;
                }
                else
                {
                    Console.WriteLine("\nПарсинг не удался. Попробуйте еще раз.");
                }
                break;
            }
            return true;
        }

        private bool ParsedDataToJson(IHtmlParsable parsable, string dir, string parsableName = "")
        {
            JsonSerializer serializer = new();

            string? fileName = string.IsNullOrEmpty(parsableName) ? parsable.GetType().Name : parsableName;

            string? jsonPath = Path.Combine(dir, $"{fileName}.json");
            try
            {
                using StreamWriter sw = new(jsonPath);
                using JsonWriter writer = new JsonTextWriter(sw);
                serializer.Serialize(writer, parsable);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nОшибка при сохранении JSON: {ex.Message}");
                return false;
            }
        }
    }
}
