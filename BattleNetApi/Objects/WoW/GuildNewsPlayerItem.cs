using Newtonsoft.Json.Linq;

namespace BattleNetApi.Objects.WoW
{
    public class GuildNewsPlayerItem : GuildNews
    {
        public int ItemId { get; private set; }

        internal static GuildNewsPlayerItem BuildPlayerItemNews(JObject playerItemNewsJson)
        {
            return new GuildNewsPlayerItem(playerItemNewsJson);
        }

        private GuildNewsPlayerItem(JObject playerItemNewsJson) : base(playerItemNewsJson)
        {
            ItemId = playerItemNewsJson["itemId"].Value<int>();
        }
    }
}
