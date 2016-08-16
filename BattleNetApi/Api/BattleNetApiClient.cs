using BattleNetApi.Api.Enums;
using BattleNetApi.WoW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BattleNetApi.Api
{
    public class BattleNetApiClient
    {
        private string _apiSecretKey { get; set; }
        private string _apiClientKey { get; set; }
        private Region _region { get; set; }
        private Locale _locale { get; set; }

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
        }

        public async Task<IEnumerable<Character>> WowProfileAsync(string accessToken)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                UriBuilder builder = new UriBuilder(WowEndPoints.OAuthProfileUri(RegionString));
                var query = HttpUtility.ParseQueryString(builder.Query);
                query["access_token"] = accessToken;
                builder.Query = query.ToString();

                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                var response = await httpClient.GetAsync(builder.ToString());
                if (response.IsSuccessStatusCode)
                {
                    
                    // TODO: parse response into Json object
                    // response.Content.ReadAsStringAsync()
                }
            }
            return null;
        }
    }
}
