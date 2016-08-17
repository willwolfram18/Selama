using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleNetApi.Common.ExtensionMethods
{
    internal static class JObjectExtensionMethods
    {
        internal static bool ContainsKey(this JObject jObject, string key)
        {
            if (jObject == null)
            {
                return false;
            }
            return ((IDictionary<string, JToken>)jObject).ContainsKey(key);
        }
    }
}
