using IDENTITY_MEMBERSHIP.Infrastructure.Identity;
using IDENTITY_MEMBERSHIP.Models.AcoountVM;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IDENTITY_MEMBERSHIP.Infrastructure.Context;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Host.SystemWeb;
using System.Security.Claims;

namespace IDENTITY_MEMBERSHIP.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        // GET: Account
        public AccountController()
        {
            IdentityDBContext DB = new IdentityDBContext();

            UserStore<ApplicationUser> userStore = new UserStore<ApplicationUser>(DB);
            userManager = new UserManager<ApplicationUser>(userStore);//manager'ı hangi store üzerinde barındırmak istiyorsun diye soruyor veriyorum barındırmak istediğim yeri
            RoleStore<ApplicationRole> roleStore = new RoleStore<ApplicationRole>(DB);//Gene bu manager'ı hangi store üzerinde barındırmak istediğimi soruyor.
            roleManager = new RoleManager<ApplicationRole>(roleStore);//rolStore'dan türettim
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser uselected = userManager.Find(model.UserName, model.Password);
                if (uselected!=null)
                {
                    IAuthenticationManager auth = HttpContext.GetOwinContext().Authentication;
                    AuthenticationProperties autProperty = new AuthenticationProperties();
                    ClaimsIdentity claims = userManager.CreateIdentity(uselected,"ApplicationCookie");

                    autProperty.IsPersistent = model.RememberMe;
                    auth.SignIn(autProperty, claims);
                    return RedirectToAction("Index", "Home");//İşte giriş başarılı olduysa yönlendirileceği yer.
                }
                else
                {
                    return View(model);//Yanlış giriş olursa gene model ile yönlendirelim ki veriler kaybolmasın.
                }
                   
            }
            return View(model);//Veriler boş ise diye aynen döndürmek için.
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser uinserted = new ApplicationUser()
                {
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserName = model.UserName,
                };
                //Password'deki hasinh işlemini gerçekleştirmek için bir yapıya ihtiyacımız olacak.
                IdentityResult Iresult = userManager.Create(uinserted, model.Password);
                if (Iresult.Succeeded)
                {
                    userManager.AddToRole(uinserted.Id, "User"); 
                    return RedirectToAction("Login", "Account");
                }
                //else
                //{
                //    return View(model);//Optional
                //}
                
            }
            return View(model);
        }
        public ActionResult LogOut()
        {
            IAuthenticationManager auth = HttpContext.GetOwinContext().Authentication;
            auth.SignOut("ApplicationCookie");
            return RedirectToAction("Index", "Home");
        }
        

    }
}