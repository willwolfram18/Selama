using BattleNetApi.Api.Enums;
using BattleNetApi.WoW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json.Linq;

namespace BattleNetApi.Api
{
    public class BattleNetApiClient
    {
        private string _apiSecretKey { get; set; }
        private string _apiClientKey { get; set; }
        private Region _region { get; set; }
        private Locale _locale { get; set; }
        private WowEndPoints _endPoints;

        protected string RegionString
        {
            get
            {
                return _region.ToString().ToLower();
            }
        }
        protected string LocaleString
        {
            get
            {
                return _locale.ToString();
            }
        }

        public BattleNetApiClient(string apiSecretKey, string apiClientKey, Region region = Region.US, Locale locale = Locale.en_US)
        {
            _apiSecretKey = apiSecretKey;
            _apiClientKey = apiClientKey;
            _region = region;
            _locale = locale;
            _endPoints = new WowEndPoints(_region);
        }

        public async Task<IEnumerable<Character>> WowProfileAsync(string accessToken)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                SetJsonAcceptHeader(httpClient);

                var response = await httpClient.GetAsync(_endPoints.OAuthProfileUri(accessToken).ToString());
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                string jsonStr = await response.Content.ReadAsStringAsync();
                JObject profile = JObject.Parse(jsonStr);
                return ParseWowCharacterProfile(profile["characters"].AsJEnumerable());
            }
        }

        private void SetJsonAcceptHeader(HttpClient httpClient)
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        private List<Character> ParseWowCharacterProfile(IJEnumerable<JToken> wowCharactersJson)
        {
            List<Character> characters = new List<Character>();
            foreach (JObject characterJson in wowCharactersJson)
            {
                characters.Add(new Character(characterJson));
            }
            return characters;
        }
    }
}
