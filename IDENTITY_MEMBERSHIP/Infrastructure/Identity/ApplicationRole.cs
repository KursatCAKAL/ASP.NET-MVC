using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDENTITY_MEMBERSHIP.Infrastructure.Identity
{
    public class ApplicationRole:IdentityRole//Geliştirilebilirliği var.
    {
        public string Description { get; set; }
        public ApplicationRole()
        {

        }
        public ApplicationRole(string roleName,string description):base(roleName)//Sonradan ekleme yapmak istediğim field'ı base'deki şu adrese gönder diyorum.
        {
            this.Description = description;
        }
    }
}