using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IDENTITY_MEMBERSHIP.Models.AcoountVM
{
    public class RegisterVM
    {
        
        [Required]
        [Display(Name = "Adı")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Soyadı")]
        public string LastName { get; set; }
        [Required]
        [EmailAddress(ErrorMessage ="Geçerli")]//Direk regex entegrasyonu ile email formatında olup olmadığını kontrol ediyor. 
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required] 
        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; }
        [Required]
        [Display(Name = "Şifre")]
        public string Password { get; set; }
        [Required]
        [Display(Name = "Şifre Tekrar")]
        [Compare("Password",ErrorMessage ="Şifreler Eşleşmedi")]
        public string ConfirmPassword { get; set; }

       
    }
}