using Newtonsoft.Json;

namespace WebScrapingProject
{
    public class HtmlMaster(IHtmlParser parser, IHtmlParsableProvider parsableProvider, ILogger logger) : IHtmlMaster
    {
        private readonly IHtmlParser _parser = parser;
        private readonly IHtmlParsableProvider _parsableProvider = parsableProvider;
        private readonly ILogger _logger = logger;

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
                    _logger.LogError($"[ERROR] Ошибка при чтении файла: {ex.Message}");
                    continue;
                }

                var parsable = _parsableProvider.CreateParsable(parsableName);

                try
                {
                    _parser.Parse(parsable, html, _logger.LogError);
                    if (parsable is News news)
                    {
                        if (ParsedDataToJson(news.newsData, Path.GetDirectoryName(htmlPath) ?? string.Empty))
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
                    }
                    else
                    {
                        _logger.LogError("[ERROR] Неизвестный тип парсабла");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"[ERROR] Ошибка при парсинге: {ex.Message}");
                    continue;
                }
                break;
            }
            return true;
        }

        private bool ParsedDataToJson(object data, string dir)
        {
            JsonSerializer serializer = new();
            string jsonPath = Path.Combine(dir, "clean-news.json");
            try
            {
                using StreamWriter sw = new(jsonPath);
                using JsonWriter writer = new JsonTextWriter(sw);
                serializer.Serialize(writer, data);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"[ERROR] Ошибка при сохранении JSON: {ex.Message}");
                return false;
            }
        }
    }
}