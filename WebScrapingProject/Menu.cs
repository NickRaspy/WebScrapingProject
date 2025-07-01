using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScrapingProject
{
    public class Menu(IHtmlMaster htmlMaster, IHtmlParsableProvider parsableProvider) : IMenu
    {
        private readonly IHtmlMaster _htmlMaster = htmlMaster;
        private readonly IHtmlParsableProvider _parsableProvider = parsableProvider;

        public void Show()
        {
            Welcome();
        }

        private void Welcome()
        {
            Console.Clear();

            Console.WriteLine("Добро пожаловать!\n1. Начать\n2. Выход\n");

            while (true)
            {
                Console.WriteLine("\nВведите значение: ");
                string? nonParsedValue = Console.ReadLine();

                if (!int.TryParse(nonParsedValue, out int parsedValue))
                {
                    Console.WriteLine("\nНе числовое значение. Попробуйте еще раз.");
                    continue;
                }

                switch (parsedValue)
                {
                    case 1:
                        SelectParser();
                        break;
                    case 2:
                        return;
                    default:
                        Console.WriteLine("\nТакой опции нет в списке.");
                        continue;
                }

                break;
            }
        }

        private void SelectParser()
        {
            Console.Clear();
            Console.WriteLine("Доступные сервисы, по которому будете извлекать данные:\n");

            List<string> parsablesNames = [.. _parsableProvider.GetAvailableParsables()];

            for(int i = 0; i < parsablesNames.Count; i++)
            {
                Console.WriteLine($"{i+1}. {parsablesNames[i]}\n");
            }

            while (true)
            {
                Console.WriteLine("\nВведите значение: ");
                string? nonParsedValue = Console.ReadLine();

                if (!int.TryParse(nonParsedValue, out int parsedValue))
                {
                    Console.WriteLine("\nНе числовое значение. Попробуйте еще раз.");
                    continue;
                }

                try
                {
                    _htmlMaster.ProcessHTML(parsablesNames[parsedValue-1]);
                    break;
                }
                catch
                {
                    Console.WriteLine("\nТакой опции нет в списке.");
                    continue;
                }
            }
        }
    }
}
