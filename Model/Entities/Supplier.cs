using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Supplier
    {
        [Display(Name="Supplier ID")]
        public int SupplierID { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string EstNo { get; set; }

        public bool Enabled { get; set; }
    }
}
