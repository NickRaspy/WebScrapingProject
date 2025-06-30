using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScrapingProject
{
    public class UtilitiesTests
    {
        [Test]
        public void CleanText_RemoveParts()
        {
            string expectedResult = "Economic Crisis 2025";
            string text = "Economic&nbsp;Crisis&quot;2025";

            text = Utilities.CleanText(text, "&nbsp;", "&quot;");

            Assert.That(expectedResult, Is.EqualTo(text));
        }
    }
}
