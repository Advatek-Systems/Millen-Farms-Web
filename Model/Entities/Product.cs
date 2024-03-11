using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Product
    {
        [Display(Name = "Product ID")]
        public int ProductID { get; set; }

        [Display(Name = "English Name")]
        public string EnglishTitle { get; set; }

        [Display(Name = "French Name")]
        public string FrenchTitle { get; set; }

        [Display(Name = "Certification Line 1")]
        public string CertLine1 { get; set; }

        [Display(Name = "Certification Line 2")]
        public string CertLine2 { get; set; }

        [Display(Name = "Product Code")]
        public string ProductCode { get; set; }

        public bool Enabled { get; set; }
    }
}
