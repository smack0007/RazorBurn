using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RazorBurn.Tests
{
    [TestFixture]
    public class RazorCompilerTests : RazorBurnTests
    {
        [Test]
        public void Empty_String_Compiles()
        {
            ListErrors(() =>
            {
                RazorCompiler compiler = new RazorCompiler();
                var template = compiler.Compile<TestRazorTemplate<string>>("");
            });
        }

        [Test]
        public void Echo_Script_Compiles()
        {
            ListErrors(() =>
            {
                RazorCompiler compiler = new RazorCompiler();
                var template = compiler.Compile<TestRazorTemplate<string>>("@Model");
            });
        }
    }
}
