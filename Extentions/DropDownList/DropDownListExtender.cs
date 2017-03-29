using System.Linq;
using System.Web.UI.WebControls;

namespace CB.CSharp.Extentions
{
    public static class DropDownListExtender
    {
        public static bool HasItemWithText(this DropDownList DropDownList, string value)=>
            DropDownList
            .Items
            .Cast<ListItem>()
            .Any(x => x.Text.Equals(value));
    }
}
