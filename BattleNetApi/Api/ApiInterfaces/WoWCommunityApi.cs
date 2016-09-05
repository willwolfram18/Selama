using BattleNetApi.Api.Enums;
using BattleNetApi.Objects.WoW;
using BattleNetApi.Objects.WoW.DataResources;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
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
        public WoWCommunityApi(string apiClientKey, Region region, Locale locale) : base(region, locale)
        {
            _apiClientKey = apiClientKey;
        }
        #endregion

        #region Public interface
        public async Task<Achievement> GetAchievementAsync(int id)
        {
            using (HttpClient httpClient = BuildHttpClient())
            {
                var response = await httpClient.GetAsync(AchievementUri(id).ToString());
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                JObject achievementJson = await ParseJsonResponseAsync(response);
                return Achievement.BuildFullAchievement(achievementJson);
            }
        }

        public async Task<Character> GetCharacterProfileAsync(string realm, string characterName, params string[] fields)
        {
            using (HttpClient httpClient = BuildHttpClient())
            {
                var response = await httpClient.GetAsync(CharacterProfileUri(realm, characterName, fields).ToString());
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                JObject characterJson = await ParseJsonResponseAsync(response);
                return Character.BuildCharacterProfileEndpoint(characterJson);
            }
        }

        public async Task<Guild> GetGuildProfileAsync(string realm, string guildName, params string[] fields)
        {
            using (HttpClient httpClient = BuildHttpClient())
            {
                var response = await httpClient.GetAsync(GuildProfileUri(realm, guildName, fields).ToString());
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                JObject guildJson = await ParseJsonResponseAsync(response);
                return Guild.BuildGuildProfileFromJson(guildJson);
            }
        }

        public async Task<Item> GetItemAsync(int id)
        {
            using (HttpClient httpClient = BuildHttpClient())
            {
                var response = await httpClient.GetAsync(ItemUri(id).ToString());
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                JObject achievementJson = await ParseJsonResponseAsync(response);
                return Item.ParseItemJson(achievementJson);
            }
        }

        public async Task<IEnumerable<RaceDataResource>> GetCharacterRacesAsync()
        {
            using (HttpClient httpClient = BuildHttpClient())
            {
                var response = await httpClient.GetAsync(DataResourceUri("character/races").ToString());
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                JObject racesJson = await ParseJsonResponseAsync(response);
                return RaceDataResource.BuildRacesList(racesJson);
            }
        }

        public async Task<IEnumerable<ItemClassDataResource>> GetItemClassesAsync()
        {
            using (HttpClient httpClient = BuildHttpClient())
            {
                var response = await httpClient.GetAsync(DataResourceUri("item/classes").ToString());
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                JObject itemClassesJson = await ParseJsonResponseAsync(response);
                return ItemClassDataResource.BuildItemClassListFromJson(itemClassesJson);
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

        private UriBuilder AchievementUri(int id)
        {
            UriBuilder achievementUriBuilder = BuildUriWithEndpoint("achievement/" + id.ToString());
            achievementUriBuilder.Query = BuildCommonQuery().ToString();
            return achievementUriBuilder;
        }

        private UriBuilder ItemUri(int id)
        {
            UriBuilder itemUriBuilder = BuildUriWithEndpoint("item/" + id.ToString());
            itemUriBuilder.Query = BuildCommonQuery().ToString();
            return itemUriBuilder;
        }

        private UriBuilder CharacterProfileUri(string realmName, string characterName, params string[] fields)
        {
            UriBuilder characterProfileUriBuilder = BuildUriWithEndpoint(string.Format("character/{0}/{1}", realmName, characterName));
            var query = BuildCommonQuery();
            if (fields.Length > 0)
            {
                query["fields"] = string.Join(",", fields);
            }
            characterProfileUriBuilder.Query = query.ToString();

            return characterProfileUriBuilder;
        }

        private UriBuilder GuildProfileUri(string realm, string guildName, params string[] fields)
        {
            UriBuilder guildProfileUriBuilder = BuildUriWithEndpoint(string.Format("guild/{0}/{1}", realm, guildName));
            var query = BuildCommonQuery();
            if (fields.Length > 0)
            {
                query["fields"] = string.Join(",", fields);
            }
            guildProfileUriBuilder.Query = query.ToString();

            return guildProfileUriBuilder;
        }

        private UriBuilder DataResourceUri(string resourceEndPoint)
        {
            UriBuilder dataResourceUriBuilder = BuildUriWithEndpoint("data/" + resourceEndPoint);
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
        private UriBuilder BuildUriWithEndpoint(string endpoint)
        {
            return new UriBuilder(string.Format(ApiUriMissingEndpoint, endpoint));
        }
        #endregion
    }
}
