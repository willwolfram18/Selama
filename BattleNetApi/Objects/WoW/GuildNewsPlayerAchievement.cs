using Newtonsoft.Json.Linq;

namespace BattleNetApi.Objects.WoW
{
    public class GuildNewsPlayerAchievement : GuildNews
    {
        public Achievement Achievement { get; private set; }

        internal static GuildNewsPlayerAchievement BuildPlayerAchievement(JObject playerAchievementNewsJson)
        {
            return new GuildNewsPlayerAchievement(playerAchievementNewsJson);
        }

        private GuildNewsPlayerAchievement(JObject playerAchievementNewsJson) : base(playerAchievementNewsJson)
        {
            Achievement = WoW.Achievement.BuildFullAchievement(playerAchievementNewsJson["achievement"].Value<JObject>());
        }
    }
}
