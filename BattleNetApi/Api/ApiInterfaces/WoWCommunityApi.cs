using BattleNetApi.Api.Enums;
using BattleNetApi.Objects.WoW;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BattleNetApi.Api.ApiInterfaces
{
    public class WoWCommunityApi : BattleNetApiInterfaceBase
    {
        private string _apiSecretKey { get; set; }
        private string _apiClientKey { get; set; }

        #region Constructors
        internal WoWCommunityApi(string apiSecretKey, string apiClientKey, Region region, Locale locale) : base(region, locale)
        {
            _apiSecretKey = apiSecretKey;
            _apiClientKey = apiClientKey;
        }
        #endregion

        #region Public interface
        public async Task<Character> CharacterProfileAsync(string realm, string characterName)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                SetJsonAcceptHeader(httpClient);

                var response = await httpClient.GetAsync(CharacterProfileUri(characterName, realm, _apiClientKey).ToString());
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                JObject characterJson = await ParseJsonResponse(response);
                // TODO: Return parsed JSON object
                return null;
            }
        }
        #endregion

        #region Private/Internal functions
        internal UriBuilder CharacterProfileUri(string characterName, string realmName, params string[] fields)
        {
            string characterProfileEndPoint = string.Format("/wow/character/{0}/{1}", realmName, characterName);
            string profileUri = string.Format(_baseUriMissingRegionAndEndpoint, RegionString, characterProfileEndPoint);

            UriBuilder uriBuilder = new UriBuilder(profileUri);
            var query = BuildCommonQuery();
            query["fields"] = string.Join(",", fields);
            uriBuilder.Query = query.ToString();

            return uriBuilder;
        }

        private NameValueCollection BuildCommonQuery()
        {
            var query = HttpUtility.ParseQueryString("");
            query["locale"] = LocaleString;
            query["apikey"] = _apiClientKey;
            return query;
        }
        #endregion
    }
}
