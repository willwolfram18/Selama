using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleNetApi.Common.ExtensionMethods
{
    internal static class EnumExtensionMethods
    {
        internal static void ParseEnum<TEnum>(this TEnum enumObj, string enumValue)
            where TEnum : struct
        {
            Enum.TryParse<TEnum>(enumValue, true, out enumObj);
        }
    }
}
