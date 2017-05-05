using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebData;

namespace WebTemplate
{
    public partial class NestedMasterPage1 : System.Web.UI.MasterPage
    {
        
        #region BilgisayarParametre
        string prmFiyat = "";
        string prmHarddisk = "";
        string prmEkranBoyutu = "";
        string prmİslemci = "";
        string prmİsletimSistemi = "";
        string prmMarka = "";
        I_filtreBilgisayar filtre;
        #endregion
        public void FiltreMethod()
        {
            filtre = Page as I_filtreBilgisayar;
            if (filtre != null)
            {
                filtre.filtreBilgisayar(prmFiyat, prmMarka, prmEkranBoyutu, prmİslemci, prmİsletimSistemi, prmHarddisk);
            }
        }
        public void FiltreStringKontrolFiyat()
        {
            if (chkFiyat500.Checked==true)
            {
                prmFiyat = "500";
            }
            if (chkFiyat1000.Checked == true)
            {
                prmFiyat = "1000";
            }
            if (chkFiyat1500.Checked == true)
            {
                prmFiyat = "1500";
            }
            if (chkFiyat2000.Checked == true)
            {
                prmFiyat = "2000";
            }
            if (chkFiyat2500.Checked == true)
            {
                prmFiyat = "2500";
            }
            if (chkFiyat3000.Checked == true)
            {
                prmFiyat = "3000";//3000-5000 arasi !!
            }
            if (chkFiyat5000.Checked == true)
            {
                prmFiyat = "5000";
            }
        }
        public void FiltreStringKontrolBilgisayar()
        {

            #region stringEkranBoyut
            if (chk12inc.Checked == true)
            {
                prmEkranBoyutu = "12 inç";
            }
            if (chk15inc.Checked == true)
            {
                prmEkranBoyutu = "15 inç";
            }
            if (chk17_1inc.Checked == true)
            {
                prmEkranBoyutu = "17.1 inç";
            }
            #endregion
            #region stringİslemci
            if (chki3.Checked == true)
            {
                prmİslemci = "İntel Core i3";
            }
            if (chki5.Checked == true)
            {
                prmİslemci = "İntel Core i5";
            }
            if (chki7.Checked == true)
            {
                prmİslemci = "İntel Core i7";
            }
            if (chkAMD10.Checked == true)
            {
                prmİslemci = "AMD 10";
            }
            #endregion
            #region stringMarka
            if (chkAsusPC.Checked == true)
            {
                prmMarka = "Asus";
            }
            if (chkCasperPC.Checked == true)
            {
                prmMarka = "Casper";
            }
            if (chkMSİPC.Checked == true)
            {
                prmMarka = "Msi";
            }
            #endregion
            #region stringHarddisk
            if (chk250gb.Checked == true)
            {
                prmHarddisk = "250 GB";
            }
            if (chk500gb.Checked == true)
            {
                prmHarddisk = "500 GB";
            }
            if (chk1tb.Checked == true)
            {
                prmHarddisk = "1 TB";
            }
            if (chk1_5tb.Checked == true)
            {
                prmHarddisk = "1.5 TB";
            }
            if (chk2tb.Checked == true)
            {
                prmHarddisk = "2 TB";
            }
            #endregion
            #region stringisletimSistemi
            if (chkWindows10.Checked == true)
            {
                prmİsletimSistemi = "Windows 10";
            }
            if (chkWindows8_1.Checked == true)
            {
                prmİsletimSistemi = "Windows 8.1";
            }
            if (chkFreeDos.Checked == true)
            {
                prmHarddisk = "Free Dos";
            }
            #endregion
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
            switch (Request.QueryString["id"])
            {
                case "1":
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "script", "<script>markaTELHide();</script>", false);
                    break;
                case "2":
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "script", "<script>markaTELHide();</script>", false);
                    break;
                case "3":
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "script", "<script>markaTELHide();</script>", false);
                    break;
                case "4":
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "script", "<script>markaTELHide();</script>", false);
                    break;
                default:
                    break;
            }

        }
        #region chkBilgisayarFiltreOlayDizini
        protected void CheckBoxi3_CheckedChanged(object sender, EventArgs e)
        {
            chki5.Checked = false;
            chki7.Checked = false;
            chkAMD10.Checked = false;
            FiltreStringKontrolBilgisayar();
            FiltreMethod();

        }
        protected void CheckBoxi5_CheckedChanged(object sender, EventArgs e)
        {
            chki3.Checked = false;
            chki7.Checked = false;
            chkAMD10.Checked = false;
            FiltreStringKontrolBilgisayar();
            FiltreMethod();


        }
        protected void CheckBoxi7_CheckedChanged(object sender, EventArgs e)
        {
            chki5.Checked = false;
            chki3.Checked = false;
            chkAMD10.Checked = false;
            FiltreStringKontrolBilgisayar();
            FiltreMethod();


        }
        protected void CheckBoxAMD10_CheckedChanged(object sender, EventArgs e)
        {
            chki5.Checked = false;
            chki7.Checked = false;
            chki3.Checked = false;
            FiltreStringKontrolBilgisayar();
            FiltreMethod();
        }

        protected void CheckBoxAsusPC_CheckedChanged(object sender, EventArgs e)
        {

            chkCasperPC.Checked = false;
            chkMSİPC.Checked = false;

            FiltreStringKontrolBilgisayar();
            FiltreMethod();

        }
        protected void CheckBoxMsiPC_CheckedChanged(object sender, EventArgs e)
        {
            chkAsusPC.Checked = false;
            chkCasperPC.Checked = false;


            FiltreStringKontrolBilgisayar();
            FiltreMethod();

        }
        protected void CheckBoxCasperPC_CheckedChanged(object sender, EventArgs e)
        {
            chkAsusPC.Checked = false;
            chkMSİPC.Checked = false;

            FiltreStringKontrolBilgisayar();
            FiltreMethod();

        }

        protected void chk12inc_CheckedChanged(object sender, EventArgs e)
        {
            chk15inc.Checked = false;
            chk17_1inc.Checked = false;
            FiltreStringKontrolBilgisayar();
            FiltreMethod();
        }
        protected void chk15inc_CheckedChanged(object sender, EventArgs e)
        {
            chk12inc.Checked = false;
            chk17_1inc.Checked = false;
            FiltreStringKontrolBilgisayar();
            FiltreMethod();
        }
        protected void chk17_1inc_CheckedChanged(object sender, EventArgs e)
        {

            chk15inc.Checked = false;
            chk12inc.Checked = false;
            FiltreStringKontrolBilgisayar();
            FiltreMethod();
        }

        protected void chkFreeDos_CheckedChanged(object sender, EventArgs e)
        {
            chkWindows10.Checked = false;
            chkWindows8_1.Checked = false;

            FiltreStringKontrolBilgisayar();
            FiltreMethod();
        }
        protected void chkWindows8_1_CheckedChanged(object sender, EventArgs e)
        {
            chkWindows10.Checked = false;
            chkFreeDos.Checked = false;

            FiltreStringKontrolBilgisayar();
            FiltreMethod();
        }
        protected void chkWindows10_CheckedChanged(object sender, EventArgs e)
        {
            chkFreeDos.Checked = false;
            chkWindows8_1.Checked = false;

            FiltreStringKontrolBilgisayar();
            FiltreMethod();
        }

        protected void chk250gb_CheckedChanged(object sender, EventArgs e)
        {
            chk500gb.Checked = false;
            chk1tb.Checked = false;
            chk1_5tb.Checked = false;
            chk2tb.Checked = false;
            FiltreStringKontrolBilgisayar();
            FiltreMethod();
        }
        protected void chk500gb_CheckedChanged(object sender, EventArgs e)
        {
            chk250gb.Checked = false;
            chk1tb.Checked = false;
            chk1_5tb.Checked = false;
            chk2tb.Checked = false;
            FiltreStringKontrolBilgisayar();
            FiltreMethod();
        }
        protected void chk1tb_CheckedChanged(object sender, EventArgs e)
        {
            chk250gb.Checked = false;
            chk500gb.Checked = false;
            chk1_5tb.Checked = false;
            chk2tb.Checked = false;
            FiltreStringKontrolBilgisayar();
            FiltreMethod();
        }
        protected void chk1_5tb_CheckedChanged(object sender, EventArgs e)
        {
            chk250gb.Checked = false;
            chk500gb.Checked = false;
            chk1_5tb.Checked = false;
            chk2tb.Checked = false;
            FiltreStringKontrolBilgisayar();
            FiltreMethod();
        }
        protected void chk2tb_CheckedChanged(object sender, EventArgs e)
        {
            chk250gb.Checked = false;
            chk500gb.Checked = false;
            chk1_5tb.Checked = false;
            chk1tb.Checked = false;
            FiltreStringKontrolBilgisayar();
            FiltreMethod();
        } 
        #endregion
    
        
        protected void chkFiyat500_CheckedChanged(object sender, EventArgs e)
        {

            chkFiyat1000.Checked = false;
            chkFiyat1500.Checked = false;
            chkFiyat2000.Checked = false;
            Interface1 pageInterface = Page as Interface1;
            if (pageInterface != null)
            {
                pageInterface.DoAction(500, 1000);
            }

        }
        protected void chkFiyat1000_CheckedChanged(object sender, EventArgs e)
        {
            chkFiyat500.Checked = false;
            chkFiyat1500.Checked = false;
            chkFiyat2000.Checked = false;

            Interface1 pageInterface = Page as Interface1;
            if (pageInterface != null)
            {
                pageInterface.DoAction(1000, 1500);
            }

        }
        protected void chkFiyat1500_CheckedChanged(object sender, EventArgs e)
        {
            chkFiyat1000.Checked = false;
            chkFiyat500.Checked = false;
            chkFiyat2000.Checked = false;
            Interface1 pageInterface = Page as Interface1;
            if (pageInterface != null)
            {
                pageInterface.DoAction(1500, 2000);
            }
        }
        protected void chkFiyat2000_CheckedChanged(object sender, EventArgs e)
        {
            chkFiyat1000.Checked = false;
            chkFiyat1500.Checked = false;
            chkFiyat500.Checked = false;
            Interface1 pageInterface = Page as Interface1;
            if (pageInterface != null)
            {
                pageInterface.DoAction(2000, 4000);
            }
        }
        protected void chkFiyat5000_CheckedChanged(object sender, EventArgs e)
        {

        }
        protected void chkFiyat2500_CheckedChanged(object sender, EventArgs e)
        {

        }
        protected void chkFiyat3000_CheckedChanged(object sender, EventArgs e)
        {

        }


        #region ChekboxTopluCalistirma
        //string chkNameTut = "";
        //protected void chkKontrol(object sender, EventArgs e)
        //{
        //    Control tut = (Control)sender;
        //    chkNameTut = tut.ID.ToString();

        //    foreach (CheckBox item in this.Controls.OfType<System.Web.UI.WebControls.CheckBox>())
        //    {
        //        if (item.GetType()==typeof(CheckBox))
        //        {
        //            if ((item.ID == tut.ID))
        //            {
        //                item.Checked = true;
        //            }
        //            else
        //            {
        //                item.Checked = false;
        //            }
        //        }


        //    }

        //} 
        #endregion
        #region chkDuzenlemeMethodlarıBilgisayar
        public void chkMethodİslemci()
        {

            switch (chki3.Checked)
            {
                case true:
                    chki5.Checked = false;
                    chki7.Checked = false;
                    chkAMD10.Checked = false;
                    prmİslemci = "İntel Core i3";
                    break;
                case false:
                    prmİslemci = "";
                    break;
            }
            switch (chki5.Checked)
            {
                case true:
                    chki3.Checked = false;
                    chki7.Checked = false;
                    chkAMD10.Checked = false;
                    prmİslemci = "İntel Core i5";
                    break;
                case false:
                    prmİslemci = "";
                    break;
            }
            switch (chki7.Checked)
            {
                case true:
                    chki3.Checked = false;
                    chki5.Checked = false;
                    chkAMD10.Checked = false;
                    prmİslemci = "İntel Core i7";
                    break;
                case false:
                    prmİslemci = "";
                    break;
            }
            switch (chkAMD10.Checked)
            {
                case true:
                    chki3.Checked = false;
                    chki5.Checked = false;
                    chki7.Checked = false;
                    prmİslemci = "AMD 10";
                    break;
                case false:
                    prmİslemci = "";
                    break;
            }
        }
        public void chkMethodMarka()
        {
            switch (chkAsusPC.Checked)
            {
                case true:
                    chkCasperPC.Checked = false;
                    chkMSİPC.Checked = false;
                    prmMarka = "Asus";
                    break;
                case false:
                    prmMarka = "";
                    break;
                default:
                    break;
            }
            switch (chkCasperPC.Checked)
            {
                case true:
                    chkAsusPC.Checked = false;
                    chkMSİPC.Checked = false;
                    prmMarka = "Casper";
                    break;
                case false:
                    prmMarka = "";
                    break;
                default:
                    break;
            }
            switch (chkMSİPC.Checked)
            {
                case true:
                    chkAsusPC.Checked = false;
                    chkCasperPC.Checked = false;
                    prmMarka = "Msi";
                    break;
                case false:
                    prmMarka = "";
                    break;
                default:
                    break;
            }
        }
        public void chkMethodisletimSistemi()
        {
            switch (chkWindows8_1.Checked)
            {
                case true:
                    chkWindows10.Checked = false;
                    chkFreeDos.Checked = false;
                    prmİsletimSistemi = "Windows 8.1";
                    break;
                case false:
                    prmİsletimSistemi = "";
                    break;
            }
            switch (chkWindows10.Checked)
            {
                case true:
                    chkWindows8_1.Checked = false;
                    chkFreeDos.Checked = false;
                    prmİsletimSistemi = "Windows 10";
                    break;
                case false:
                    prmİsletimSistemi = "";
                    break;
            }
            switch (chkFreeDos.Checked)
            {
                case true:
                    chkWindows8_1.Checked = false;
                    chkWindows10.Checked = false;
                    prmİsletimSistemi = "Free Dos";
                    break;
                case false:
                    prmİsletimSistemi = "";
                    break;
            }
        }
        public void chkMethodHardDisk()
        {
            switch (chk250gb.Checked)
            {
                case true:
                    chk500gb.Checked = false;
                    chk1tb.Checked = false;
                    chk2tb.Checked = false;
                    chk1_5tb.Checked = false;
                    prmHarddisk = "250 GB";
                    break;
                case false:
                    prmHarddisk = "";
                    break;
            }
            switch (chk500gb.Checked)
            {
                case true:
                    chk250gb.Checked = false;
                    chk1tb.Checked = false;
                    chk2tb.Checked = false;
                    chk1_5tb.Checked = false;
                    prmHarddisk = "500 GB";
                    break;
                case false:
                    prmHarddisk = "";
                    break;
            }
            switch (chk1tb.Checked)
            {
                case true:
                    chk500gb.Checked = false;
                    chk250gb.Checked = false;
                    chk2tb.Checked = false;
                    chk1_5tb.Checked = false;
                    prmHarddisk = "1 TB";
                    break;
                case false:
                    prmHarddisk = "";
                    break;
            }
            switch (chk1_5tb.Checked)
            {
                case true:
                    chk500gb.Checked = false;
                    chk1tb.Checked = false;
                    chk2tb.Checked = false;
                    chk250gb.Checked = false;
                    prmHarddisk = "1.5 TB";
                    break;
                case false:
                    prmHarddisk = "";
                    break;
            }
            switch (chk2tb.Checked)
            {
                case true:
                    chk500gb.Checked = false;
                    chk1tb.Checked = false;
                    chk1_5tb.Checked = false;
                    chk250gb.Checked = false;
                    prmHarddisk = "2 TB";
                    break;
                case false:
                    prmHarddisk = "";
                    break;
            }
        }
        public void chkMethodEkranBoyutu()
        {
            switch (chk12inc.Checked)
            {
                case true:
                    chk15inc.Checked = false;
                    chk17_1inc.Checked = false;
                    prmEkranBoyutu = "12 inç";
                    break;
                case false:
                    prmEkranBoyutu = "";
                    break;
                default:
                    break;
            }
            switch (chk15inc.Checked)
            {
                case true:
                    chk12inc.Checked = false;
                    chk17_1inc.Checked = false;
                    prmEkranBoyutu = "15 inç";
                    break;
                case false:
                    prmEkranBoyutu = "";
                    break;
                default:
                    break;
            }
            switch (chk17_1inc.Checked)
            {
                case true:
                    chk12inc.Checked = false;
                    chk15inc.Checked = false;
                    prmEkranBoyutu = "17.1 inç";
                    break;
                case false:
                    prmEkranBoyutu = "";
                    break;
                default:
                    break;
            }
        }
        #endregion
    }

    //protected List<products> denemeSwitch(Button sender)
    //{
    //    Button yeni=sender;

    //    NorthwindEntities db = new NorthwindEntities();

    //    List<products> lst=db.Products.ToList();
    //    switch (yeni.ID.ToString())
    //    {
    //        case "CheckBox1":
    //            lst = db.Products.Where(x => x.UnitPrice <= 1000 && x.UnitPrice=>1500);
    //            break;
    //        default:
    //            break;
    //    }

    //    return lst;
    //}
}
