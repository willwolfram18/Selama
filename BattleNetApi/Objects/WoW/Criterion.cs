using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleNetApi.Objects.WoW
{
    public class Criterion
    {
        public int Id { get; private set; }

        public string Description { get; private set; }

        public int OrderIndex { get; private set; }

        public int Max { get; private set; }
    }
}
