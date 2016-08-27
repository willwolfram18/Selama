using BattleNetApi.Common;
using BattleNetApi.Objects.WoW.DataResources;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleNetApi.Objects.WoW
{
    public class Item
    {
        #region Properties
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
        #endregion

        internal static Item ParseItemJson(JObject itemJson)
        {
            return new Item(itemJson);
        }

        private Item(JObject itemJson)
        {
            Id = itemJson["id"].Value<int>();
            Name = itemJson["name"].Value<string>();
            Icon = itemJson["icon"].Value<string>();
            ItemLevel = itemJson["itemLevel"].Value<int>();

            
        }

        private void ParseOptionalFields(JObject itemJson)
        {
            DisenchantingSkillRank = itemJson["disenchantingSkillRank"].Value<int>();
            Description = itemJson["description"].Value<string>();
            Stackable = itemJson["stackable"].Value<int>();
            ItemBind = itemJson["itemBind"].Value<int>();
            BuyPriceCopper = itemJson["buyPrice"].Value<double>();
            ContainerSlots = itemJson["containerSlots"].Value<int>();
            InventoryType = itemJson["inventoryType"].Value<int>();
            Equippable = itemJson["equippable"].Value<bool>();
            MaxCount = itemJson["maxCount"].Value<int>();
            MaxDurability = itemJson["maxDurability"].Value<int>();
        }
    }
}
