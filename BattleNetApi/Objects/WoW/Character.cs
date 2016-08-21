using BattleNetApi.Common;
using BattleNetApi.Common.ExtensionMethods;
using BattleNetApi.Objects.WoW.Enums;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleNetApi.Objects.WoW
{
    public class Character
    {
        internal static Character BuildOAuthProfileCharacter(JObject jsonCharacter)
        {
            return new Character(jsonCharacter);
        }

        internal static Character BuildCharacterProfileEndpoint(JObject jsonCharacter)
        {
            // TODO: Account for issues like Achievements field
            return new Character(jsonCharacter);
        }

        #region Constructors
        private Character(JObject jsonCharacter)
        {
            ParsePrimitiveTypes(jsonCharacter);

            ParseEnums(jsonCharacter);

            ParseComplexTypes(jsonCharacter);
        }
        #endregion

        #region Properties
        public string Name { get; private set; }

        public string Realm { get; private set; }

        public Class Class { get; private set; }

        public Race Race { get; private set; }

        public Gender Gender { get; private set; }

        public int Level { get; private set; }

        public Guild Guild { get; private set; }

        public Faction Faction { get; private set; }

        public string Thumbnail { get; private set; }

        public Specialization Specialization { get; private set; }

        public int AchievementPoints { get; private set; }

        public List<Title> Titles { get; private set; }

        public int TotalHonorableKills { get; private set; }
        #endregion

        #region Private interface
        private void ParsePrimitiveTypes(JObject jsonCharacter)
        {
            Name = jsonCharacter["name"].Value<string>();
            Realm = jsonCharacter["realm"].Value<string>();
            Thumbnail = jsonCharacter["thumbnail"].Value<string>();
            Level = jsonCharacter["level"].Value<int>();
            AchievementPoints = jsonCharacter["achievementPoints"].Value<int>();
            ParsePossiblyMissingPrimitiveTypes(jsonCharacter);
        }

        private void ParsePossiblyMissingPrimitiveTypes(JObject jsonCharacter)
        {
            if (jsonCharacter.ContainsKey("totalHonorableKills"))
            {
                TotalHonorableKills = jsonCharacter["totalHonorableKills"].Value<int>();
            }
        }

        private void ParseEnums(JObject jsonCharacter)
        {
            Class = Util.ParseEnum<Class>(jsonCharacter, "class");
            Race = Util.ParseEnum<Race>(jsonCharacter, "race");
            Gender = Util.ParseEnum<Gender>(jsonCharacter, "gender");
            Faction = Util.SelectFactionFromRace(Race);
        }

        private void ParseComplexTypes(JObject jsonCharacter)
        {
            Guild = null;
            if (jsonCharacter.ContainsKey("guild"))
            {
                Guild = Guild.BuildOAuthCharacterGuild(jsonCharacter);
            }

            Specialization = null;
            if (jsonCharacter.ContainsKey("spec"))
            {
                Specialization = Specialization.BuildCharacterSpecialization(jsonCharacter["spec"].Value<JObject>(), Class);
            }


            Titles = Title.BuildListOfTitles(jsonCharacter);
        }
        #endregion
    }
}
