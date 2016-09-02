using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleNetApi.Objects.WoW
{
    public class ItemSpell
    {
        #region Properties
        public int SpellId { get; private set; }

        public Spell Spell { get; private set; }
        #endregion

        internal static ItemSpell ParseItemSpellJson(JObject itemSpellJson)
        {

        }

        private ItemSpell(JObject itemSpellJson)
        {
            SpellId = itemSpellJson["spellId"].Value<int>();
            Spell = Spell.ParseSpellJson(itemSpellJson["spell"].Value<JObject>());
        }
        // TODO: Add properties and factory function
    }
}
