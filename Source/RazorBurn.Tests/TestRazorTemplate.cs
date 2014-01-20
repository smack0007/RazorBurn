﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RazorBurn.Tests
{
    public abstract class TestRazorTemplate<T> : RazorTemplate<T>
    {
        protected T Model
        {
            get;
            private set;
        }
        
        protected override void SetModel(T model)
        {
            this.Model = model;
        }
    }
}
