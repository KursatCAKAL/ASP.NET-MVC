using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_FUNDAMENTALS.Models.ModelEntity;
using MVC_FUNDAMENTALS.Models.ViewModel;

namespace MVC_FUNDAMENTALS.Controllers
{
    public class ProductController : Controller
    {
       
        private readonly NorthwindEntities DB;

        public ProductController()
        {

            DB = new NorthwindEntities();

        }
        // GET: Product
        public ActionResult TumUrunler()
        {
            List<Product> plist = DB.Products.ToList();
            return View(plist);
        }
        public ActionResult TumKategoriUrunler()
        {
            KategoriUrunVM VM = new KategoriUrunVM();
            VM.KategoriList = DB.Categories.ToList();
            VM.UrunList = DB.Products.ToList();

            return View(VM);
        }
        [HttpPost]
        public ActionResult TumKategoriUrunler(int hdnYakalananCatId)
        {
            KategoriUrunVM VM = new KategoriUrunVM();
            VM.KategoriList = DB.Categories.ToList();
            VM.UrunList = DB.Products.Where(x=>x.CategoryID==hdnYakalananCatId).ToList();
            
            return View(VM);
        }

    }
}