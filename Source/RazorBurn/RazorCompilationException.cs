using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RazorBurn
{
    public class RazorCompilationException : Exception
    {
        public string[] Errors
        {
            get;
            private set;
        }

        public RazorCompilationException(string message, string[] errors)
            : base(message)
        {
            this.Errors = errors;
        }
    }
}
