using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using MVC_CRUD.DATA.Model_Entity;

namespace MVC_CRUD.WEBUI.Controllers
{
    public class CategoryController : Controller
    {
        private readonly NorthwindEntities DB;
        public CategoryController()
        {
            DB = new NorthwindEntities();
        }
        // GET: Category
        public ActionResult AllCategories()
        {
            //List<Category> CatList = DB.Categories.ToList();

            //return View(CatList);
            return View(DB.Categories.ToList());
        }
        [HttpPost]
        public ActionResult InsertUpdateCategory(string txtAd,string txtAciklama,int? hdnKatID)
        {
            if (hdnKatID!=null)
            {
                Category cselected=DB.Categories.SingleOrDefault(x => x.CategoryID == hdnKatID);
                cselected.CategoryName = txtAd;
                cselected.Description = txtAciklama;
                DB.Categories.Attach(cselected);//Entity için update işlemi yapan method attach
                DB.Entry(cselected).State = System.Data.Entity.EntityState.Modified;//ben entitye dokunulduğunu söyliycem-Modified diyince tabloda dokunulmuş veri varmı karşılaştıracak ona göre işlem yapıcak.
                DB.SaveChanges();
            }
            else
            {
                Category c = new Category()
                {
                    CategoryName = txtAd,
                    Description = txtAciklama
                };
                DB.Categories.Add(c);
                DB.SaveChanges();
            }
           return RedirectToAction("AllCategories","Category");
            //hangi actionın hangi controlını çağırmak istiyorsun. //AllCategories action result'ını  Category controllerından çalıştırmak istiyorum diyorum.
            //return View();//Returnde view dönerse sistem patlar çünkü yeni bir kategory kaydettim ve o kategory'i çalıştıramıyoruz eski view de kaldı
            //HER ActionResult view döndürmek zorunda değil.
        }


        //public ActionResult DeleteCategory(int ID)!!!ÇOK DİKKAT ET GET METHODLARI ID 'yi bu şekilde algılayamazlar.
        //{
        //    Category cselected = DB.Categories.Where(x => x.CategoryID == ID).FirstOrDefault();//SingleOrDefault daha doğru ama bunuda gör
        //    DB.Categories.Remove(cselected);
        //    DB.SaveChanges();
        //    return View();
        //}
        public ActionResult DeleteCategory(int id)//get methodu id'yi bu şekilde algılaya biliyor bir nevi türkçe karakter olayından kaynaklı galiba.
        {
            Category cselected = DB.Categories.Where(x => x.CategoryID == id).FirstOrDefault();//SingleOrDefault daha doğru ama bunuda gör
            DB.Categories.Remove(cselected);
            //DB.SaveChanges();
            try//Ekstra durumda kullandık.
            {
                DB.SaveChanges();
            }
            catch (Exception)
            {

                return Content("<script language='javascript' type='text/javascript'>alert('Dikkat bu veri üzerinde değişiklik yapma yetkiniz yoktur.');</script>");
            }

            //return View(); returnde view dönemezsin
            return RedirectToAction("AllCategories", "Category");//içeriye hangi action'ın hangi controllerdan çalıştırmak istediğimi yazıyorum
        }

    }
}