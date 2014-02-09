using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace RazorBurn
{
    public abstract class HtmlRazorTemplate : RazorTemplate
    {
        protected HtmlHelper Html
        {
            get;
            private set;
        }

        public HtmlRazorTemplate()
            : base()
        {
            this.Html = new HtmlHelper();
        }

        protected override void Write(object value)
        {
            if (value is IHtmlString)
            {
                this.WriteInternal(((IHtmlString)value).ToHtmlString());
            }
            else
            {
                this.WriteInternal(WebUtility.HtmlEncode(value.ToString()));
            }
        }
    }
}
