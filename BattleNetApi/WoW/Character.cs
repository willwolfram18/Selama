using BattleNetApi.WoW.Enums;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleNetApi.WoW
{
    public class Character
    {
        internal Character(JObject jsonCharacter)
        {
            ParseSimpleTypes(jsonCharacter);

            ParseEnums(jsonCharacter);

            ParseComplexTypes(jsonCharacter);
        }

        public string Name { get; private set; }

        public string Realm { get; private set; }

        public Class Class { get; private set; }

        public Race Race { get; private set; }

        public Gender Gender { get; private set; }

        public int Level { get; private set; }

        public Guild Guild { get; private set; }

        public string RealmName { get; private set; }

        public Alliance Alliance { get; private set; }

        public string Thumbnail { get; private set; }

        private void ParseSimpleTypes(JObject jsonCharacter)
        {
            Name = jsonCharacter["name"].Value<string>();
            Realm = jsonCharacter["realm"].Value<string>();
            Thumbnail = jsonCharacter["thumbnail"].Value<string>();
            Level = jsonCharacter["level"].Value<int>();
        }

        private void ParseEnums(JObject jsonCharacter)
        {
            Class = ParseCharacterEnum<Class>(jsonCharacter, "class");
            Race = ParseCharacterEnum<Race>(jsonCharacter, "race");
            Gender = ParseCharacterEnum<Gender>(jsonCharacter, "gender");
        }

        private void ParseComplexTypes(JObject jsonCharacter)
        {
            if (jsonCharacter["guild"] == null)
            {
                Guild = null;
            }
            else
            {
                Guild = new Guild(jsonCharacter["guild"] as JObject);
            }
        }
        private TEnum ParseCharacterEnum<TEnum>(JObject jsonCharacter, string key)
            where TEnum : struct
        {
            TEnum tempEnum;
            Enum.TryParse<TEnum>(jsonCharacter[key].Value<string>(), out tempEnum);
            return tempEnum;
        }

    }
}
