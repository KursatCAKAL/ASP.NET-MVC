using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IDENTITY_MEMBERSHIP.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        [Authorize(Roles ="Admin")]//Rollerden sadece Admin'e sahip olacak girebilecek
        public ActionResult Index()
        {
            return View();
        }
    }
}