using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Last.Bench.Coder.Beauty.World.Utility
{
    public static class EnumData
    {
        public static bool TryParseEnum<TEnum>(this string enumValue)
        {
            bool success = Enum.IsDefined(typeof(TEnum), enumValue);
            return success;
        }
    }
}