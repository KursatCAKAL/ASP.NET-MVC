using IDENTITY_MEMBERSHIP.Infrastructure.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace IDENTITY_MEMBERSHIP.Infrastructure.Context
{
    public class IdentityDBContext:IdentityDbContext<ApplicationUser>
    {
        public IdentityDBContext():base("server=.;database=IdentityDataBase;user id=sa;password=PAROLA")
        {

        }
    }
}
