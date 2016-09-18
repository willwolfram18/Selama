using Newtonsoft.Json.Linq;

namespace BattleNetApi.Objects.WoW
{
    public class GuildNewsAchievement : GuildNews
    {
        public Achievement Achievement { get; private set; }

        internal static GuildNewsAchievement BuildPlayerAchievement(JObject playerAchievementNewsJson)
        {
            return new GuildNewsAchievement(playerAchievementNewsJson);
        }

        private GuildNewsAchievement(JObject playerAchievementNewsJson) : base(playerAchievementNewsJson)
        {
            Achievement = WoW.Achievement.BuildFullAchievement(playerAchievementNewsJson["achievement"].Value<JObject>());
        }
    }
}
