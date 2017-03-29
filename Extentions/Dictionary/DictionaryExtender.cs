using System.Collections.Generic;

namespace CB.CSharp.Extentions
{
    public static class DictionaryExtender
    {
        public static void AddKeyIfValueIsNotNull<T>(this Dictionary<T, string> dic, T key, string Value)
        {
            if (!string.IsNullOrEmpty(Value))
                dic[key] = Value;
        }
    }
}