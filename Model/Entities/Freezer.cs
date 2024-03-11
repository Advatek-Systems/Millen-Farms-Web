using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Model
{
    public class Freezer
    {
        [Display(Name = "Freezer ID")]
        public int FreezerID { get; set; }

        [Display(Name = "Freezer Name")]
        public string FreezerName { get; set; }
    }
}
