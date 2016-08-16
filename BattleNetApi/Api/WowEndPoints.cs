using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleNetApi.Api
{
    internal static class WowEndPoints
    {
        private const string _baseUri = "https://{0}.api.battle.net/{1}";

        public static string OAuthProfileUri(string region)
        {
            return string.Format(_baseUri, region, "wow/user/characters");
        }
    }
}
