using Newtonsoft.Json.Linq;

namespace BattleNetApi.Objects.WoW
{
    public class BonusChanceStat
    {
        #region Properties
        public int Stat { get; private set; }

        public int Delta { get; private set; }
        #endregion

        internal static BonusChanceStat ParseBonusChanceStatJson(JObject bonusChanceStatJson)
        {
            return new BonusChanceStat
            {
                Stat = bonusChanceStatJson["statId"].Value<int>(),
                Delta = bonusChanceStatJson["delta"].Value<int>(),
            };
        }
    }
}
