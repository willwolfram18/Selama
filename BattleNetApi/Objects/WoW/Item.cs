using BattleNetApi.Common;
using BattleNetApi.Objects.WoW.DataResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleNetApi.Objects.WoW
{
    public class Item
    {
        public int Id { get; private set; }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public int DisenchantingSkillRank { get; private set; }

        public string Icon { get; private set; }

        public int Stackable { get; private set; }

        public int ItemBind { get; private set; }

        // TODO: Include BounsStats property

        public List<ItemSpell> ItemSpells { get; private set; }

        public double BuyPriceCopper { get; private set; }

        public double BuyPriceSilver { get { return BuyPriceCopper / Util.COPPER_IN_SILVER; }  }

        public double BuyPriceGold { get { return BuyPriceSilver / Util.SILVER_IN_GOLD; } }

        public ItemClassDataResource ItemClass { get; private set; }

        public ItemSubClassDataResource ItemSubClass { get; private set; }

        public int ContainerSlots { get; private set; }

        public int InventoryType { get; private set; }

        public bool Equippable { get; private set; }

        public int ItemLevel { get; private set; }

        public int MaxCount { get; private set; }

        public int MaxDurability { get; private set; }

        // TODO: Include remaining fields
    }
}
