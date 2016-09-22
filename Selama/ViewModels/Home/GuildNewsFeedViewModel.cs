using BattleNetApi.Api;
using BattleNetApi.Objects.WoW;
using Selama.Common.Utility;
using Selama.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Selama.ViewModels.Home
{
    public class GuildNewsFeedViewModel : IComparable<GuildNewsFeedViewModel>
    {
        #region Class properties
        private static BattleNetApiClient _bnetApi = new BattleNetApiClient(Util.BattleNetApiClientId);
        #endregion

        #region Instance properties
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:g}")]
        public DateTime Timestamp { get; private set; }

        [AllowHtml]
        public MvcHtmlString Content { get; private set; }

        public GuildNewsFeedType Type { get; private set; }
        #endregion

        public static GuildNewsFeedViewModel BuildGuildNewsFeedItem(object newsSource)
        {
            if (newsSource == null)
            {
                throw new ArgumentNullException("newsSource");
            }

            if (newsSource is GuildNews)
            {
                return BuildFromBattleNetGuildNews(newsSource as GuildNews);
            }
            else if (newsSource is GuildNewsFeedItem)
            {
                return BuildFromForumNews(newsSource as GuildNewsFeedItem);
            }
            else
            {
                throw new NotImplementedException("Nothing implemented for type " + newsSource.GetType());
            }
        }

        #region Build Battle.net News
        private static GuildNewsFeedViewModel BuildFromBattleNetGuildNews(GuildNews battleNetNews)
        {
            GuildNewsFeedViewModel result;
            switch (battleNetNews.Type)
            {
                case BattleNetApi.Objects.WoW.Enums.GuildNewsType.ItemLoot:
                case BattleNetApi.Objects.WoW.Enums.GuildNewsType.ItemPurchase:
                case BattleNetApi.Objects.WoW.Enums.GuildNewsType.ItemCraft:
                    result = BuildGuildItemNews(battleNetNews as GuildNewsPlayerItem);
                    break;
                case BattleNetApi.Objects.WoW.Enums.GuildNewsType.PlayerAchievement:
                case BattleNetApi.Objects.WoW.Enums.GuildNewsType.GuildAchievement:
                    result = BuildGuildAchievementNews(battleNetNews as GuildNewsAchievement);
                    break;
                default:
                    result = new GuildNewsFeedViewModel { Timestamp = battleNetNews.DateTimeTimestamp, Content = new MvcHtmlString("Sample") };
                    break;
            }
            result.Type = GuildNewsFeedType.BattleNet;
            return result;
        }

        private static GuildNewsFeedViewModel BuildGuildItemNews(GuildNewsPlayerItem itemNews)
        {
            return new GuildNewsFeedViewModel
            {
                Timestamp = itemNews.DateTimeTimestamp,
                Content = new MvcHtmlString(
                    string.Format("{0} obtained {1}.", itemNews.CharacterName, ItemTag(itemNews))
                ),
            };
        }

        private static string ItemTag(GuildNewsPlayerItem itemNews)
        {
            string baseTag = "<a href = '//wowhead.com/item={0}' class='item' target='_blank' rel='{1}' >Item {0}</a>";
            string relAttribute = "";
            if (itemNews.BonusLists.Count > 0)
            {
                relAttribute = "bonus=" + string.Join(":", itemNews.BonusLists);
            }
            return string.Format(baseTag, itemNews.ItemId, relAttribute);
        }

        private static GuildNewsFeedViewModel BuildGuildAchievementNews(GuildNewsAchievement achievementNews)
        {
            string achievementEarner = DetermineAchievementEarner(achievementNews);
            return new GuildNewsFeedViewModel
            {
                Timestamp = achievementNews.DateTimeTimestamp,
                Content = new MvcHtmlString(
                    string.Format("{0} earned {1} for {2} points.", achievementEarner, AchievementTag(achievementNews), achievementNews.Achievement.Points)
                ),
            };
        }

        private static string AchievementTag(GuildNewsAchievement achievementNews)
        {
            string baseTag = "<a class='achievement' href='//wowhead.com/achievement={0}' rel='achievement={0}&who={1}&when={2}' target='_blank'>Achievement {0}</a>";
            return string.Format(baseTag, achievementNews.Achievement.Id, 
                achievementNews.CharacterName, achievementNews.BattleNetTimestamp);
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
        #endregion

        #region Build forum news
        private static GuildNewsFeedViewModel BuildFromForumNews(GuildNewsFeedItem forumNews)
        {
            return new GuildNewsFeedViewModel
            {
                Timestamp = forumNews.Timestamp,
                Content = new MvcHtmlString(forumNews.Content),
                Type = GuildNewsFeedType.Forums
            };
        }
        #endregion

        public int CompareTo(GuildNewsFeedViewModel other)
        {
            return -(Timestamp.CompareTo(other.Timestamp));
        }
    }
}