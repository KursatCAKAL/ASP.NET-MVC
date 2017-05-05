using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC_SCAFFOLDING.Models.Entity_Model;
using PagedList;
using PagedList.Mvc;
using System.Data.Entity.Infrastructure;

namespace MVC_SCAFFOLDING.Controllers
{
    public class ProductsController : Controller
    {
       
        private NorthwindEntities1 db = new NorthwindEntities1();
    
        #region [ViewResult Şeklinde Denendi Fakat cshtml tarafında Html.BeginForm içinde döndürmemiz gereken action ve controllermethod isimleri tutması gerekiyordu We have move that in AR Index]
        //public ViewResult Sorting(string currentFilter,string searchString,int? page)
        //{

        //    if (searchString!=null)
        //    {
        //        page = 1;
        //    }
        //    else
        //    {
        //        searchString = currentFilter;
        //    }
        //    ViewBag.CurrentFilter = searchString;

        //    //var products = db.Products.ToList();--> Bu şekilde yaparsan List olarak değer döner . var 41.Satırda IEnumerable olarak değer döner.
        //    //var products = from p db.Products select p;//yazlış yazma.
        //    var products = from p in db.Products select p;

        //    if (!String.IsNullOrEmpty(searchString))
        //    {
        //        products = products.Where(p => p.ProductName.Contains(searchString));
        //    }

        //    int pageSize = 3;
        //    int pageNumber = (page ?? 1);

        //    return View(products.OrderBy(x => x.CategoryID).ToPagedList(pageNumber,pageSize));
        //} 
        #endregion
        // GET: Products
        public ActionResult Index(int? SayfaNo, string currentFilter, string searchString, int? page)
        {
            //var products = db.Products.Include(p => p.Category).Include(p => p.Supplier);//Product tablosu için Category ve Supplier seçeneklerinide çekmiş dahil etmiş oluyorum.
            //return View(products.ToList());
            //Yukarıdaki değişikliklerden Category ve Suplieer ı çekememiş olucam.

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;

            //var products = db.Products.ToList();--> Bu şekilde yaparsan List olarak değer döner . var 41.Satırda IEnumerable olarak değer döner.
            //var products = from p db.Products select p;//yazlış yazma.
            var products = from p in db.Products select p;

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.ProductName.Contains(searchString));
            }
            //!Dikkat bu Line 73 ile çakıştırığı için  Line 69 kullanıldı//var products = db.Products;
            return View(products.OrderBy(x=>x.CategoryID).ToPagedList(SayfaNo ?? 1,10)); 

        }
  
        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "CompanyName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,ProductName,SupplierID,CategoryID,QuantityPerUnit,UnitPrice,UnitsInStock,UnitsOnOrder,ReorderLevel,Discontinued")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "CompanyName", product.SupplierID);
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "CompanyName", product.SupplierID);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,ProductName,SupplierID,CategoryID,QuantityPerUnit,UnitPrice,UnitsInStock,UnitsOnOrder,ReorderLevel,Discontinued")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "CompanyName", product.SupplierID);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
