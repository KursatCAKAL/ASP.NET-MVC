using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IDENTITY_MEMBERSHIP.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        [Authorize(Roles ="User")]//Rollerden sadece User'e sahip olacak girebilecek
        public ActionResult Index()
        {
            return View();
        }
    }
}