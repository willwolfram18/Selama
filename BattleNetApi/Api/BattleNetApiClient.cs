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
using BattleNetApi.Api.ApiInterfaces;

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

        public OAuthApi OAuthApi { get; private set; }

        public WoWCommunityApi WowCommunityApi { get; private set; }

        public BattleNetApiClient(string apiSecretKey, string apiClientKey, Region region = Region.US, Locale locale = Locale.en_US)
        {
            _apiSecretKey = apiSecretKey;
            _apiClientKey = apiClientKey;
            _region = region;
            _locale = locale;

            InitializeApiInterfaces();
        }

        private void InitializeApiInterfaces()
        {
            OAuthApi = new OAuthApi(_region, _locale);
            WowCommunityApi = new WoWCommunityApi(_apiSecretKey, _apiClientKey, _region, _locale);
        }

        
        
    }
}
