using BattleNetApi.Objects.WoW.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleNetApi.Objects.WoW
{
    public class Achievement
    {
        public int Id { get; private set; }

        public string Title { get; private set; }

        public int Points { get; private set; }

        public string Description { get; private set; }

        //TODO: Include Reward Items array

        public string Icon { get; private set; }

        public List<Criterion> Criteria { get; private set; }

        public bool AccountWide { get; private set; }

        public Faction Faction { get; private set; }
    }
}
