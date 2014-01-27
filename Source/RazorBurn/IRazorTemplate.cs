using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RazorBurn
{
    public interface IRazorTemplate
    {
    }

    public interface IRazorTemplate<T> : IRazorTemplate
    {
        string Run(T model);
    }
}
