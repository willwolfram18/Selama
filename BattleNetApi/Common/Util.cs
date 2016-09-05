using BattleNetApi.Objects.WoW.Enums;
using Newtonsoft.Json.Linq;
using System;

namespace BattleNetApi.Common
{
    internal static class Util
    {
        internal const double COPPER_IN_SILVER = 100.0;
        internal const double SILVER_IN_GOLD = 100.0;

        internal static TEnum ParseEnum<TEnum>(JObject jsonObject, string enumKey)
            where TEnum : struct
        {
            TEnum temp;
            Enum.TryParse<TEnum>(jsonObject[enumKey].Value<string>(), true, out temp);
            return temp;
        }

        internal static Faction SelectFactionFromRace(Race race)
        {
            switch (race)
            {
                case Race.Human:
                case Race.Dwarf:
                case Race.NightElf:
                case Race.Gnome:
                case Race.Draenei:
                case Race.Worgen:
                case Race.PandarenAlliance:
                    return Faction.Alliance;
                case Race.Undead:
                case Race.Orc:
                case Race.Tauren:
                case Race.Troll:
                case Race.Goblin:
                case Race.BloodElf:
                case Race.PandarenHorde:
                    return Faction.Horde;
                case Race.PandarenNeutral:
                    return Faction.Neutral;
                default:
                    return Faction.NULL;
            }
        }

        internal static DateTime BuildUnixTimestamp(long milliseconds)
        {
            return new DateTime(1970, 1, 1).AddMilliseconds(milliseconds);
        }
    }
}
