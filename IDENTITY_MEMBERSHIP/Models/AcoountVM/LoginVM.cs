using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;//İşlemlerimiz için ihtiyaç duyacağız.
namespace IDENTITY_MEMBERSHIP.Models.AcoountVM
{
    public class LoginVM
    {
        [Required]
        [Display(Name="Kullanıcı Adı")]
        public string UserName { get; set; }
        [Required]
        [Display(Name = "Şifre")]
        public string Password { get; set; }
        [Display(Name = "Beni Hatırla")]
        public bool RememberMe { get; set; }
    }
}