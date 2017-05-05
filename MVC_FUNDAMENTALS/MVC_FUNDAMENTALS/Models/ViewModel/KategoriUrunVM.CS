using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVC_FUNDAMENTALS.Models.ModelEntity;



namespace MVC_FUNDAMENTALS.Models.ViewModel
{
    public class KategoriUrunVM
    {
        public List<Product> UrunList { get; set; }
        public List<Category> KategoriList { get; set; }
    }
}