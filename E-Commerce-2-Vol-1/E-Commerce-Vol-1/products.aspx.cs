using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebData;

namespace WebTemplate
{
    public partial class products : System.Web.UI.Page, Interface1,I_filtreBilgisayar
    {
        
        public void DoAction(int a, int b)
        {

            NorthwindEntities db = new NorthwindEntities();
            rptProduct.DataSource = db.Products.Where(x => x.UnitPrice >= a && x.UnitPrice <= b).ToList();
            rptProduct.DataBind();
           

        }
        public void filtreBilgisayar(string fiyat="",string marka="",string ekranboyutu="",string islemci="",string isletimSis="",string harddisk="")
        {
            NorthwindEntities db = new NorthwindEntities();
            var joinList = (from pe in db.PE_Bilgisayar
                            join p in db.Products on pe.ProductID equals p.ProductID
                            select new
                            {
                               p.ProductName,
                               p.ImageURL,
                               p.ProductID,
                               p.UnitPrice,
                               pe.Marka,
                               pe.Harddisk,
                               pe.SubCategoryID,
                               pe.İslemci,
                               pe.İsletimSistemi,
                               pe.EkranBoyutu
                            }
                            );

            var LST2=joinList.Where(x => x.Marka.Contains(marka) && x.UnitPrice.ToString().Contains(fiyat) &&x.Marka.Contains(fiyat) && x.EkranBoyutu.Contains(ekranboyutu) && x.İslemci.Contains(islemci) && x.İsletimSistemi.Contains(isletimSis) && x.Harddisk.Contains(harddisk)).ToList().OrderBy(x=>x.UnitPrice);
            //List<PE_Bilgisayar> LST = db.PE_Bilgisayar.Where(x => x.Marka.Contains(marka) && x.Fiyat.Contains(fiyat) && x.EkranBoyutu.Contains(ekranboyutu) && x.İslemci.Contains(islemci) && x.İsletimSistemi.Contains(isletimSis) && x.Harddisk.Contains(harddisk)).ToList();

            rptProduct.DataSource = LST2;
            rptProduct.DataBind();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "none", "<script>markaPCShow();</script>", false);
            if (!IsPostBack)
            {
                NorthwindEntities db = new NorthwindEntities();
                if (Request.QueryString["id"]=="1")
                {
                    if (Request.QueryString["a"] == "500" && Request.QueryString["b"] == "1000")
                    {
                        rptProduct.DataSource = db.Products.Where(x => x.UnitPrice >= 500 && x.UnitPrice <= 1000);
                        rptProduct.DataBind();
                    }
                    else
                    {
                        rptProduct.DataSource = db.Products.Where(x => x.SubCategoryID == 1).ToList().OrderBy(x=>x.UnitPrice);
                        rptProduct.DataBind();
                    }
                }
            }

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
        }
    }
}