using BattleNetApi.Api;
using BattleNetApi.Objects.WoW;
using Selama.Common.Utility;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Selama.ViewModels.Home
{
    public class GuildNewsFeedViewModel : IComparable<GuildNewsFeedViewModel>
    {
        private static BattleNetApiClient _bnetApi = new BattleNetApiClient(Util.BattleNetApiClientId);

        public DateTime Timestamp { get; private set; }

        [AllowHtml]
        public MvcHtmlString Content { get; private set; }

        public static GuildNewsFeedViewModel BuildFromBattleNetGuildNews(GuildNews battleNetNews)
        {
            switch (battleNetNews.Type)
            {
                case BattleNetApi.Objects.WoW.Enums.GuildNewsType.ItemLoot:
                case BattleNetApi.Objects.WoW.Enums.GuildNewsType.ItemPurchase:
                case BattleNetApi.Objects.WoW.Enums.GuildNewsType.ItemCraft:
                    return BuildGuildItemNews(battleNetNews as GuildNewsPlayerItem);
                case BattleNetApi.Objects.WoW.Enums.GuildNewsType.PlayerAchievement:
                case BattleNetApi.Objects.WoW.Enums.GuildNewsType.GuildAchievement:
                    return BuildGuildAchievementNews(battleNetNews as GuildNewsAchievement);
                default:
                    return new GuildNewsFeedViewModel { Timestamp = battleNetNews.Timestamp, Content = new MvcHtmlString("Sample") };
                    throw new NotImplementedException(string.Format("No action defined for GuildNewsType of {0}", battleNetNews.Type.ToString()));
            }
        }

        private static GuildNewsFeedViewModel BuildGuildItemNews(GuildNewsPlayerItem itemNews)
        {
            string itemAcquisitionAction = DetermineItemNewsAction(itemNews.Type);

            return new GuildNewsFeedViewModel
            {
                Timestamp = itemNews.Timestamp,
                Content = new MvcHtmlString(
                    string.Format("{0} {1} <a href='#' class='item' rel='item={2}'></a>.", itemNews.CharacterName, itemAcquisitionAction, itemNews.ItemId)
                ),
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
                Content = new MvcHtmlString(
                    string.Format("{0} earned {1}.", achievementEarner, achievementNews.Achievement.Title)
                ),
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