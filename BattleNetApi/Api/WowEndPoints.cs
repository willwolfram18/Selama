using BattleNetApi.Api.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BattleNetApi.Api
{
    internal class WowEndPoints
    {
        private const string _baseApiUri = "https://{0}.api.battle.net/{1}";
        private Region _region;

        private string RegionString
        {
            get
            {
                return _region.ToString().ToLower();
            }
        }

        internal WowEndPoints(Region region)
        {
            _region = region;
        }

        public UriBuilder OAuthProfileUri(string accessToken)
        {
            string profileUri = string.Format(_baseApiUri, RegionString, "wow/user/characters");
            UriBuilder builder = new UriBuilder(profileUri);
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["access_token"] = accessToken;
            builder.Query = query.ToString();
            return builder;
        }
    }
}
