using BattleNetApi.Objects.WoW.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleNetApi.Objects.WoW
{
    public class Stats
    {
        public int Health { get; private set; }

        public PowerType PowerType { get; private set; }

        public int Power { get; private set; }
    }
}
