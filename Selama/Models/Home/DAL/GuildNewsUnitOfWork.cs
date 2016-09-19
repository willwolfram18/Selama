using BattleNetApi.Api;
using BattleNetApi.Objects.WoW;
using Selama.Areas.Forums.Models.DAL;
using Selama.Common.ExtensionMethods;
using Selama.Common.Utility;
using Selama.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Selama.Models.Home.DAL
{
    public class GuildNewsUnitOfWork: IDisposable
    {
        private BattleNetApiClient _battleNetClient { get; set; }
        private ForumsUnitOfWork _forumsDb { get; set; }

        public GuildNewsUnitOfWork()
        {
            _battleNetClient = new BattleNetApiClient(Util.BattleNetApiClientId);
            _forumsDb = new ForumsUnitOfWork();
        }

        public async Task<List<GuildNewsFeedViewModel>> GetGuildNews()
        {
            var battleNetNews = GetBattleNetNews();
            

            List<GuildNewsFeedViewModel> result = new List<GuildNewsFeedViewModel>();
            
            result.AddRange(await battleNetNews);

            result.Sort();
            return result;
        }

        private async Task<List<GuildNewsFeedViewModel>> GetBattleNetNews()
        {
            Guild guildProfile = await _battleNetClient.WowCommunityApi.GetGuildProfileAsync(Util.WowRealmName, Util.WowGuildName, "news");

            return await guildProfile.News.Skip(0).Take(25).ToListOfDifferentType(GuildNewsFeedViewModel.BuildFromBattleNetGuildNews);
        }



        public void Dispose()
        {
            _forumsDb.Dispose();
        }
    }
}