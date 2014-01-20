using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RazorBurn.Tests
{
    [TestFixture]
    public class SimpleTests : RazorBurnTests
    {
        [Test]
        public void Empty_String_Compiles()
        {
            ListErrors(() =>
                {
                    RazorCompiler compiler = new RazorCompiler(RazorLanguage.CSharp);
                    var template = compiler.Compile<TestRazorTemplate<string>>("");
                });
        }

        [Test]
        public void Echo_Script()
        {
            ListErrors(() =>
            {
                RazorCompiler compiler = new RazorCompiler(RazorLanguage.CSharp);
                var template = compiler.Compile<TestRazorTemplate<string>>("@Model");
                Assert.AreEqual("Hello World!", template.Run("Hello World!"));
            });
        }

        [Test]
        public void Echo_Script_In_Visual_Basic()
        {
            ListErrors(() =>
            {
                RazorCompiler compiler = new RazorCompiler(RazorLanguage.VisualBasic);
                var template = compiler.Compile<TestRazorTemplate<string>>("@Model");
                Assert.AreEqual("Hello World!", template.Run("Hello World!"));
            });
        }
    }
}
