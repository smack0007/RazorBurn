using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RazorBurn.Demo
{
    public abstract class HelloRazorTemplate : RazorTemplate<string>
    {
        protected string Name
        {
            get;
            private set;
        }

        protected override void SetModel(string model)
        {
            this.Name = model;
        }
    }

    class Program
    {
        public static void Main(string[] args)
        {
            var compiler = new RazorCompiler(RazorLanguage.CSharp);
            var template = compiler.Compile<HelloRazorTemplate>("Hello @Name.");
            Console.WriteLine(template.Run("Bob Freeman"));

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
