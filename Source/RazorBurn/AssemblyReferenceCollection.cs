using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RazorBurn
{
    public class AssemblyReferenceCollection : ICollection<AssemblyReference>
    {
        readonly List<AssemblyReference> references;

        public int Count
        {
            get { return this.references.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public AssemblyReferenceCollection()
        {
            this.references = new List<AssemblyReference>();
        }

        public void Add(AssemblyReference item)
        {
            this.references.Add(item);
        }

        public void Add(string name)
        {
            this.Add(new AssemblyReference(name, false));
        }

        public void AddFile(string fileName)
        {
            this.Add(new AssemblyReference(fileName, true));
        }

        public void Clear()
        {
            this.references.Clear();
        }

        public bool Contains(AssemblyReference item)
        {
            return this.references.Contains(item);
        }

        public void CopyTo(AssemblyReference[] array, int arrayIndex)
        {
            this.references.CopyTo(array, arrayIndex);
        }

        public bool Remove(AssemblyReference item)
        {
            return this.references.Remove(item);
        }

        public IEnumerator<AssemblyReference> GetEnumerator()
        {
            return this.references.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.references.GetEnumerator();
        }
    }
}
