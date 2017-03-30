using System;

namespace CB.CSharp.Extentions
{
    internal class NotAllColumnsFoundException : Exception
    {
        public string[] UnFoundColumns { get; set; }

        public NotAllColumnsFoundException(params string[] unFoundColumns)
        {
            UnFoundColumns = unFoundColumns;
        }
    }
}