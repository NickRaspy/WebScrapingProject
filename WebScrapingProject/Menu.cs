namespace WebScrapingProject
{
    public class Menu(IHtmlMaster htmlMaster, IHtmlParsableProvider parsableProvider) : IMenu
    {
        private readonly IHtmlMaster _htmlMaster = htmlMaster;
        private readonly IHtmlParsableProvider _parsableProvider = parsableProvider;

        private const string WelcomeMessage = "Добро пожаловать!\n1. Начать\n2. Выход\n";
        private const string InputPrompt = "\nВведите значение:";
        private const string NotNumberError = "\nНе числовое значение. Попробуйте еще раз.";
        private const string NoSuchOptionError = "\nТакой опции нет в списке.";
        private const string ServiceListHeader = "Доступные сервисы, по которому будете извлекать данные:\n";
        private const string ServiceProcessError = "\nОшибка при обработке выбранного сервиса.";

        public void Show()
        {
            while (true)
            {
                Welcome();
            }
        }

        private void Welcome()
        {
            Console.Clear();
            Console.WriteLine(WelcomeMessage);

            int parsedValue = ConsoleHelper.ReadInt(
                InputPrompt,
                NotNumberError
            );

            switch (parsedValue)
            {
                case 1:
                    SelectParser();
                    break;

                case 2:
                    Environment.Exit(0);
                    break;

                default:
                    Console.WriteLine(NoSuchOptionError);
                    break;
            }
        }

        private void SelectParser()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine(ServiceListHeader);

                List<string> parsablesNames = [.. _parsableProvider.GetAvailableParsables()];

                for (int i = 0; i < parsablesNames.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {parsablesNames[i]}\n");
                }
                Console.WriteLine("0. Вернуться в главное меню\n");

                int parsedValue = ConsoleHelper.ReadInt(
                    InputPrompt,
                    NotNumberError
                );

                if (parsedValue == 0)
                    return;

                if (parsedValue < 1 || parsedValue > parsablesNames.Count)
                {
                    Console.WriteLine(NoSuchOptionError);
                    continue;
                }

                bool result = _htmlMaster.ProcessHTML(parsablesNames[parsedValue - 1]);
                if (!result)
                {
                    continue;
                }
                break;
            }
        }
    }
}