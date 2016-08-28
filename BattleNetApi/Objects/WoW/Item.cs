using BattleNetApi.Common;
using BattleNetApi.Common.ExtensionMethods;
using BattleNetApi.Objects.WoW.DataResources;
using BattleNetApi.Objects.WoW.Enums;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
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

        public ItemQuality Quality { get; private set; }

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

        public ItemTooltipParams TooltipParams { get; private set; }

        public int Armor { get; private set; }

        public string Context { get; private set; }
        #endregion

        internal static Item ParseItemJson(JObject itemJson)
        {
            return new Item(itemJson);
        }

        private Item(JObject itemJson)
        {
            ParseConsistentFields(itemJson);

            ParseOptionalFields(itemJson);
        }

        private void ParseConsistentFields(JObject itemJson)
        {
            Id = itemJson["id"].Value<int>();
            Name = itemJson["name"].Value<string>();
            Icon = itemJson["icon"].Value<string>();
            ItemLevel = itemJson["itemLevel"].Value<int>();
            Quality = Util.ParseEnum<ItemQuality>(itemJson, "quality");
            Armor = itemJson["armor"].Value<int>();
            Context = itemJson["context"].Value<string>();
        }

        private void ParseOptionalFields(JObject itemJson)
        {
            AssignOptionalPrimitiveField(i => i.DisenchantingSkillRank, itemJson, "disenchantingSkillRank");
            AssignOptionalPrimitiveField(i => i.Description, itemJson, "description");
            AssignOptionalPrimitiveField(i => i.Stackable, itemJson, "stackable");
            AssignOptionalPrimitiveField(i => i.ItemBind, itemJson, "itemBind");
            AssignOptionalPrimitiveField(i => i.BuyPriceCopper, itemJson, "buyPrice");
            AssignOptionalPrimitiveField(i => i.ContainerSlots, itemJson, "containerSlots");
            AssignOptionalPrimitiveField(i => i.InventoryType, itemJson, "inventoryType");
            AssignOptionalPrimitiveField(i => i.Equippable, itemJson, "equippable");
            AssignOptionalPrimitiveField(i => i.MaxCount, itemJson, "maxCount");
            AssignOptionalPrimitiveField(i => i.MaxDurability, itemJson, "maxDurability");

            ParseOptionalComplextTypes(itemJson);
        }

        private void AssignOptionalPrimitiveField<TProperty>(Expression<Func<Item, TProperty>> propertyExpression, JObject itemJson, string jsonKey)
        {
            if (!itemJson.ContainsKey(jsonKey))
            {
                return;
            }
            var memberExpression = (MemberExpression)propertyExpression.Body;
            var propertyInfo = (PropertyInfo)memberExpression.Member;
            propertyInfo.SetValue(this, itemJson[jsonKey].Value<TProperty>());
        }

        private void ParseOptionalComplextTypes(JObject itemJson)
        {
            if (itemJson.ContainsKey("tooltipParams"))
            {
                TooltipParams = new ItemTooltipParams(itemJson["tooltipParams"].Value<JObject>());
            }
            if (itemJson.ContainsKey("itemClass"))
            {
                ItemClass = ItemClassDataResource.BuildItemClassWithOnlyId(itemJson["itemClass"].Value<int>());
            }
            if (itemJson.ContainsKey("itemSubClass"))
            {
                ItemSubClass = ItemSubClassDataResource.BuildItemSubClassWithOnlyId(itemJson["itemSubClass"].Value<int>());
            }
        }
    }
}
