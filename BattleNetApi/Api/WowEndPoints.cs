using BattleNetApi.Api.Enums;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
        private Locale _locale;

        private string RegionString
        {
            get
            {
                return _region.ToString().ToLower();
            }
        }
        private string LocaleString
        {
            get
            {
                return _locale.ToString();
            }
        }

        internal WowEndPoints(Region region, Locale locale)
        {
            _region = region;
            _locale = locale;
        }

        internal UriBuilder CharacterProfile(string characterName, string realmName, string apiKey, params string[] fields)
        {
            string characterProfileEndPoint = string.Format("/wow/character/{0}/{1}", realmName, characterName);
            string profileUri = string.Format(_baseApiUri, RegionString, characterProfileEndPoint);

            UriBuilder uriBuilder = new UriBuilder(profileUri);
            var query = BaseQuery();
            query["fields"] = string.Join(",", fields);
            query["apikey"] = apiKey;
            uriBuilder.Query = query.ToString();

            return uriBuilder;
        }

        private NameValueCollection BaseQuery()
        {
            var query = HttpUtility.ParseQueryString("");
            query["locale"] = LocaleString;
            return query;
        }
    }
}
