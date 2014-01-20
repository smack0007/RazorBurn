using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace RazorBurn
{
    public abstract class RazorTemplate<T> : IRazorTemplate
    {
        StringBuilder output;

        protected virtual void SetModel(T model)
        {
        }

        public string Run(T model)
        {
            this.SetModel(model);
   
            this.output = new StringBuilder();

            this.Execute();
            string result = this.output.ToString();

            return result;
        }

        public abstract void Execute();

        private string ConvertToString(object value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            return value.ToString();
        }

        protected void Write(object value)
        {
            string output = null;

            if (value is IEncodableString)
            {
                output = ((IEncodableString)value).Encode();
            }
            else
            {
                output = this.ConvertToString(value);
            }

            this.output.Append(output);
        }

        protected void WriteLiteral(object value)
        {
            this.output.Append(value);
        }

        public virtual void WriteAttribute(string name, PositionTagged<string> prefix, PositionTagged<string> suffix, params AttributeValue[] values)
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
