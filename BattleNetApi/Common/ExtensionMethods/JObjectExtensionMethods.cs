using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleNetApi.Common.ExtensionMethods
{
    internal static class JsonExtensionMethods
    {
        internal static bool ContainsKey(this JObject jObject, string key)
        {
            if (jObject == null)
            {
                return false;
            }
            return ((IDictionary<string, JToken>)jObject).ContainsKey(key);
        }

        internal static bool ContainsKey(this JToken jToken, string key)
        {
            if (jToken == null)
            {
                return false;
            }
            return ((IDictionary<string, JToken>)jToken).ContainsKey(key);
        }
    }
}
