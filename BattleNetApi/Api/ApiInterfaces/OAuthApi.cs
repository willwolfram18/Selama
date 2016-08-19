using BattleNetApi.Api.Enums;
using BattleNetApi.WoW;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BattleNetApi.Api
{
    public class OAuthApi : BattleNetApiInterfaceBase
    {
        public OAuthApi(Region region, Locale locale) : base(region, locale) { }

        public async Task<IEnumerable<Character>> WowProfileAsync(string accessToken)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                SetJsonAcceptHeader(httpClient);

                var response = await httpClient.GetAsync(WowOAuthProfileUri(accessToken).ToString());
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                JObject profile = await ParseJsonResponse(response);
                return ParseWowCharacterProfile(profile["characters"].AsJEnumerable());
            }
        }

        private UriBuilder WowOAuthProfileUri(string accessToken)
        {
            string oAuthProfileUri = string.Format(_baseApiUri, RegionString, "wow/user/characters");
            UriBuilder uriBuilder = new UriBuilder(oAuthProfileUri);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["access_token"] = accessToken;
            uriBuilder.Query = query.ToString();
            return uriBuilder;
        }

        private List<Character> ParseWowCharacterProfile(IJEnumerable<JToken> wowCharactersJson)
        {
            List<Character> characters = new List<Character>();
            foreach (JObject characterJson in wowCharactersJson)
            {
                characters.Add(Character.BuildOAuthProfileCharacter(characterJson));
            }
            return characters;
        }
    }
}
