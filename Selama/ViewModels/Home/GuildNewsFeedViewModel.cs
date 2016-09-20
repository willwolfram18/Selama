using BattleNetApi.Api;
using BattleNetApi.Objects.WoW;
using Selama.Common.Utility;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Selama.ViewModels.Home
{
    public class GuildNewsFeedViewModel : IComparable<GuildNewsFeedViewModel>
    {
        private static BattleNetApiClient _bnetApi = new BattleNetApiClient(Util.BattleNetApiClientId);

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:g}")]
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
            return new GuildNewsFeedViewModel
            {
                Timestamp = itemNews.Timestamp,
                Content = new MvcHtmlString(
                    string.Format("{0} obtained {1}.", itemNews.CharacterName, ItemTag(itemNews))
                ),
            };
        }

        private static string ItemTag(GuildNewsPlayerItem itemNews)
        {
            string baseTag = "<a href = '//wowhead.com/item={0}' class='item' target='_blank' rel='item={1}'>Item {0}</a>";
            string relAttribute = "item=" + itemNews.ItemId.ToString();
            relAttribute += "&" + string.Join(":", itemNews.BonusLists);
            return string.Format(baseTag, itemNews.ItemId, relAttribute);
        }

        private static GuildNewsFeedViewModel BuildGuildAchievementNews(GuildNewsAchievement achievementNews)
        {
            string achievementEarner = DetermineAchievementEarner(achievementNews);
            return new GuildNewsFeedViewModel
            {
                Timestamp = achievementNews.Timestamp,
                Content = new MvcHtmlString(
                    string.Format("{0} earned {1} for {2} points.", achievementEarner, AchievementTag(achievementNews), achievementNews.Achievement.Points)
                ),
            };
        }

        private static string AchievementTag(GuildNewsAchievement achievementNews)
        {
            string baseTag = "<a class='achievement' href='//wowhead.com/achievement={0}' rel='achievement={0}&who={1}&when={2}' target='_blank'>Achievement {0}</a>";
            return string.Format(baseTag, achievementNews.Achievement.Id, 
                achievementNews.CharacterName, ConverDateTimeToUnixMilliseconds(achievementNews.Timestamp));
        }

        private static long ConverDateTimeToUnixMilliseconds(DateTime timestamp)
        {
            return timestamp.AddMilliseconds(-(new DateTime(1970, 1, 1).Millisecond)).Millisecond;
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