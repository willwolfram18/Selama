using BattleNetApi.WoW.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleNetApi.WoW
{
    public class Guild
    {
        public string Name { get; set; }

        public string Realm { get; set; }

        public int Level { get; set; }

        public Alliance Alliance { get; set; }
    }
}
