using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RazorBurn
{
    internal class RazorCompilerVisualBasic : IInternalRazorCompiler
    {
        public AssemblyReferenceCollection ReferencedAssemblies
        {
            get;
            private set;
        }

        public RazorCompilerVisualBasic()
        {
            this.ReferencedAssemblies = new AssemblyReferenceCollection();
        }

        public T Compile<T>(string source) where T : RazorTemplate
        {
            throw new NotImplementedException();
        }
    }
}
