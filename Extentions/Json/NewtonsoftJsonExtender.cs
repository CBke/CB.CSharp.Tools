using Newtonsoft.Json;
using System.Web.Mvc;

namespace CB.CSharp.Extentions
{
    public static class NewtonsoftJsonExtender
    {
        public static ActionResult ToJsonResult(this object obj, Formatting formatting = Formatting.Indented, JsonIgnoreGroupContractResolver jsonIgnoreGroupContractResolver = null) =>
            new ContentResult
            {
                ContentType = "application/json",
                Content = JsonConvert.SerializeObject(obj, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    Formatting = formatting,
                    ContractResolver = jsonIgnoreGroupContractResolver
                })
            };
    }
}