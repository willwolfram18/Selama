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
        private string _apiClientKey { get; set; }

        private string ApiUriMissingEndpoint
        {
            get
            {
                return string.Format(BaseApiUriFormat, "wow/{0}");
            }
        }

        #region Constructors
        internal WoWCommunityApi(string apiClientKey, Region region, Locale locale) : base(region, locale)
        {
            _apiClientKey = apiClientKey;
        }
        #endregion

        #region Public interface
        public async Task<Character> GetCharacterProfileAsync(string realm, string characterName, params string[] fields)
        {
            using (HttpClient httpClient = BuildHttpClient())
            {
                var response = await httpClient.GetAsync(CharacterProfileUri(characterName, realm, fields).ToString());
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                JObject characterJson = await ParseJsonResponse(response);
                // TODO: Return parsed JSON object
                return null;
            }
        }

        public async Task<IEnumerable<RaceDataResource>> GetCharacterRaces()
        {
            using (HttpClient httpClient = BuildHttpClient())
            {
                var response = await httpClient.GetAsync(DataResourceUri("character/races").ToString());
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                JObject racesJson = await ParseJsonResponse(response);
                // TODO: Return parsed JSON object
                return null;
            }
        }
        #endregion

        #region Private/Internal functions
        private HttpClient BuildHttpClient()
        {
            HttpClient httpClient = new HttpClient();
            SetJsonAcceptHeader(httpClient);
            return httpClient;
        }

        private UriBuilder CharacterProfileUri(string characterName, string realmName, params string[] fields)
        {
            string characterProfileEndPoint = string.Format("character/{0}/{1}", realmName, characterName);
            string profileUri = string.Format(ApiUriMissingEndpoint, characterProfileEndPoint);

            UriBuilder characterProfileUriBuilder = new UriBuilder(profileUri);
            var query = BuildCommonQuery();
            query["fields"] = string.Join(",", fields);
            characterProfileUriBuilder.Query = query.ToString();

            return characterProfileUriBuilder;
        }

        private UriBuilder DataResourceUri(string resourceEndPoint)
        {
            string dataResourceUri = string.Format(ApiUriMissingEndpoint, "data/" + resourceEndPoint);

            UriBuilder dataResourceUriBuilder = new UriBuilder(dataResourceUri);
            var query = BuildCommonQuery();
            dataResourceUriBuilder.Query = query.ToString();

            return dataResourceUriBuilder;
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
