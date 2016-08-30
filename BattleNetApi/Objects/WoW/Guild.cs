using BattleNetApi.Common;
using BattleNetApi.Common.ExtensionMethods;
using BattleNetApi.Objects.WoW.Enums;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace BattleNetApi.Objects.WoW
{
    public class Guild
    {
        #region Properties
        public string Name { get; private set; }

        public string Realm { get; private set; }

        public int Level { get; private set; }

        public Faction Faction { get; private set; }

        public int AchievementPoints { get; private set; }

        public GuildEmblem Emblem { get; private set; }

        public List<Character> Members { get; private set; }
        #endregion

        #region Static factory functions
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
        #endregion

        #region Private constructors
        private Guild(string name, string realm, Faction faction)
        {
            Name = name;
            Realm = realm;
            Faction = faction;
        }

        private Guild(JObject guildJson)
        {
            Name = guildJson["name"].Value<string>();
            Realm = guildJson["realm"].Value<string>();
            Level = guildJson["level"].Value<int>();
            Faction = Util.ParseEnum<Faction>(guildJson, "side");
            AchievementPoints = guildJson["achievementPoints"].Value<int>();
            Emblem = new GuildEmblem(guildJson["emblem"].Value<JObject>());

            ParseCharacters(guildJson);
        }
        #endregion

        #region Private methods
        private void ParseCharacters(JObject guildJson)
        {
            if (guildJson.ContainsKey("members"))
            {
                Members = new List<Character>();
                foreach (JToken member in guildJson["members"].AsJEnumerable())
                {
                    Members.Add(GuildMember.BuildGuildMemberFromJson(
                        member["character"].Value<JObject>(), 
                        member["rank"].Value<int>(),
                        this
                    ));
                }
            }
        }
        #endregion
    }
}
