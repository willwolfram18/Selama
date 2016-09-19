using BattleNetApi.Api;
using BattleNetApi.Objects.WoW;
using Selama.Common.Utility;
using System;
using System.Threading.Tasks;

namespace Selama.ViewModels.Home
{
    public class GuildNewsViewModel : IComparable<GuildNewsViewModel>
    {
        private static BattleNetApiClient _bnetApi = new BattleNetApiClient(Util.BattleNetApiClientId);

        public DateTime Timestamp { get; private set; }

        public string Content { get; private set; }

        public static GuildNewsViewModel BuildModelFromBattleNetGuildNews(GuildNews battleNetNews)
        {
            switch (battleNetNews.Type)
            {
                case BattleNetApi.Objects.WoW.Enums.GuildNewsType.ItemLoot:
                case BattleNetApi.Objects.WoW.Enums.GuildNewsType.ItemPurchase:
                case BattleNetApi.Objects.WoW.Enums.GuildNewsType.ItemCraft:
                    return BuildGuildItemNews(battleNetNews as GuildNewsPlayerItem);
                case BattleNetApi.Objects.WoW.Enums.GuildNewsType.PlayerAchievement:
                case BattleNetApi.Objects.WoW.Enums.GuildNewsType.GuildAchievement:
                    return null;
                default:
                    throw new NotImplementedException(string.Format("No action defined for GuildNewsType of {0}", battleNetNews.Type.ToString()));
            }
        }

        private static GuildNewsViewModel BuildGuildItemNews(GuildNewsPlayerItem battleNetNews)
        {
            Task<Item> itemTask = _bnetApi.WowCommunityApi.GetItemAsync(battleNetNews.ItemId);
            string itemAcquisitionAction = DetermineItemNewsAction(battleNetNews.Type);
            
            itemTask.Wait(); // need to wait for item response before building message

            return new GuildNewsViewModel
            {
                Timestamp = battleNetNews.Timestamp,
                Content = string.Format("{0} {1} {2}.", battleNetNews.CharacterName, itemAcquisitionAction, itemTask.Result.Name),
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
                    throw new NotImplementedException(string.Format("No action defined for GuildNewsType of {0}", battleNetNews.Type.ToString()));
            }
        }

        public int CompareTo(GuildNewsViewModel other)
        {
            return Timestamp.CompareTo(other.Timestamp);
        }
    }
}