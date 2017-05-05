using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Net.Mail;

namespace WebTemplate
{
    public partial class Contact : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnGonder_Click(object sender, EventArgs e)
        {
            MailMessage gidecekMesaj = new MailMessage();
            gidecekMesaj.From = new MailAddress(txtEmail.ToString());
            gidecekMesaj.To.Add("kursat.cakal@hotmail.com");
            gidecekMesaj.Subject = txtKonu.ToString();
            gidecekMesaj.Body = txtMesaj.ToString();
            SmtpClient smtp = new SmtpClient();
            #region Tamamlanacak
            //smtp.Credentials = new NetworkCredential("mail adresi", "mail adresimizin şifresi");
            //smtp.Port = mail sunucunuzun port numarası;
            //smtp.Host = "mailin sunucusu ";
            //smtp.Send(ePosta);
            //btnGonder.Text = "E-Posta Başarıyla Gönderildi"; 
            #endregion
        }
    }
}