using Newtonsoft.Json.Linq;

namespace BattleNetApi.Objects.WoW.DataResources
{
    public class ItemSubClassDataResource
    {
        public int Id { get; private set; }

        public string Name { get; private set; }

        internal ItemSubClassDataResource(JObject itemSubClassJson)
        {
            Id = itemSubClassJson["subclass"].Value<int>();
            Name = itemSubClassJson["name"].Value<string>();
        }
    }
}
