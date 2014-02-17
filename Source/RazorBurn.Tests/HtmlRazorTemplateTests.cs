using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RazorBurn.Tests
{
    [TestFixture]
    public class HtmlRazorTemplateTests : RazorBurnTests
    {
        [Test]
        public void Html_Is_Escaped()
        {
            ListErrors(() =>
            {
                RazorCompiler compiler = new RazorCompiler();
                var template = compiler.Compile<TestHtmlRazorTemplate<string>>("@Model");
                Assert.AreEqual("&lt;strong&gt;Hello World!&lt;/strong&gt;", template.Run("<strong>Hello World!</strong>"));
            });
        }

        [Test]
        public void Html_Is_Not_Escaped_When_Using_Raw()
        {
            ListErrors(() =>
            {
                RazorCompiler compiler = new RazorCompiler();
                var template = compiler.Compile<TestHtmlRazorTemplate<string>>("@Html.Raw(@Model)");
                Assert.AreEqual("<strong>Hello World!</strong>", template.Run("<strong>Hello World!</strong>"));
            });
        }
    }
}
