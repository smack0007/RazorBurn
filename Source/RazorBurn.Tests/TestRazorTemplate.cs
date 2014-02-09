using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RazorBurn.Tests
{
    public abstract class TestRazorTemplate<T> : RazorTemplate
    {
        protected T Model
        {
            get;
            private set;
        }

        public string Run(T model)
        {
            this.Model = model;

            return this.ExecuteTemplate();
        }
    }
}
