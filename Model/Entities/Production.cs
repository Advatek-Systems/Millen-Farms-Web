using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Model
{
    public class Production
    {
        [Display(Name = "Device ID")]
        public int DeviceID { get; set; }

        [Display(Name = "Lot Number")]
        public string LotNo { get; set; }

        [Display(Name = "Serial Number")]
        public string SerialNo { get; set; }

        [Display(Name = "Full Skid Number")]
        public string FullSkidNo { get; set; }

        public decimal Gross { get; set; }

        public decimal Tare { get; set; }

        public string FreshOrFrozen { get; set; }

        public decimal Net { get; set; }

        [Display(Name = "My Target")]
        public decimal MyTarget { get; set; }

        public string Unit { get; set; }

        [Display(Name = "Date/Time")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime RecDateTime { get; set; }

        [Display(Name = "Date/Time")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime InsDateTime { get; set; }

        [Display(Name = "State Code")]
        public int StateCode { get; set; }

        [Display(Name = "State Code")]
        public string StateCodeName { get; set; }

        public string DisplayDate { get; set; }

        public string Name { get; set; }

        [Display(Name = "Trailer Number")]
        public string TrailerNo { get; set; }

        [Display(Name = "Customer ID")]
        public int CustomerID { get; set; }

        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }

        [Display(Name = "Box Size ID")]
        public int BoxSizeID { get; set; }

        [Display(Name = "Box Size Name")]
        public string BoxName { get; set; }

        [Display(Name = "Box Weight (LBs)")]
        public decimal BoxWeightLB { get; set; }

        [Display(Name = "Freezer ID")]
        public int FreezerID { get; set; }
    }
}
