using BattleNetApi.Common;
using BattleNetApi.WoW.Enums;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleNetApi.WoW
{
    public class Guild
    {
        internal static Guild BuildOAuthCharacterGuild(JObject characterProfile)
        {
            return new Guild(
                characterProfile["guild"].Value<string>(),
                characterProfile["guildRealm"].Value<string>(),
                Util.SelectFactionFromRace(Util.ParseEnum<Race>(characterProfile, "race"))
            );
        }

        private Guild(string name, string realm, Faction faction)
        {
            Name = name;
            Realm = realm;
            Faction = faction;
        }

        private Guild(JObject jsonGuild)
        {
            
        }

        public string Name { get; private set; }

        public string Realm { get; private set; }

        public int Level { get; private set; }

        public Faction Faction { get; private set; }
    }
}
