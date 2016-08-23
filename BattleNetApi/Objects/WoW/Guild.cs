using BattleNetApi.Common;
using BattleNetApi.Objects.WoW.Enums;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleNetApi.Objects.WoW
{
    public class Guild
    {
        internal static Guild BuildOAuthCharacterGuild(JObject characterProfile)
        {
            return new Guild(
                characterProfile["guild"].Value<string>(),
                characterProfile["guildRealm"].Value<string>(),
                Util.SelectFactionFromRace(Util.ParseEnum<Enums.Race>(characterProfile, "race"))
            );
        }

        internal static Guild BuildGuildProfileFromJson(JObject guildProfile)
        {
            return new Guild(guildProfile);
        }

        private Guild(string name, string realm, Faction faction)
        {
            Name = name;
            Realm = realm;
            Faction = faction;
        }

        private Guild(JObject jsonGuild)
        {
            Name = jsonGuild["name"].Value<string>();
            Realm = jsonGuild["realm"].Value<string>();
            Level = jsonGuild["level"].Value<int>();
            Faction = Util.ParseEnum<Faction>(jsonGuild, "side");
            AchievementPoints = jsonGuild["achievementPoints"].Value<int>();
        }

        #region Properties
        public string Name { get; private set; }

        public string Realm { get; private set; }

        public int Level { get; private set; }

        public Faction Faction { get; private set; }

        public int AchievementPoints { get; private set; }
        #endregion
    }
}
