using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using IDENTITY_MEMBERSHIP.Infrastructure.Context;
using Microsoft.AspNet.Identity.EntityFramework;
using IDENTITY_MEMBERSHIP.Infrastructure.Identity;
using Microsoft.AspNet.Identity;

namespace IDENTITY_MEMBERSHIP
{
    public class MvcApplication : System.Web.HttpApplication
    {
        //Global.asax applicationsal işlemlerimi yapar.
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            IdentityDBContext DB = new IdentityDBContext();
            RoleStore<ApplicationRole> rStore = new RoleStore<ApplicationRole>(DB);
            RoleManager<ApplicationRole> rManager = new RoleManager<ApplicationRole>(rStore);
            if (!rManager.RoleExists("Admin"))
            {
                ApplicationRole adminRole = new ApplicationRole("Admin", "SistemYöneticisi");
                rManager.Create(adminRole); 
            }
            if (!rManager.RoleExists("User"))
            {
                ApplicationRole userRole = new ApplicationRole("User", "SistemKullanici");
                rManager.Create(userRole);
            }
        }
    }
}
