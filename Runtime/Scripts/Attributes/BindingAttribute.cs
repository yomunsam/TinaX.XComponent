using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace TinaX.XComponent
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class BindingAttribute : Attribute
    {
        public string BindingName = string.Empty;
        public bool Nullable = false;

        public BindingAttribute() { }
        public BindingAttribute(string bindingName, bool nullable = false) { this.BindingName = bindingName; this.Nullable = nullable; }

    }
}

