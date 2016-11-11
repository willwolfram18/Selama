using Selama.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Selama.Models
{
    public class IdentityRepository : EntityRepositoryBase<ApplicationDbContext, ApplicationUser>
    {
        public IdentityRepository(ApplicationDbContext context) : base(context) { }
    }
}