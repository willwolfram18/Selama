using BattleNetApi.WoW.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleNetApi.WoW
{
    public class Character
    {
        public string Name { get; set; }

        public string Realm { get; set; }

        public Class Class { get; set; }

        public Race Race { get; set; }

        public Gender Gender { get; set; }

        public int Level { get; set; }

        public Guild Guild { get; set; }

        public string RealmName { get; set; }

        public Alliance Alliance { get; set; }

        public string ThumbnailInfo { get; set; }
    }
}
