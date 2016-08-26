using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleNetApi.Objects.WoW
{
    public class ItemUpgradeInfo
    {
        public int Current { get; private set; }

        public int Total { get; private set; }

        public int ItemLevelIncrement { get; private set; }
    }
}
