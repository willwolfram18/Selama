using BattleNetApi.Common.ExtensionMethods;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleNetApi.Objects.WoW
{
    public class ItemTooltipParams
    {
        #region Properties
        public List<int> GemIds { get; private set; }

        public int? TransmogItemId { get; private set; }

        public int? EnchantmentId { get; private set; }

        public ItemUpgradeInfo Upgrade { get; private set; }
        #endregion

        internal ItemTooltipParams(JObject itemTooltipJson)
        {
            if (itemTooltipJson.ContainsKey("transmogItem"))
            {
                TransmogItemId = itemTooltipJson["transmogItem"].Value<int>();
            }
            if (itemTooltipJson.ContainsKey("enchant"))
            {
                EnchantmentId = itemTooltipJson["enchant"].Value<int>();
            }

            GetItemGems(itemTooltipJson);
            if (itemTooltipJson.ContainsKey("upgrade"))
            {
                Upgrade = new ItemUpgradeInfo(itemTooltipJson["upgrade"].Value<JObject>());
            }
        }

        private void GetItemGems(JObject itemTooltipJson)
        {
            // remove the "gem" portion of the key "gem#" to allow ordering by the gem position number
            var gemKeys = itemTooltipJson.Keys().Where(k => k.StartsWith("gem"))
                .OrderBy(s => Int32.Parse(s.Replace("gem", "")));

            GemIds = new List<int>();
            foreach (var gemId in gemKeys)
            {
                GemIds.Add(itemTooltipJson[gemId].Value<int>());
            }
        }
    }
}
