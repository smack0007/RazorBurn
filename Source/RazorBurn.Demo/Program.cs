using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RazorBurn.Demo
{
    public abstract class HelloRazorTemplate : RazorTemplate
    {
        protected string Name
        {
            get;
            private set;
        }

        public string Run(string name)
        {
            this.Name = name;

            return this.ExecuteTemplate();
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
