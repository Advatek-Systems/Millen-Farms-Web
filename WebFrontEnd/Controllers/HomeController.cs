using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Protocols;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using Model;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.ExcelUtilities;
using Service;

namespace WebFrontEnd.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppService service = new AppService();

        private List<string> year = GetYearList();
        
        private static List<string> GetYearList()
        {
            List<string> yearList = new List<string>();
            int currentYear = DateTime.Now.Year;

            for (int i = currentYear; i >= 2021; i--)
            {
                yearList.Add(i.ToString());
            }

            return yearList;
        }

        public ActionResult Index()
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Login");

            return View();
        }

        #region Receiving
        public ActionResult ReceivingBySupplierAndLot()
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Login");

            ViewBag.Suppliers = service.GetAllSuppliers();

            return View();
        }

        [HttpPost]
        public ActionResult ReceivingBySupplierAndLot(int supplierID, string lotNo)
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Login");

            ViewBag.Suppliers = service.GetAllSuppliers();

            if (string.IsNullOrEmpty(lotNo) || string.IsNullOrWhiteSpace(lotNo))
            {
                ViewBag.Msg = "Lot Number is required!";
                return View();
            }

            return RedirectToAction("ReceivingReport", new { supplierID, selectedYear = "", lotNo });
        }

        public ActionResult ReceivingBySupplierAndYear()
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Login");

            ViewBag.Suppliers = service.GetAllSuppliers();
            ViewBag.Years = year;

            return View();
        }

        [HttpPost]
        public ActionResult ReceivingBySupplierAndYear(int supplierID, string selectedYear)
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Login");

            ViewBag.Suppliers = service.GetAllSuppliers();
            ViewBag.Years = year;

            return RedirectToAction("ReceivingReport", new { supplierID, selectedYear, lotNo = "" });
        }

        public ActionResult ReceivingReport(int supplierID, string selectedYear, string lotNo)
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Login");

            List<ReceivingScale> info = new List<ReceivingScale>();

            if (string.IsNullOrEmpty(selectedYear))
                info = service.GetReceivingBySupplierAndLot(supplierID, lotNo);
            else if (string.IsNullOrEmpty(lotNo))
                info = service.GetReceivingBySupplierAndYear(supplierID, selectedYear);

            ViewBag.Count = info.Count;
            ViewBag.ReportViewer = GenerateReceivingScaleReport(info);

            return View();
        }

        public ActionResult ManageReceiving()
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Login");

            return View(service.GetTodaysReceivingScaleInfo());
        }

        [HttpPost]
        public ActionResult ManageReceiving(string searchString)
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Login");

            TempData["RecLoc"] = "search";
            TempData["RecSearchStr"] = searchString;

            return View(service.GetReceivingScaleInfoBySearch(searchString));
        }

        public ActionResult SearchByDates()
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Login");

            return View();
        }

        [HttpPost]
        public ActionResult SearchByDates(DateTime? start, DateTime? end)
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Login");

            if (!start.HasValue)
            {
                ViewBag.Msg = "Start Date is required!";
                return View();
            }
            if (!end.HasValue)
            {
                ViewBag.Msg = "End Date is required!";
                return View();
            }
            if (start.Value > end.Value)
            {
                ViewBag.Msg = "Start Date cannot be ahead of End Date!";
                return View();
            }

            //changed to show report instead of table view
            List<ReceivingScale> info = service.GetReceivingByDates(start.Value, end.Value);

            ViewBag.Count = info.Count;
            ViewBag.ReportViewer = GenerateReceivingScaleReport(info);

            return View("ReceivingReport");//RedirectToAction("ReceivingByDate", new { start = start.Value, end = end.Value });
        }

        public ActionResult ReceivingByDate(DateTime start, DateTime end)
        {
            if (Session["Username"] == null)
                return RedirectToAction("NoPermission", "Login");

            TempData["RecLoc"] = "dates";
            TempData["RecDate1"] = start;
            TempData["RecDate2"] = end;

            return View(service.GetReceivingByDates(start, end));
        }

        public ActionResult ReceivingDetails(string palletNumber)
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Login");

            if (TempData["RecLoc"] != null)
            {
                if (TempData["RecLoc"].ToString() == "search")
                {
                    ViewBag.RecLoc = "Search";
                    ViewBag.SearchStr = TempData["RecSearchStr"].ToString();
                }
                else if (TempData["RecLoc"].ToString() == "dates")
                {
                    ViewBag.RecLoc = "Dates";
                    ViewBag.Start = Convert.ToDateTime(TempData["RecDate1"]);
                    ViewBag.End = Convert.ToDateTime(TempData["RecDate2"]);
                }
            }

            return View(service.GetReceivingDetails(palletNumber));
        }

        public ActionResult EditReceivingData(string palletNumber)
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Login");

            ViewBag.Suppliers = service.GetAllSuppliers();
            ViewBag.Products = service.GetProducts();
            ViewBag.TotesOrBins = new List<TotesOrBins>
            {
                new TotesOrBins{ID = "B", Name = "Blue Totes"},
                new TotesOrBins{ID = "S", Name = "Small Bins"}
            };

            return View(service.GetReceivingDetails(palletNumber));
        }

        [HttpPost]
        public ActionResult EditReceivingData(ReceivingScale info)
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Login");

            ViewBag.Suppliers = service.GetAllSuppliers();
            ViewBag.Products = service.GetProducts();
            ViewBag.TotesOrBins = new List<TotesOrBins>
            {
                new TotesOrBins{ID = "B", Name = "Blue Totes"},
                new TotesOrBins{ID = "S", Name = "Small Bins"}
            };

            if (info.QuantityTotesBins <= 0)
            {
                ViewBag.Msg = "Quantity of Totes/Bins cannot be 0 or less than 0!";
                return View(info);
            }
            if (!service.EditReceiving(info))
            {
                ViewBag.Msg = "There was a problem saving that record, please screenshot this entire page and submit a ticket at: https://advateksupport.azurewebsites.net/Home/";
                return View(info);
            }

            return RedirectToAction("ReceivingDetails", new { palletNumber = info.PalletNo });
        }

        public ActionResult DeleteReceivingData(string palletNumber)
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Login");

            service.DeleteReceivingData(palletNumber);

            return RedirectToAction("ManageReceiving");
        }
        #endregion

        #region Production
        public ActionResult ManageProduction()
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Login");

            return View();
        }

        public ActionResult FreshProduction()
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Login");

            return View(service.GetTodaysFreshProductionInformation());
        }

        [HttpPost]
        public ActionResult FreshProduction(string searchString)
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Login");

            return View(service.GetFreshProductionBySearchString(searchString));
        }

        public ActionResult SearchFreshProductionByDates()
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Login");

            return View();
        }

        [HttpPost]
        public ActionResult SearchFreshProductionByDates(DateTime? start, DateTime? end)
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Login");

            if (!start.HasValue)
            {
                ViewBag.Msg = "Start Date is required!";
                return View();
            }
            if (!end.HasValue)
            {
                ViewBag.Msg = "End Date is required!";
                return View();
            }
            if (start.Value > end.Value)
            {
                ViewBag.Msg = "Start Date cannot be ahead of End Date!";
                return View();
            }

            return RedirectToAction("ProductionByDate", new { start = start.Value, end = end.Value, freshOrFrozen = "Fresh" });
        }

        public ActionResult FrozenProduction()
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Login");

            return View(service.GetTodaysFrozenProductionInformation());
        }

        [HttpPost]
        public ActionResult FrozenProduction(string searchString)
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Login");

            return View(service.GetFrozenProductionBySearchString(searchString));
        }

        public ActionResult SearchFrozenProductionByDates()
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Login");

            return View();
        }

        [HttpPost]
        public ActionResult SearchFrozenProductionByDates(DateTime? start, DateTime? end)
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Login");

            if (!start.HasValue)
            {
                ViewBag.Msg = "Start Date is required!";
                return View();
            }
            if (!end.HasValue)
            {
                ViewBag.Msg = "End Date is required!";
                return View();
            }
            if (start.Value > end.Value)
            {
                ViewBag.Msg = "Start Date cannot be ahead of End Date!";
                return View();
            }

            return RedirectToAction("ProductionByDate", new { start = start.Value, end = end.Value, freshOrFrozen = "Frozen" });
        }

        public ActionResult ProductionByDate(DateTime start, DateTime end, string freshOrFrozen)
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Login");

            ViewBag.FreshOrFrozen = freshOrFrozen;

            return View(service.GetProductionByDates(start, end, freshOrFrozen));
        }

        public ActionResult ProductionDetails(string serialNumber)
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Login");

            return View(service.GetProductionDetails(serialNumber));
        }

        //public ActionResult EditProduction(string serialNumber)
        //{
        //    if (Session["Username"] == null)
        //        return RedirectToAction("Index", "Login");

        //    return View(service.GetProductionDetails(serialNumber));
        //}

        //[HttpPost]
        //public ActionResult EditProduction(Production info)
        //{
        //    if (Session["Username"] == null)
        //        return RedirectToAction("Index", "Login");

        //    //validation

        //    return View("ProductionDetails", new { serialNumber = info.SerialNo });
        //}

        public ActionResult DeleteProduction(string serialNumber)
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Login");

            service.DeleteProduction(serialNumber);

            return RedirectToAction("ManageProduction");
        }
        #endregion

        #region LotLookup
        public ActionResult LotLookup()
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Login");

            return View();
        }

        [HttpPost]
        public ActionResult LotLookup(string lotNo)
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Login");

            if (string.IsNullOrEmpty(lotNo) || string.IsNullOrWhiteSpace(lotNo) || lotNo.Length > 10)
            {
                ViewBag.Msg = "Lot Number is required and cannot be more than 10 characters.";
                return View();
            }

            return RedirectToAction("LotLookupDisplay", new { lotNo });
        }

        public ActionResult LotLookupDisplay(string lotNo)
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Login");

            List<ReceivingScale> receivingInfo = service.FindReceivingInfo(lotNo);
            decimal netWeight = 0.0m;
            decimal grossWeight = 0.0m;
            decimal tareWeight = 0.0m;

            foreach(ReceivingScale info in receivingInfo)
            {
                netWeight += info.Net;
                grossWeight += info.Gross;
                tareWeight += info.Tare;
            }

            ViewBag.Receiving = receivingInfo;
            ViewBag.Production = service.FindProductionInfo(lotNo);
            ViewBag.Gross = grossWeight;
            ViewBag.Tare = tareWeight;
            ViewBag.Net = netWeight;

            return View();
        }
        #endregion

        #region Inventory
        public ActionResult Inventory()
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Login");

            return View();
        }

        public ActionResult LoadSuppliers()
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Login");

            service.LoadSupplers();

            return RedirectToAction("SupplierList");
        }

        public ActionResult ManageInventory()
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Login");

            List<ReceivingScale> receiving = service.GetReceivingFrozenInventory();
            ViewBag.Receiving = receiving;
            ViewBag.ReceivingCount = receiving.Count();

            List<Production> production = service.GetProductionFrozenInventory();
            ViewBag.Production = production;
            ViewBag.ProductionCount = production.Count();

            List<Production> cases = service.GetCaseInventory();
            ViewBag.Cases = cases;
            ViewBag.CasesCount = cases.Count();

            return View();
        }

        public ActionResult ManageOutgoing()
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Login");

            List<ReceivingScale> receiving = service.GetOutgoingReceiving();
            ViewBag.Receiving = receiving;
            ViewBag.ReceivingCount = receiving.Count();

            List<Production> production = service.GetOutgoingProduction();
            ViewBag.Production = production;
            ViewBag.ProductionCount = production.Count();

            return View();
        }
        #endregion

        #region Setup
        public ActionResult SetupModule()
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Login");

            return View();
        }

        public ActionResult SupplierList()
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Login");

            return View(service.GetAllSuppliers());
        }

        public ActionResult EditSupplier(int id)
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Login");

            return View(service.GetSupplierByID(id));
        }

        [HttpPost]
        public ActionResult EditSupplier(Supplier supplier)
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Login");

            if (string.IsNullOrEmpty(supplier.Name) || string.IsNullOrWhiteSpace(supplier.Name))
            {
                ViewBag.Msg = "Supplier Name is required.";
                return View();
            }
            if (supplier.Name.Length > 200 || supplier.Name.Length <= 0)
            {
                ViewBag.Msg = "Supplier Name is not valid.";
                return View();
            }
            if (string.IsNullOrEmpty(supplier.Address))
            {
                supplier.Address = "";
            }
            if (supplier.Address != null && supplier.Address.Length > 200)
            {
                ViewBag.Msg = "Address is not valid.";
                return View();
            }
            if (!service.UpdateSupplier(supplier))
            {
                ViewBag.Msg = "There was a problem saving that supplier, please screenshot this entire page and submit a ticket at: https://advateksupport.azurewebsites.net/Home/";
                return View();
            }
            return RedirectToAction("SupplierList");
        }

        public ActionResult AddSupplier()
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Login");

            return View(new Supplier());
        }

        [HttpPost]
        public ActionResult AddSupplier(Supplier info)
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Login");

            if (string.IsNullOrEmpty(info.Name) || string.IsNullOrWhiteSpace(info.Name))
            {
                ViewBag.Msg = "Supplier Name is required.";
                return View(info);
            }
            if (info.Name.Length > 200 || info.Name.Length <= 0)
            {
                ViewBag.Msg = "Supplier Name is not valid.";
                return View(info);
            }
            if (string.IsNullOrEmpty(info.Address))
            {
                info.Address = "";
            }
            if (info.Address != null && info.Address.Length > 200)
            {
                ViewBag.Msg = "Address is not valid.";
                return View(info);
            }
            if (!service.AddSupplier(info))
            {
                ViewBag.Msg = "There was a problem creating that supplier, please screenshot this entire page and submit a ticket at: https://advateksupport.azurewebsites.net/Home/";
                return View(info);
            }

            return RedirectToAction("SupplierList");
        }

        public ActionResult EnableDisableSupplier(int supplierID, bool value)
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Login");

            service.EnableDisableSupplier(supplierID, value);

            return RedirectToAction("SupplierList");
        }

        public ActionResult CustomerList()
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Login");

            return View(service.GetAllCustomers());
        }

        public ActionResult EditCustomer(int id)
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Login");

            return View(service.GetCustomerByID(id));
        }

        [HttpPost]
        public ActionResult EditCustomer(Customer customer)
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Login");

            if (string.IsNullOrEmpty(customer.Name) || string.IsNullOrWhiteSpace(customer.Name))
            {
                ViewBag.Msg = "Customer Name is required.";
                return View();
            }
            if (customer.Name.Length > 200 || customer.Name.Length <= 0)
            {
                ViewBag.Msg = "Customer Name is not valid.";
                return View();
            }
            if (string.IsNullOrEmpty(customer.Address) || string.IsNullOrWhiteSpace(customer.Address))
            {
                ViewBag.Msg = "Customer Address is required.";
                return View();
            }
            if (customer.Address.Length > 200 || customer.Address.Length <= 0)
            {
                ViewBag.Msg = "Customer Address is not valid.";
                return View();
            }
            if (!service.UpdateCustomer(customer))
            {
                ViewBag.Msg = "There was a problem saving that customer, please screenshot this entire page and submit a ticket at: https://advateksupport.azurewebsites.net/Home/";
                return View();
            }
            return RedirectToAction("CustomerList");
        }

        public ActionResult CreateCustomer()
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Login");

            return View();
        }

        [HttpPost]
        public ActionResult CreateCustomer(Customer customer)
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Login");

            if (string.IsNullOrEmpty(customer.Name) || string.IsNullOrWhiteSpace(customer.Name) || customer.Name.Length > 200)
            {
                ViewBag.Msg = "Name is required and must be less than 200 characters.";
                return View();
            }
            if (string.IsNullOrEmpty(customer.Address) || string.IsNullOrWhiteSpace(customer.Address) || customer.Address.Length > 200)
            {
                ViewBag.Msg = "Address is required and must be less than 200 characters";
                return View();
            }
            if (!service.CreateCustomer(customer))
            {
                ViewBag.Msg = "There was a problem creating that customer, please screenshot this entire page and submit a ticket at: https://advateksupport.azurewebsites.net/Home/";
                return View();
            }
            return RedirectToAction("CustomerList");
        }

        public ActionResult ManageBoxSizes()
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Login");

            return View(service.GetBoxSizes());
        }

        public ActionResult CreateBoxSize()
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Login");

            return View(new BoxSize());
        }

        [HttpPost]
        public ActionResult CreateBoxSize(BoxSize info)
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Login");

            if (string.IsNullOrEmpty(info.BoxName) || string.IsNullOrWhiteSpace(info.BoxName) || info.BoxName.Length > 100)
            {
                ViewBag.Msg = "Box name is required and cannot be more than 100 characters!";
                return View();
            }
            if (info.BoxWeightLB <= 0)
            {
                ViewBag.Msg = "Weight in LBs cannot be 0 or less than 0!";
                return View();
            }
            if (info.BoxWeightKG <= 0)
            {
                ViewBag.Msg = "Weight in KGs cannot be 0 or less than 0!";
                return View();
            }
            if (info.BoxWeightLB < info.BoxWeightKG)
            {
                ViewBag.Msg = "Somethings not right, LBs can't be lower than KGs!";
                return View();
            }
            if (!service.AddBoxSize(info))
            {
                ViewBag.Msg = "There was a problem creating that box size, please screenshot this entire page and submit a ticket at: https://advateksupport.azurewebsites.net/Home/";
                return View();
            }

            return RedirectToAction("ManageBoxSizes");
        }

        public ActionResult EditBoxSize(int boxSizeID)
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Login");

            return View(service.GetBoxSize(boxSizeID));
        }

        [HttpPost]
        public ActionResult EditBoxSize(BoxSize info)
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Login");

            if (string.IsNullOrEmpty(info.BoxName) || string.IsNullOrWhiteSpace(info.BoxName) || info.BoxName.Length > 100)
            {
                ViewBag.Msg = "Box name is required and cannot be more than 100 characters!";
                return View();
            }
            if (info.BoxWeightLB <= 0)
            {
                ViewBag.Msg = "Weight in LBs cannot be 0 or less than 0!";
                return View();
            }
            if (info.BoxWeightKG <= 0)
            {
                ViewBag.Msg = "Weight in KGs cannot be 0 or less than 0!";
                return View();
            }
            if (info.BoxWeightLB < info.BoxWeightKG)
            {
                ViewBag.Msg = "Somethings not right, LBs can't be lower than KGs!";
                return View();
            }
            if (!service.EditBoxSize(info))
            {
                ViewBag.Msg = "There was a problem saving that box size, please screenshot this entire page and submit a ticket at: https://advateksupport.azurewebsites.net/Home/";
                return View();
            }

            return RedirectToAction("ManageBoxSizes");
        }

        public ActionResult ManageFreezers()
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Login");

            return View(service.GetFreezers());
        }

        public ActionResult CreateFreezer()
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Login");

            return View(new Freezer());
        }

        [HttpPost]
        public ActionResult CreateFreezer(Freezer info)
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Login");

            if (string.IsNullOrEmpty(info.FreezerName) || string.IsNullOrWhiteSpace(info.FreezerName) || info.FreezerName.Length > 100)
            {
                ViewBag.Msg = "Freezer Name is required and cannot be more than 100 characters!";
                return View();
            }
            if (!service.AddFreezer(info))
            {
                ViewBag.Msg = "There was a problem creating that freezer, please screenshot this entire page and submit a ticket at: https://advateksupport.azurewebsites.net/Home/";
                return View();
            }

            return RedirectToAction("ManageFreezers");
        }

        public ActionResult EditFreezer(int freezerID)
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Login");

            return View(service.GetFreezer(freezerID));
        }

        [HttpPost]
        public ActionResult EditFreezer(Freezer info)
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Login");

            if (string.IsNullOrEmpty(info.FreezerName) || string.IsNullOrWhiteSpace(info.FreezerName) || info.FreezerName.Length > 100)
            {
                ViewBag.Msg = "Freezer Name is required and cannot be more than 100 characters!";
                return View();
            }
            if (!service.EditFreezer(info))
            {
                ViewBag.Msg = "There was a problem saving that freezer, please screenshot this entire page and submit a ticket at: https://advateksupport.azurewebsites.net/Home/";
                return View();
            }

            return RedirectToAction("ManageFreezers");
        }

        public ActionResult ManageProducts()
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Login");

            return View(service.GetProducts());
        }

        public ActionResult CreateProduct()
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Login");

            return View(new Product());
        }

        [HttpPost]
        public ActionResult CreateProduct(Product info)
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Login");

            if (string.IsNullOrEmpty(info.EnglishTitle) || string.IsNullOrWhiteSpace(info.EnglishTitle) || info.EnglishTitle.Length > 100)
            {
                ViewBag.Msg = "English Name is required and cannot be more than 100 characters!";
                return View();
            }
            if (string.IsNullOrEmpty(info.FrenchTitle) || string.IsNullOrWhiteSpace(info.FrenchTitle) || info.FrenchTitle.Length > 100)
            {
                ViewBag.Msg = "French Name is required and cannot be more than 100 characters!";
                return View();
            }
            if (string.IsNullOrEmpty(info.CertLine1) || string.IsNullOrWhiteSpace(info.CertLine1) || info.CertLine1.Length > 100)
            {
                ViewBag.Msg = "Certification Line 1 is required and cannot be more than 100 characters!";
                return View();
            }
            if (string.IsNullOrEmpty(info.CertLine2) || string.IsNullOrWhiteSpace(info.CertLine2) || info.CertLine2.Length > 100)
            {
                ViewBag.Msg = "Certification Line 2 is required and cannot be more than 100 characters!";
                return View();
            }
            if (string.IsNullOrEmpty(info.ProductCode) || string.IsNullOrWhiteSpace(info.ProductCode) || info.ProductCode.Length > 10)
            {
                ViewBag.Msg = "Product Code is required and cannot be more than 10 characters!";
                return View();
            }
            if (!service.AddProduct(info))
            {
                ViewBag.Msg = "There was a problem creating that product, please screenshot this entire page and submit a ticket at: https://advateksupport.azurewebsites.net/Home/";
                return View();
            }

            return RedirectToAction("ManageProducts");
        }

        public ActionResult EditProduct(int productID)
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Login");

            return View(service.GetProduct(productID));
        }

        [HttpPost]
        public ActionResult EditProduct(Product info)
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Login");

            if (string.IsNullOrEmpty(info.EnglishTitle) || string.IsNullOrWhiteSpace(info.EnglishTitle) || info.EnglishTitle.Length > 100)
            {
                ViewBag.Msg = "English Name is required and cannot be more than 100 characters!";
                return View();
            }
            if (string.IsNullOrEmpty(info.FrenchTitle) || string.IsNullOrWhiteSpace(info.FrenchTitle) || info.FrenchTitle.Length > 100)
            {
                ViewBag.Msg = "French Name is required and cannot be more than 100 characters!";
                return View();
            }
            if (string.IsNullOrEmpty(info.CertLine1) || string.IsNullOrWhiteSpace(info.CertLine1) || info.CertLine1.Length > 100)
            {
                ViewBag.Msg = "Certification Line 1 is required and cannot be more than 100 characters!";
                return View();
            }
            if (string.IsNullOrEmpty(info.CertLine2) || string.IsNullOrWhiteSpace(info.CertLine2) || info.CertLine2.Length > 100)
            {
                ViewBag.Msg = "Certification Line 2 is required and cannot be more than 100 characters!";
                return View();
            }
            if (string.IsNullOrEmpty(info.ProductCode) || string.IsNullOrWhiteSpace(info.ProductCode) || info.ProductCode.Length > 10)
            {
                ViewBag.Msg = "Product Code is required and cannot be more than 10 characters!";
                return View();
            }
            if (!service.EditProduct(info))
            {
                ViewBag.Msg = "There was a problem saving that product, please screenshot this entire page and submit a ticket at: https://advateksupport.azurewebsites.net/Home/";
                return View();
            }

            return RedirectToAction("ManageProducts");
        }
        #endregion

        private ReportViewer GenerateReceivingScaleReport(List<ReceivingScale> receivingScaleInfo)
        {
            ReportViewer reportViewer = new ReportViewer
            {
                ProcessingMode = ProcessingMode.Local,
                AsyncRendering = false,
                SizeToReportContent = true
            };

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Reports\ReceivingScaleReport.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ReceivingScaleInfo", receivingScaleInfo));

            return reportViewer;
        }
    }
}