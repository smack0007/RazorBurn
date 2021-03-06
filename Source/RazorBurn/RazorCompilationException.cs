﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RazorBurn
{
    public class RazorCompilationException : Exception
    {
        public string Source
        {
            get;
            private set;
        }

        public string[] Errors
        {
            get;
            private set;
        }

        public RazorCompilationException(string message, string source, string[] errors)
            : base(message)
        {
            this.Source = source;
            this.Errors = errors;
        }
    }
}
