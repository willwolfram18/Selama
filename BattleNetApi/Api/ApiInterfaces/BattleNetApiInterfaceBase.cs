using BattleNetApi.Api.Enums;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BattleNetApi.Api.ApiInterfaces
{
    public class BattleNetApiInterfaceBase
    {
        protected const string _baseUriMissingRegionAndEndpoint = "https://{0}.api.battle.net/{1}";
        protected Region _region { get; set; }
        protected Locale _locale { get; set; }

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

        internal BattleNetApiInterfaceBase(Region region, Locale locale)
        {
            _region = region;
            _locale = locale;
        }

        protected void SetJsonAcceptHeader(HttpClient httpClient)
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        protected async Task<JObject> ParseJsonResponse(HttpResponseMessage response)
        {
            string jsonStr = await response.Content.ReadAsStringAsync();
            return JObject.Parse(jsonStr);
        }
    }
}
