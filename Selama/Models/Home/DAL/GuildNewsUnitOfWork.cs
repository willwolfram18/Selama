using BattleNetApi.Api;
using BattleNetApi.Objects.WoW;
using Selama.Areas.Forums.Models.DAL;
using Selama.Common.ExtensionMethods;
using Selama.Common.Utility;
using Selama.Models.DAL;
using Selama.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Selama.Models.Home.DAL
{
    public class GuildNewsUnitOfWork: IDisposable
    {
        private BattleNetApiClient BattleNetApi { get; set; }
        private GuildNewsFeedRepository GuildNewsFeedRepository { get; set; }

        public GuildNewsUnitOfWork()
        {
            BattleNetApi = new BattleNetApiClient(Util.BattleNetApiClientId);

            var appDbContext = new ApplicationDbContext();
            GuildNewsFeedRepository = new GuildNewsFeedRepository(appDbContext);
        }

        public async Task<List<GuildNewsFeedViewModel>> GetGuildNews(int pageNumber, int pageSize)
        {
            var battleNetNews = GetBattleNetNews();

            List<GuildNewsFeedViewModel> result = new List<GuildNewsFeedViewModel>();

            result.AddRange(GetForumNews());
            result.AddRange(await battleNetNews);
            result.Sort();
            
            return result.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }

        private async Task<List<GuildNewsFeedViewModel>> GetBattleNetNews()
        {
            Guild guildProfile = await BattleNetApi.WowCommunityApi.GetGuildProfileAsync(Util.WowRealmName, Util.WowGuildName, "news");

            var result = guildProfile.News.ToListOfDifferentType(GuildNewsFeedViewModel.BuildGuildNewsFeedItem);
            result.Sort();
            return result;
        }

        private List<GuildNewsFeedViewModel> GetForumNews()
        {
            var newsItems = GuildNewsFeedRepository.Get(orderBy: (news) => news.OrderByDescending(n => n.Timestamp));
            return newsItems.ToListOfDifferentType(GuildNewsFeedViewModel.BuildGuildNewsFeedItem);
        }

        private List<GuildNewsFeedViewModel> GetPageItems(List<List<GuildNewsFeedViewModel>> newsSources, int pageNumber, int pageSize)
        {
            if (pageNumber < 0 || newsSources == null || newsSources.Count == 0)
            {
                return new List<GuildNewsFeedViewModel>();
            }

            return null;
        }

        public void Dispose()
        {
            GuildNewsFeedRepository.Dispose();
        }
    }
}