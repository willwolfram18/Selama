using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleNetApi.Objects.WoW
{
    public class GuildNews
    {
        public string Type { get; private set; }

        public string CharacterName { get; private set; }

        public DateTime Timestamp { get; private set; }

        public int? ItemId { get; private set; }

        public string Context { get; private set; }

        public List<int> BonusLists { get; private set; }

        public Achievement Achievement { get; private set; }
    }
}
