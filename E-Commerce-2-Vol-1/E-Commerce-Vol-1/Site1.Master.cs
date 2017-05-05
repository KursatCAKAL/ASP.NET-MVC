using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;



namespace WebTemplate
{
    public partial class Site1 : System.Web.UI.MasterPage/*, chkControlInterface*/
    {
        //public void DoActionCheckBox(Object sender)
        //{

        //    string controlName = string.Empty;
        //    Control tut = (Control)sender;

        //    foreach (CheckBox item in NestedMasterPage1.Control.Controls)
        //    {
        //        if (item.ID.Substring(0, 5).ToString() == "fiyat")
        //        {
        //            if (item.ID != tut.ID)
        //            {
        //                item.Checked = false;
        //            }
        //        }
        //    }

        //}
       
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void lnkbtnDB_Click(object sender, EventArgs e)
        {
            Response.Redirect(DirectMenu("1"));
        }
        protected void lnkbtnMB_Click(object sender, EventArgs e)
        {
            Response.Redirect(DirectMenu("1"));
        }
        protected void lnkbtnT_Click(object sender, EventArgs e)
        {
            Response.Redirect(DirectMenu("2"));
        }
        protected void lnkbtnBB_Click(object sender, EventArgs e)
        {
            Response.Redirect(DirectMenu("3"));
        }
        protected void lnkbtnV_Click(object sender, EventArgs e)
        {
            Response.Redirect(DirectMenu("4"));
        }
        public string DirectMenu(string id)
        {
            string query="";

            switch (id)
            {
                case "1":
                    query += "/products.aspx?id=1";
                    break;
                case "2":
                    query += "/products.aspx?id=2";
                    break;
                case "3":
                    query += "/products.aspx?id=3";
                    break;
                case "4":
                    query += "/products.aspx?id=4";
                    break;
                case "5":
                    query += "/products.aspx?id=5";
                    break;
                case "6":
                    query += "/products.aspx?id=6";
                    break;
                default:
                    query += "/products.aspx";
                    break;
            }
            return query;
        }
        //private void controlChoicer_click(object sender, EventArgs e)
        //{
        //    LinkButton btn = sender as LinkButton;

        //    switch (btn.ID)
        //    {
        //        case "lnkbtnDB":
        //            Response.Redirect(DirectMenu("1"));
        //            break;
        //        case "lnkbtnMB":
        //            Response.Redirect(DirectMenu("1"));
        //            break;
        //        case "lnkbtnOB":
        //            Response.Redirect(DirectMenu("1"));
        //            break;
        //        case "lnkbtnT":
        //            Response.Redirect(DirectMenu("2"));
        //            break;
        //        case "lnkbtnBB":
        //            Response.Redirect(DirectMenu("3"));
        //            break;
        //        case "lnkbtnV":
        //            Response.Redirect(DirectMenu("4"));
        //            break;
        //    }
        //}
    }
}