using System.Collections.Generic;
using System.Web.UI;

namespace CB.CSharp.Extentions
{
    public static class ControlCollectionExtender
    {
        public static List<T> GetControlList<T>(this ControlCollection ControlCollection, bool HasValidID = true) where T : Control
        {
            var ResultCollection = new List<T>();
            ControlCollection.GetControlsAndAddToList<T>(ResultCollection, HasValidID);
            return ResultCollection;
        }

        public static void GetControlsAndAddToList<T>(this ControlCollection ControlCollection, List<T> ResultCollection, bool HasValidID = true) where T : Control
        {
            foreach (Control Control in ControlCollection)
            {
                if ((Control is T) && (!HasValidID || Control.HasValidID()))
                    ResultCollection
                        .Add((T)Control);

                if (Control.HasControls())
                    Control
                        .Controls
                        .GetControlsAndAddToList(ResultCollection, HasValidID);
            }
        }
    }
}