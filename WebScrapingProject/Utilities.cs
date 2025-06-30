using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScrapingProject
{
    static class Utilities
    {
        public static string CleanText(string text, params string[] partsToClean)
        {
            if (string.IsNullOrEmpty(text) || partsToClean == null || partsToClean.Length == 0)
                return text;

            var sb = new StringBuilder(text);

            foreach (string part in partsToClean)
            {
                if (!string.IsNullOrEmpty(part))
                    sb.Replace(part, " ");
            }

            return sb.ToString();
        }
    }
}
