using System.Web.UI;

namespace CB.CSharp.Extentions
{
    public static class ControlExtender
    {
        public static bool HasValidID(this Control Control) =>
            (Control.ID != null) && (!Control.ID.Equals("0")) && !Control.ID.Equals("null");
    }
}