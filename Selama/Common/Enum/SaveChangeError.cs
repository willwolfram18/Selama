using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Selama.Common.Enum
{
    public enum SaveChangeError
    {
        None,
        ConcurrencyError,
        Unknown
    }
}