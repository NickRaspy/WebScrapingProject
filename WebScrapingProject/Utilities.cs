using System.Text;

namespace WebScrapingProject
{
    internal static class Utilities
    {
        public static string CleanText(string text, bool spaceInsteadOfDelete = false, params string[] partsToClean)
        {
            if (string.IsNullOrEmpty(text) || partsToClean == null || partsToClean.Length == 0)
                return text;

            var sb = new StringBuilder(text);

            foreach (string part in partsToClean)
            {
                if (!string.IsNullOrEmpty(part))
                    sb.Replace(part, spaceInsteadOfDelete ? " " : "");
            }

            return sb.ToString();
        }

        public static string CleanText(string text, params string[] partsToClean)
            => CleanText(text, false, partsToClean);
    }
}