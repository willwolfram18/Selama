using BattleNetApi.Api.Enums;
using BattleNetApi.Objects.WoW.Enums;
using System.Collections.Generic;

namespace BattleNetApi.Objects.WoW
{

    public class Rootobject
    {
        #region Instance properties
        public RealmType Type { get; private set; }
        
        public RealmPopulation Population { get; private set; }
        
        public bool Queue { get; private set; }
        
        public bool Status { get; private set; }
        
        public string Name { get; private set; }
        
        public string Slug { get; private set; }
        
        public string BattleGroup { get; private set; }
        
        public Locale Locale { get; private set; }
        
        public string Timezone { get; private set; }
        
        public List<string> ConnectedRealms { get; private set; }
        #endregion
    }

}
