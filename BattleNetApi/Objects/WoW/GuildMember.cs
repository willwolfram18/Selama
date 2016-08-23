using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleNetApi.Objects.WoW
{
    public class GuildMember : Character
    {
        internal static GuildMember BuildGuildMemberFromJson(JObject guildMember, int rank, Guild characterGuild)
        {
            return new GuildMember(guildMember, rank) { Guild = characterGuild };
        }

        protected GuildMember(JObject guildMember, int rank) : base(guildMember)
        {
            Rank = rank;
        }

        public int Rank { get; private set; }
    }
}
