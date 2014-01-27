using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RazorBurn
{
    public class HtmlHelper
    {
        public HtmlString Raw(object value)
        {
            return new HtmlString(value);
        }
    }
}
