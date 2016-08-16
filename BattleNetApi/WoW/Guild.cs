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
        internal Guild(JObject jsonGuild)
        {
            
        }

        public string Name { get; private set; }

        public string Realm { get; private set; }

        public int Level { get; private set; }

        public Alliance Alliance { get; private set; }
    }
}
