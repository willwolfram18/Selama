using BattleNetApi.Objects.WoW.Enums;

namespace BattleNetApi.Objects.WoW
{
    public class RaceDataResource
    {
        public int Id { get; private set; }

        public int Mask { get; private set; }

        public Race Race { get; private set; }
        
        public Faction Faction { get; private set; }

    }
}
