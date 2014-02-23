using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RazorBurn
{
    public class RazorTemplateFactory<T>
        where T : RazorTemplate
    {
        private readonly Assembly assembly;
        private readonly string typeName;

        internal RazorTemplateFactory(Assembly assembly, string typeName)
        {
            this.assembly = assembly;
            this.typeName = typeName;
        }

        public T Create()
        {
            return (T)this.assembly.CreateInstance("RazorBurn.CompiledTemplates.Template");
        }
    }
}
