using BattleNetApi.Objects.WoW.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleNetApi.Objects.WoW
{
    public class Spell
    {
        public int Id { get; private set; }

        public string Name { get; private set; }

        public string Icon { get; private set; }

        public string Description { get; private set; }

        public string Range { get; private set; }

        public SpellCastingType CastingType { get; private set; }

        public string CastTime { get; private set; }

        public string CoolDown { get; private set; }

        public string PowerCost { get; private set; }
    }
}
