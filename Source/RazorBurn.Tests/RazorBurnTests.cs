using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RazorBurn.Tests
{
    public class RazorBurnTests
    {
        protected static void ListErrors(Action action)
        {
            try
            {
                action();
            }
            catch (RazorCompilationException ex)
            {
                Console.WriteLine("Errors:");

                foreach (var error in ex.Errors)
                    Console.WriteLine(error);

                throw;
            }
        }
    }
}
