using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class BoxSize
    {
        [Display(Name = "Box Size ID")]
        public int BoxSizeID { get; set; }

        [Display(Name = "Box Name")]
        public string BoxName { get; set; }

        [Display(Name = "Box Weight (LBs)")]
        public decimal BoxWeightLB { get; set; }

        [Display(Name = "Box Weight (KGs)")]
        public decimal BoxWeightKG { get; set; }

        public bool Enabled { get; set; }
    }
}
