using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Linq;
using System.Reflection;

namespace CB.CSharp.Extentions
{
    public class JsonIgnoreGroupContractResolver : DefaultContractResolver
    {
        public string Group { get; set; }

        public JsonIgnoreGroupContractResolver(string group)
        {
            Group = group;
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            var attributes = (JsonIgnoreGroupAttribute[])System.Attribute.GetCustomAttributes(member, typeof(JsonIgnoreGroupAttribute), true);

            if (!attributes.Any())
                return property;

            var blocked = attributes.Any(x => x.Groups.Contains(Group));

            property.Ignored = blocked;
            property.ShouldSerialize = propInstance => !blocked;

            return property;
        }
    }
}