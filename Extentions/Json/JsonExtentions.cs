﻿using System;

namespace CB.CSharp.Extentions
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class JsonIgnoreGroupAttribute : Attribute
    {
        public string[] Groups { get; set; }

        public JsonIgnoreGroupAttribute(params string[] groups)
        {
            Groups = groups;
        }
    }
}