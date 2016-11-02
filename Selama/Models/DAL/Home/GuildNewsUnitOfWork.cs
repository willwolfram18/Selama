using BattleNetApi.Api;
using BattleNetApi.Objects.WoW;
using Selama.Areas.Forums.Models.DAL;
using Selama.Common.Extensions;
using Selama.Common.Utility;
using Selama.Models.DAL;
using Selama.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Selama.Models.DAL.Home
{
    public class GuildNewsUnitOfWork : IDisposable
    {
        private BattleNetApiClient BattleNetApi { get; set; }
        private GuildNewsFeedRepository GuildNewsFeedRepository { get; set; }

        public GuildNewsUnitOfWork()
        {
            BattleNetApi = new BattleNetApiClient(Util.BattleNetApiClientId);

            var appDbContext = new ApplicationDbContext();
            GuildNewsFeedRepository = new GuildNewsFeedRepository(appDbContext);
        }

        public async Task<List<GuildNewsFeedViewModel>> GetPublicGuildNews(int pageNumber, int pageSize)
        {
            return GetPageItems(new List<List<GuildNewsFeedViewModel>> { await GetBattleNetNews() }, pageNumber, pageSize);
        }
                    
        public async Task<List<GuildNewsFeedViewModel>> GetMembersOnlyNews(int pageNumber, int pageSize)
        {
            var battleNetNews = GetBattleNetNews();
            List<List<GuildNewsFeedViewModel>> sources = new List<List<GuildNewsFeedViewModel>>();

            sources.Add(GetForumNews());
            sources.Add(await battleNetNews);

            return GetPageItems(sources, pageNumber, pageSize);
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

            // TODO: Convert to a "Merge"
            List<GuildNewsFeedViewModel> results = new List<GuildNewsFeedViewModel>();
            foreach (var newSource in newsSources)
            {
                results.AddRange(newSource);
            }
            results.Sort();

            return results.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }

        public void Dispose()
        {
            GuildNewsFeedRepository.Dispose();
        }
    }
}