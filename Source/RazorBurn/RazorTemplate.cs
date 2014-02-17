using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace RazorBurn
{
    public abstract class RazorTemplate
    {
        protected StringBuilder Output
        {
            get;
            set;
        }
        
        /// <summary>
        /// This method should not be called directly. Call on of the ExectueTemplate overloads instead.
        /// </summary>
        protected abstract void Execute();

        protected string ExecuteTemplate()
        {
            this.Output = new StringBuilder();

            this.Execute();

            return this.Output.ToString();
        }

        private string ConvertToString(object value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            return value.ToString();
        }

        protected void WriteInternal(string value)
        {
            this.Output.Append(value);
        }

        protected virtual void Write(object value)
        {
            this.WriteInternal(this.ConvertToString(value));
        }

        protected virtual void WriteLiteral(object value)
        {
            this.WriteInternal(value.ToString());
        }

        protected virtual void WriteAttribute(string name, PositionTagged<string> prefix, PositionTagged<string> suffix, params AttributeValue[] values)
        {
            bool first = true;
            bool wroteSomething = false;

            if (values.Length == 0)
            {
                // Explicitly empty attribute, so write the prefix and suffix
                this.WriteLiteral(prefix);
                this.WriteLiteral(suffix);
            }
            else
            {
                for (int i = 0; i < values.Length; i++)
                {
                    AttributeValue attrVal = values[i];
                    PositionTagged<object> val = attrVal.Value;

                    bool? boolVal = null;
                    if (val.Value is bool)
                    {
                        boolVal = (bool)val.Value;
                    }

                    if (val.Value != null && (boolVal == null || boolVal.Value))
                    {
                        string valStr = val.Value as string;
                        if (valStr == null)
                        {
                            valStr = val.Value.ToString();
                        }
                        if (boolVal != null)
                        {
                            valStr = name;
                        }

                        if (first)
                        {
                            this.WriteLiteral(prefix);
                            first = false;
                        }
                        else
                        {
                            this.WriteLiteral(attrVal.Prefix);
                        }

                        if (attrVal.Literal)
                        {
                            this.WriteLiteral(valStr);
                        }
                        else
                        {
                            this.Write(valStr); // Write value
                        }
                        wroteSomething = true;
                    }
                }

                if (wroteSomething)
                {
                    this.WriteLiteral(suffix);
                }
            }
        }


    }
}
