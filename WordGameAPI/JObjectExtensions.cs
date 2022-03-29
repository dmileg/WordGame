using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace WordGameAPI
{
    public static class JObjectExtensions
    {
        public static T GetOrDefault<T>(this JObject src, string name, T defaultValue)
        {
            JToken jt;
            if (src.TryGetValue(name, StringComparison.OrdinalIgnoreCase, out jt) && jt.Type != JTokenType.Null)
            {
                T rc = jt.Value<T>();
                return rc;
            }
            return defaultValue;
        }
    }
}
