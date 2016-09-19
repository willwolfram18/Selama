using BattleNetApi.Api;
using BattleNetApi.Objects.WoW;
using Selama.Common.Utility;
using System;
using System.Threading.Tasks;

namespace Selama.ViewModels.Home
{
    public class GuildNewsFeedViewModel : IComparable<GuildNewsFeedViewModel>
    {
        private static BattleNetApiClient _bnetApi = new BattleNetApiClient(Util.BattleNetApiClientId);

        public DateTime Timestamp { get; private set; }

        public string Content { get; private set; }

        public async static Task<GuildNewsFeedViewModel> BuildFromBattleNetGuildNews(GuildNews battleNetNews)
        {
            switch (battleNetNews.Type)
            {
                case BattleNetApi.Objects.WoW.Enums.GuildNewsType.ItemLoot:
                case BattleNetApi.Objects.WoW.Enums.GuildNewsType.ItemPurchase:
                case BattleNetApi.Objects.WoW.Enums.GuildNewsType.ItemCraft:
                    return await BuildGuildItemNews(battleNetNews as GuildNewsPlayerItem);
                case BattleNetApi.Objects.WoW.Enums.GuildNewsType.PlayerAchievement:
                case BattleNetApi.Objects.WoW.Enums.GuildNewsType.GuildAchievement:
                    return BuildGuildAchievementNews(battleNetNews as GuildNewsAchievement);
                default:
                    return new GuildNewsFeedViewModel { Timestamp = battleNetNews.Timestamp, Content = "Sample" };
                    throw new NotImplementedException(string.Format("No action defined for GuildNewsType of {0}", battleNetNews.Type.ToString()));
            }
        }

        private async static Task<GuildNewsFeedViewModel> BuildGuildItemNews(GuildNewsPlayerItem itemNews)
        {
            var itemGetTask = _bnetApi.WowCommunityApi.GetItemAsync(itemNews.ItemId);
            string itemAcquisitionAction = DetermineItemNewsAction(itemNews.Type);

            var item = await itemGetTask;

            return new GuildNewsFeedViewModel
            {
                Timestamp = itemNews.Timestamp,
                Content = string.Format("{0} {1} {2}.", itemNews.CharacterName, itemAcquisitionAction,item.Name),
            };
        }

        private static string DetermineItemNewsAction(BattleNetApi.Objects.WoW.Enums.GuildNewsType guildNewsType)
        {
            switch (guildNewsType)
            {
                case BattleNetApi.Objects.WoW.Enums.GuildNewsType.ItemLoot:
                    return "looted";
                case BattleNetApi.Objects.WoW.Enums.GuildNewsType.ItemPurchase:
                    return "purchased";
                case BattleNetApi.Objects.WoW.Enums.GuildNewsType.ItemCraft:
                    return "crafted";
                default:
                    throw new NotImplementedException(string.Format("No action defined for GuildNewsType of {0}", guildNewsType.ToString()));
            }
        }

        private static GuildNewsFeedViewModel BuildGuildAchievementNews(GuildNewsAchievement achievementNews)
        {
            string achievementEarner = DetermineAchievementEarner(achievementNews);
            return new GuildNewsFeedViewModel
            {
                Timestamp = achievementNews.Timestamp,
                Content = string.Format("{0} earned {1}.", achievementEarner, achievementNews.Achievement.Title),
            };
        }

        private static string DetermineAchievementEarner(GuildNewsAchievement achievementNews)
        {
            switch (achievementNews.Type)
            {
                case BattleNetApi.Objects.WoW.Enums.GuildNewsType.PlayerAchievement:
                    return achievementNews.CharacterName;
                case BattleNetApi.Objects.WoW.Enums.GuildNewsType.GuildAchievement:
                    return Util.WowGuildName;
                default:
                    throw new NotImplementedException(string.Format("No case implemented for guild news type {0}", achievementNews.Type.ToString()));
            }
        }

        public int CompareTo(GuildNewsFeedViewModel other)
        {
            return -(Timestamp.CompareTo(other.Timestamp));
        }
    }
}