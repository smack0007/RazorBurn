using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RazorBurn.Tests
{
    [TestFixture]
    public class RazorTemplateTests : RazorBurnTests
    {        
        [Test]
        public void Html_Is_Not_Escaped()
        {
            ListErrors(() =>
            {
                RazorCompiler compiler = new RazorCompiler(RazorLanguage.CSharp);
                var template = compiler.Compile<TestRazorTemplate<string>>("@Model");
                Assert.AreEqual("<strong>Hello World!</strong>", template.Run("<strong>Hello World!</strong>"));
            });
        }
    }
}
