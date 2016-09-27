using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Selama.Models
{
    public interface IGenericModel<TIdentity>
    {
        TIdentity ID { get; set; }
    }
}