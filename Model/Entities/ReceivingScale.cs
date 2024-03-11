using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ReceivingScale
    {
        [Display(Name = "Device ID")]
        public int DeviceID { get; set; }

        [Display(Name = "Lot Number")]
        public string LotNo { get; set; }

        [Display(Name = "Trailer Number")]
        public string TrailerNo { get; set; }

        [Display(Name = "Pallet Number")]
        public string PalletNo { get; set; }

        [Display(Name = "Supplier ID")]
        public int SupplierID { get; set; }

        [Display(Name = "Supplier Name")]
        public string SupplierName { get; set; }

        [Display(Name = "Product ID")]
        public int ProductID { get; set; }

        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        public decimal Gross { get; set; }

        public decimal Tare { get; set; }

        public decimal Net { get; set; }

        [Display(Name = "Blue Totes or Small Bins")]
        public string BlueTotesOrSmallBins { get; set; }

        [Display(Name = "Quantity of Totes/Bins")]
        public int QuantityTotesBins { get; set; }

        public int Status { get; set; }

        [Display(Name = "Date/Time")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime RecDateTime { get; set; }

        [Display(Name = "Insert Date/Time")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime InsDateTime { get; set; }

        public string DisplayDate { get; set; }

        [Display(Name = "Pallet Weight")]
        public decimal PalletWeight { get; set; }

        [Display(Name = "State Code ID")]
        public int StateCode { get; set; }

        [Display(Name = "State Code Name")]
        public string StateCodeName { get; set; }

        [Display(Name = "Freezer ID")]
        public int FreezerID { get; set; }
    }
}
