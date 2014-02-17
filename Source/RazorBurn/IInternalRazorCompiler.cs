using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RazorBurn
{
    internal interface IInternalRazorCompiler
    {
        AssemblyReferenceCollection ReferencedAssemblies { get; }

        T Compile<T>(string source) where T : RazorTemplate;
    }
}
