using Selama.Common.Enum;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Selama.Controllers
{
    [Authorize]
    public class _AuthorizeControllerBase : _ControllerBase
    {
    }
}