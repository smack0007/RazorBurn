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
                RazorCompiler compiler = new RazorCompiler(RazorLanguage.CSharp);
                var template = compiler.Compile<TestRazorTemplate<string>>("");
            });
        }

        [Test]
        public void CSharp_Echo_Script_Compiles()
        {
            ListErrors(() =>
            {
                RazorCompiler compiler = new RazorCompiler(RazorLanguage.CSharp);
                var template = compiler.Compile<TestRazorTemplate<string>>("@Model");
            });
        }

        [Test]
        public void VisualBasic_Echo_Script_Compiles()
        {
            ListErrors(() =>
            {
                RazorCompiler compiler = new RazorCompiler(RazorLanguage.VisualBasic);
                var template = compiler.Compile<TestRazorTemplate<string>>("@Model");
            });
        }
    }
}
