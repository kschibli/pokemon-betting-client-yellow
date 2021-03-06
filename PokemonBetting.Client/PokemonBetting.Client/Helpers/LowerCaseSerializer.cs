﻿using System.Reflection;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace PokemonBetting.Client.Helpers
{
    public static class LowerCaseSerializer
    {
        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            ContractResolver = new LowercaseContractResolver()
        };

        public static string SerializeObject(object o)
        {
            return JsonConvert.SerializeObject(o, Formatting.Indented, Settings);
        }

        public static T DeserializeObject<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, Settings);
        }

        public class LowercaseContractResolver : DefaultContractResolver
        {
            protected override string ResolvePropertyName(string propertyName)
            {
                var underscoreSeparatedName = Regex.Replace(propertyName, "([a-z](?=[A-Z])|[A-Z](?=[A-Z][a-z]))", "$1_");

                return underscoreSeparatedName.ToLower();
            }

            protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
            {
                var prop = base.CreateProperty(member, memberSerialization);

                if (prop.Writable) return prop;

                var property = member as PropertyInfo;
                if (property == null) return prop;

                var hasPrivateSetter = property.SetMethod != null;
                prop.Writable = hasPrivateSetter;

                return prop;
            }
        }
    }
}
