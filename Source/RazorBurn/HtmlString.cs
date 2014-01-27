using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace RazorBurn
{
    public class HtmlString : IHtmlString
    {
        object value;

        public HtmlString(object value)
        {
            this.value = value;
        }

        public string ToHtmlString()
        {
            return this.value.ToString();
        }
    }
}
