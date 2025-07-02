namespace WebScrapingProject
{
    public static class ConsoleHelper
    {
        public static int ReadInt(string prompt, string errorMessage)
        {
            while (true)
            {
                Console.WriteLine(prompt);
                string? input = Console.ReadLine();
                if (int.TryParse(input, out int value))
                    return value;
                Console.WriteLine(errorMessage);
            }
        }

        public static string ReadNonEmptyString(string prompt, string errorMessage)
        {
            while (true)
            {
                Console.WriteLine(prompt);
                string? input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                    return input;
                Console.WriteLine(errorMessage);
            }
        }

        public static bool Confirm(string prompt)
        {
            Console.WriteLine($"{prompt} (y/n, default - n): ");
            string? input = Console.ReadLine();
            return string.Equals(input, "y", StringComparison.OrdinalIgnoreCase);
        }
    }
}