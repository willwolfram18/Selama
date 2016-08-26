using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleNetApi.Objects.WoW
{
    public class ItemTooltipParams
    {
        public List<int> GemIds { get; private set; }

        public int TransmogItemId { get; private set; }

        public ItemUpgradeInfo Upgrade { get; private set; }
    }
}
