using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RazorBurn.Tests
{
    [TestFixture]
    public class RazorTemplateFactoryTests : RazorBurnTests
    {
        [Test]
        public void Create_Returns_Instance_Which_Can_Be_Run()
        {
            ListErrors(() =>
            {
                RazorCompiler compiler = new RazorCompiler();
                var factory = compiler.CompileFactory<TestRazorTemplate<string>>("@Model");
                var template = factory.Create();
                Assert.AreEqual("<strong>Hello World!</strong>", template.Run("<strong>Hello World!</strong>"));
            });
        }

        [Test]
        public void Create_Returns_New_Instances_Each_Time()
        {
            ListErrors(() =>
            {
                RazorCompiler compiler = new RazorCompiler();
                var factory = compiler.CompileFactory<TestRazorTemplate<string>>("@Model");
                var template1 = factory.Create();
                var template2 = factory.Create();
                Assert.AreNotSame(template1, template2);
            });
        }
    }
}
