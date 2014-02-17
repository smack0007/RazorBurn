using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RazorBurn
{
    public class AssemblyReference
    {
        public string Name
        {
            get;
            private set;
        }

        public bool IsFile
        {
            get;
            private set;
        }

        public AssemblyReference(string name, bool isFile)
        {
            this.Name = name;
            this.IsFile = isFile;
        }
    }
}
