using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Types;

namespace Repository
{
    public class Repo
    {
        private readonly DataAccess db;

        public Repo()
        {
            db = new DataAccess();
        }

        #region Receiving
        public List<ReceivingScale> GetReceivingBySupplierAndLot(int supplierID, string lotNo)
        {
            List<ReceivingScale> info = new List<ReceivingScale>();

            List<ParmStruct> parms = new List<ParmStruct>()
            {
                new ParmStruct("@SupplierID", SqlDbType.Int, 0, supplierID, ParameterDirection.Input),
                new ParmStruct("@LotNo", SqlDbType.VarChar, 50, lotNo, ParameterDirection.Input)
            };

            DataTable dt = db.GetData("spGetReceivingScaleInfoBySupplierAndLot", parms);

            foreach (DataRow row in dt.Rows)
            {
                info.Add(PopulateReceivingScaleObj(row));
            }

            return info;
        }

        public List<ReceivingScale> GetReceivingBySupplierAndYear(int supplierID, string year)
        {
            List<ReceivingScale> info = new List<ReceivingScale>();

            List<ParmStruct> parms = new List<ParmStruct>()
            {
                new ParmStruct("@SupplierID", SqlDbType.Int, 0, supplierID, ParameterDirection.Input),
                new ParmStruct("@Year", SqlDbType.VarChar, 4, year, ParameterDirection.Input)
            };

            DataTable dt = db.GetData("spGetReceivingScaleInfoBySupplierAndYear", parms);

            foreach (DataRow row in dt.Rows)
            {
                info.Add(PopulateReceivingScaleObj(row));
            }

            return info;
        }

        public List<ReceivingScale> GetTodaysReceivingScaleInfo()
        {
            List<ReceivingScale> info = new List<ReceivingScale>();

            DataTable dt = db.GetData("spGetReceivingScaleInfo");

            foreach(DataRow row in dt.Rows)
            {
                info.Add(PopulateReceivingScaleObj(row));
            }

            return info;
        }

        public List<ReceivingScale> GetReceivingScaleInfoBySearch(string searchString)
        {
            List<ReceivingScale> info = new List<ReceivingScale>();

            List<ParmStruct> parms = new List<ParmStruct>()
            {
                new ParmStruct("@SearchString", SqlDbType.VarChar, 0, searchString, ParameterDirection.Input)
            };

            DataTable dt = db.GetData("spGetReceivingScaleInfoBySearchString", parms);

            foreach (DataRow row in dt.Rows)
            {
                info.Add(PopulateReceivingScaleObj(row));
            }

            return info;
        }

        public ReceivingScale GetReceivingDetails(string palletNumber)
        {
            List<ParmStruct> parms = new List<ParmStruct>()
            {
                new ParmStruct("@PalletNumber", SqlDbType.VarChar, 20, palletNumber, ParameterDirection.Input)
            };

            DataTable dt = db.GetData("spGetReceivingDetails", parms);

            return PopulateReceivingScaleObj(dt.Rows[0]);
        }

        public bool DeleteReceivingData(string palletNumber)
        {
            List<ParmStruct> parms = new List<ParmStruct>()
            {
                new ParmStruct("@PalletNumber", SqlDbType.VarChar, 20, palletNumber, ParameterDirection.Input)
            };

            return db.SendData("spDeleteReceivingData", parms) > 0;
        }

        public List<ReceivingScale> GetReceivingByDates(DateTime start, DateTime end)
        {
            List<ReceivingScale> info = new List<ReceivingScale>();

            List<ParmStruct> parms = new List<ParmStruct>()
            {
                new ParmStruct("@StartDate", SqlDbType.DateTime, 0, start, ParameterDirection.Input),
                new ParmStruct("@EndDate", SqlDbType.DateTime, 0, end, ParameterDirection.Input)
            };

            DataTable dt = db.GetData("spGetReceivingScaleInfoBetweenDates", parms);

            foreach (DataRow row in dt.Rows)
            {
                info.Add(PopulateReceivingScaleObj(row));
            }

            return info;
        }

        public bool EditReceiving(ReceivingScale info)
        {
            List<ParmStruct> parms = new List<ParmStruct>()
            {
                new ParmStruct("@PalletNumber", SqlDbType.VarChar, 20, info.PalletNo, ParameterDirection.Input),
                new ParmStruct("@SupplierID", SqlDbType.Int, 0, info.SupplierID, ParameterDirection.Input),
                new ParmStruct("@ProductID", SqlDbType.Int, 0, info.ProductID, ParameterDirection.Input),
                new ParmStruct("@BlueTotesOrSmallBins", SqlDbType.VarChar, 1, info.BlueTotesOrSmallBins, ParameterDirection.Input),
                new ParmStruct("@QuantityTotesBins", SqlDbType.Int, 0, info.QuantityTotesBins, ParameterDirection.Input),
            };

            return db.SendData("spEditReceivingData", parms) > 0;
        }

        private static ReceivingScale PopulateReceivingScaleObj(DataRow row)
        {
            return new ReceivingScale
            {
                DeviceID = Convert.ToInt32(row["DeviceID"]),
                LotNo = row["LotNo"].ToString(),
                PalletNo = row["PalletNo"].ToString(),
                SupplierID = Convert.ToInt32(row["SupplierID"]),
                SupplierName = row["Name"].ToString(),
                ProductID = Convert.ToInt32(row["ProductID"]),
                ProductName = row["EnglishTitle"].ToString(),
                Gross = Convert.ToDecimal(row["Gross"]),
                Tare = Convert.ToDecimal(row["Tare"]),
                Net = Convert.ToDecimal(row["Net"]),
                BlueTotesOrSmallBins = row["BlueTotesOrSmallBin"].ToString(),
                QuantityTotesBins = Convert.ToInt32(row["QuantityTotesBins"]),
                Status = Convert.ToInt32(row["Status"]),
                RecDateTime = Convert.ToDateTime(row["RecDateTime"]),
                InsDateTime = Convert.ToDateTime(row["InsDateTime"]),
                PalletWeight = Convert.ToDecimal(row["PalletWeight"]),
                StateCode = Convert.ToInt32(row["StateCode"]),
                StateCodeName = row["StateCode_Info"].ToString()
            };
        }
        #endregion

        #region Production
        public List<Production> GetTodaysFreshProductionInformation()
        {
            List<Production> info = new List<Production>();

            DataTable dt = db.GetData("spGetTodaysFreshProductionInfo");

            foreach(DataRow row in dt.Rows)
            {
                info.Add(PopulateProductionObj(row));
            }

            return info;
        }

        public List<Production> GetTodaysFrozenProductionInformation()
        {
            List<Production> info = new List<Production>();

            DataTable dt = db.GetData("spGetTodaysFrozenProductionInfo");

            foreach (DataRow row in dt.Rows)
            {
                info.Add(PopulateProductionObj(row));
            }

            return info;
        }

        public List<Production> GetFreshProductionBySearchString(string searchString)
        {
            List<ParmStruct> parms = new List<ParmStruct>()
            {
                new ParmStruct("@SearchString", SqlDbType.VarChar, 0, searchString, ParameterDirection.Input)
            };

            List<Production> info = new List<Production>();

            DataTable dt = db.GetData("spGetFreshProductionBySearchString", parms);

            foreach (DataRow row in dt.Rows)
            {
                info.Add(PopulateProductionObj(row));
            }

            return info;
        }

        public List<Production> GetFrozenProductionBySearchString(string searchString)
        {
            List<ParmStruct> parms = new List<ParmStruct>()
            {
                new ParmStruct("@SearchString", SqlDbType.VarChar, 0, searchString, ParameterDirection.Input)
            };

            List<Production> info = new List<Production>();

            DataTable dt = db.GetData("spGetFrozenProductionBySearchString", parms);

            foreach (DataRow row in dt.Rows)
            {
                info.Add(PopulateProductionObj(row));
            }

            return info;
        }

        public List<Production> GetProductionByDates(DateTime start, DateTime end, string freshOrFrozen)
        {
            List<ParmStruct> parms = new List<ParmStruct>()
            {
                new ParmStruct("@StartDate", SqlDbType.DateTime, 0, start, ParameterDirection.Input),
                new ParmStruct("@EndDate", SqlDbType.DateTime, 0, end, ParameterDirection.Input),
                new ParmStruct("@FreshOrFrozen", SqlDbType.VarChar, 50, freshOrFrozen, ParameterDirection.Input)
            };

            List<Production> info = new List<Production>();

            DataTable dt = db.GetData("spGetProductionByDates", parms);

            foreach (DataRow row in dt.Rows)
            {
                info.Add(PopulateProductionObj(row));
            }

            return info;
        }

        public Production GetProductionDetails(string serialNo)
        {
            List<ParmStruct> parms = new List<ParmStruct>()
            {
                new ParmStruct("@SerialNo", SqlDbType.VarChar, 20, serialNo, ParameterDirection.Input)
            };

            List<Production> info = new List<Production>();

            DataTable dt = db.GetData("spGetProductionDetails", parms);

            return PopulateProductionObj(dt.Rows[0]);
        }

        public bool DeleteProduction(string serialNo)
        {
            List<ParmStruct> parms = new List<ParmStruct>()
            {
                new ParmStruct("@SerialNumber", SqlDbType.VarChar, 20, serialNo, ParameterDirection.Input)
            };

            return db.SendData("spDeleteProduction", parms) > 0;
        }

        private static Production PopulateProductionObj(DataRow row)
        {
            return new Production
            {
                DeviceID = Convert.ToInt32(row["DeviceID"]),
                LotNo = row["LotNo"].ToString(),
                SerialNo = row["SerialNo"].ToString(),
                FullSkidNo = row["FullSkidNo"].ToString(),
                Gross = Convert.ToDecimal(row["Gross"]),
                Tare = Convert.ToDecimal(row["Tare"]),
                Net = Convert.ToDecimal(row["Net"]),
                MyTarget = Convert.ToDecimal(row["MyTarget"]),
                Unit = row["Unit"].ToString(),
                RecDateTime = Convert.ToDateTime(row["RecDateTime"]),
                InsDateTime = Convert.ToDateTime(row["InsDateTime"]),
                StateCode = Convert.ToInt32(row["StateCode"]),
                StateCodeName = row["StateCodeName"].ToString(),
                CustomerID = row.IsNull("CustomerID") ? 0 : Convert.ToInt32(row["CustomerID"]),
                CustomerName = row["CustomerName"].ToString(),
                BoxSizeID = row.IsNull("BoxSizeID") ? 0 : Convert.ToInt32(row["BoxSizeID"]),
                BoxName = row["BoxName"].ToString(),
                BoxWeightLB = row.IsNull("BoxWeightLB") ? 0.00m : Convert.ToDecimal(row["BoxWeightLB"])
            };
        }
        #endregion

        #region LotLookup
        public List<ReceivingScale> FindReceivingInfo(string lotNo)
        {
            List<ReceivingScale> info = new List<ReceivingScale>();

            List<ParmStruct> parms = new List<ParmStruct>
            {
                new ParmStruct("@LotNo", SqlDbType.VarChar, 10, lotNo, ParameterDirection.Input)
            };

            DataTable dt = db.GetData("spFindReceivingInfo", parms);

            foreach(DataRow row in dt.Rows)
            {
                info.Add(
                        new ReceivingScale
                        {
                            DeviceID = Convert.ToInt32(row["DeviceID"]),
                            LotNo = row["LotNo"].ToString(),
                            PalletNo = row["PalletNo"].ToString(),
                            SupplierName = row["Name"].ToString(),
                            ProductName = row["EnglishTitle"].ToString(),
                            Gross = Convert.ToDecimal(row["Gross"]),
                            Tare = Convert.ToDecimal(row["Tare"]),
                            Net = Convert.ToDecimal(row["Net"]),
                            BlueTotesOrSmallBins = row["BlueTotesOrSmallBin"].ToString(),
                            QuantityTotesBins = Convert.ToInt32(row["QuantityTotesBins"]),
                            RecDateTime = Convert.ToDateTime(row["RecDateTime"]),
                            StateCodeName = row["StateCode_Info"].ToString()
                        }
                    );
            }

            return info;
        }

        public List<Production> FindProductionInfo(string lotNo)
        {
            List<Production> info = new List<Production>();

            List<ParmStruct> parms = new List<ParmStruct>
            {
                new ParmStruct("@LotNo", SqlDbType.VarChar, 10, lotNo, ParameterDirection.Input)
            };

            DataTable dt = db.GetData("spFindProductionInfo", parms);

            foreach (DataRow row in dt.Rows)
            {
                info.Add(
                        new Production
                        {
                            DeviceID = Convert.ToInt32(row["DeviceID"]),
                            LotNo = row["LotNo"].ToString(),
                            SerialNo = row["SerialNo"].ToString(),
                            FullSkidNo = row["FullSkidNo"].ToString(),
                            RecDateTime = Convert.ToDateTime(row["RecDateTime"]),
                            StateCodeName = row["StateCode_Info"].ToString(),
                            Name = row.IsNull("Name") ? "" : row["Name"].ToString(),
                            TrailerNo = row.IsNull("TrailerNo") ? "" : row["TrailerNo"].ToString()
                        }
                    );
            }

            return info;
        }
        #endregion

        #region Inventory
        public List<ReceivingScale> GetReceivingFrozenInventory()
        {
            List<ReceivingScale> info = new List<ReceivingScale>();

            DataTable dt = db.GetData("spGetReceivingFrozenInventory");

            foreach (DataRow row in dt.Rows)
            {
                info.Add(
                        new ReceivingScale
                        {
                            PalletNo = row["PalletNo"].ToString(),
                            FreezerID = Convert.ToInt32(row["FreezerID"]),
                            LotNo = row["LotNo"].ToString(),
                            RecDateTime = Convert.ToDateTime(row["RecDateTime"])
                        }
                    );
            }

            return info;
        }

        public List<Production> GetProductionFrozenInventory()
        {
            List<Production> info = new List<Production>();

            DataTable dt = db.GetData("spGetProductionFrozenInventory");

            foreach(DataRow row in dt.Rows)
            {
                info.Add(
                        new Production
                        {
                            FullSkidNo = row["FullSkidNo"].ToString(),
                            FreezerID = Convert.ToInt32(row["FreezerID"]),
                            LotNo = row["LotNo"].ToString(),
                            RecDateTime = Convert.ToDateTime(row["RecDateTime"])
                        }
                    );
            }

            return info;
        }

        public List<ReceivingScale> GetOutgoingReceiving()
        {
            List<ReceivingScale> info = new List<ReceivingScale>();

            DataTable dt = db.GetData("spGetOutgoingReceiving");

            foreach(DataRow row in dt.Rows)
            {
                info.Add(
                    new ReceivingScale
                    {
                        LotNo = row["LotNo"].ToString(),
                        PalletNo = row["PalletNo"].ToString(),
                        TrailerNo = row["TrailerNo"].ToString(),
                        ProductName = row["EnglishTitle"].ToString(),
                        BlueTotesOrSmallBins = row["BlueTotesOrSmallBin"].ToString(),
                        QuantityTotesBins = Convert.ToInt32(row["QuantityTotesBins"]),
                        StateCodeName = row["StateCode_Info"].ToString(),
                        DisplayDate = Convert.ToDateTime(row["date"]).ToString("yyyy-MM-dd")
                    }
                    );
            }

            return info;
        }

        public List<Production> GetOutgoingProduction()
        {
            List<Production> info = new List<Production>();

            DataTable dt = db.GetData("spGetOutgoingProduction");

            foreach(DataRow row in dt.Rows)
            {
                info.Add(
                        new Production
                        {
                            FullSkidNo = row["FullSkidNo"].ToString(),
                            CustomerName = row["Name"].ToString(),
                            TrailerNo = row["TrailerNo"].ToString(),
                            DisplayDate = Convert.ToDateTime(row["date"]).ToString("yyyy-MM-dd")
                        }
                    );
            }

            return info;
        }

        public List<Production> GetCaseInventory()
        {
            List<Production> info = new List<Production>();

            DataTable dt = db.GetData("spGetCaseInventory");

            foreach(DataRow row in dt.Rows)
            {
                info.Add(
                        new Production
                        {
                            LotNo = row["LotNo"].ToString(),
                            SerialNo = row["SerialNo"].ToString(),
                            FreshOrFrozen = row["FreshOrFrozen"].ToString(),
                            RecDateTime = Convert.ToDateTime(row["RecDateTime"])
                        }
                    );
            }

            return info;
        }
        #endregion

        public void LoadAll()
        {
            List<ParmStruct> parms = new List<ParmStruct>
            {
                new ParmStruct("@p_ID", SqlDbType.Int, 0, 0, ParameterDirection.Input),
                new ParmStruct("@p_DeviceID", SqlDbType.Int, 0, 120, ParameterDirection.Input)
            };

            db.SendDataLocal("adv_LoadSupplier", parms);
        }

        #region Setup
        public bool EnableDisableSupplier(int supplierID, bool value)
        {
            List<ParmStruct> parms = new List<ParmStruct>()
            {
                new ParmStruct("@SupplierID", SqlDbType.Int, 0, supplierID, ParameterDirection.Input),
                new ParmStruct("@Value", SqlDbType.Bit, 0, value, ParameterDirection.Input)
            };

            return db.SendData("spEnableDisableSupplier", parms) > 0;
        }

        public List<Supplier> GetAllSuppliers()
        {
            List<Supplier> suppliers = new List<Supplier>();

            DataTable dt = db.GetData("spGetSuppliers");

            foreach (DataRow row in dt.Rows)
            {
                suppliers.Add(
                        new Supplier
                        {
                            SupplierID = Convert.ToInt32(row["SupplierID"]),
                            Name = row["Name"].ToString(),
                            Address = row["Address"].ToString(),
                            EstNo = row["EstNo"].ToString(),
                            Enabled = Convert.ToBoolean(row["Enabled"])
                        }
                    );
            }

            return suppliers;
        }

        public Supplier GetSupplierByID(int id)
        {
            List<ParmStruct> parms = new List<ParmStruct>
            {
                new ParmStruct("@SupplierID", SqlDbType.Int, 0, id, ParameterDirection.Input)
            };

            DataTable dt = db.GetData("spGetSupplierByID", parms);

            return new Supplier
            {
                SupplierID = Convert.ToInt32(dt.Rows[0]["SupplierID"]),
                Name = dt.Rows[0]["Name"].ToString(),
                Address = dt.Rows[0]["Address"].ToString(),
                EstNo = dt.Rows[0]["EstNo"].ToString(),
                Enabled = Convert.ToBoolean(dt.Rows[0]["Enabled"])
            };
        }

        public bool UpdateSupplier(Supplier supplier)
        {
            List<ParmStruct> parms = new List<ParmStruct>
            {
                new ParmStruct("@SupplierID", SqlDbType.Int, 0, supplier.SupplierID, ParameterDirection.Input),
                new ParmStruct("@Name", SqlDbType.VarChar, 200, supplier.Name, ParameterDirection.Input),
                new ParmStruct("@Address", SqlDbType.VarChar, 200, supplier.Address, ParameterDirection.Input)
            };

            return db.SendData("spUpdateSupplier", parms) > 0;
        }

        public bool AddSupplier(Supplier supplier)
        {
            List<ParmStruct> parms = new List<ParmStruct>
            {
                new ParmStruct("@Name", SqlDbType.VarChar, 200, supplier.Name, ParameterDirection.Input),
                new ParmStruct("@Address", SqlDbType.VarChar, 200, supplier.Address, ParameterDirection.Input)
            };

            return db.SendData("spAddSupplier", parms) > 0;
        }

        public List<Customer> GetAllCustomers()
        {
            List<Customer> customers = new List<Customer>();

            DataTable dt = db.GetData("spGetCustomers");

            foreach (DataRow row in dt.Rows)
            {
                customers.Add(
                        new Customer
                        {
                            CustomerID = Convert.ToInt32(row["CustomerID"]),
                            Name = row["Name"].ToString(),
                            Address = row["Address"].ToString()
                        }
                    );
            }

            return customers;
        }

        public Customer GetCustomerByID(int id)
        {
            List<ParmStruct> parms = new List<ParmStruct>
            {
                new ParmStruct("@CustomerID", SqlDbType.Int, 0, id, ParameterDirection.Input)
            };

            DataTable dt = db.GetData("spGetCustomerByID", parms);

            return new Customer
            {
                CustomerID = Convert.ToInt32(dt.Rows[0]["CustomerID"]),
                Name = dt.Rows[0]["Name"].ToString(),
                Address = dt.Rows[0]["Address"].ToString()
            };
        }

        public bool UpdateCustomer(Customer customer)
        {
            List<ParmStruct> parms = new List<ParmStruct>
            {
                new ParmStruct("@CustomerID", SqlDbType.Int, 0, customer.CustomerID, ParameterDirection.Input),
                new ParmStruct("@Name", SqlDbType.VarChar, 200, customer.Name, ParameterDirection.Input),
                new ParmStruct("@Address", SqlDbType.VarChar, 200, customer.Address, ParameterDirection.Input)
            };

            return db.SendData("spUpdateCustomer", parms) > 0;
        }

        public bool CreateCustomer(Customer customer)
        {
            List<ParmStruct> parms = new List<ParmStruct>
            {
                new ParmStruct("@Name", SqlDbType.VarChar, 200, customer.Name, ParameterDirection.Input),
                new ParmStruct("@Address", SqlDbType.VarChar, 200, customer.Address, ParameterDirection.Input)
            };

            return db.SendData("spCreateCustomer", parms) > 0;
        }

        public List<BoxSize> GetBoxSizes()
        {
            List<BoxSize> info = new List<BoxSize>();

            DataTable dt = db.GetData("spGetBoxSizes");

            foreach (DataRow row in dt.Rows)
            {
                info.Add(PopulateBoxSizeObj(row));
            }

            return info;
        }

        public BoxSize GetBoxSize(int boxSizeID)
        {
            List<ParmStruct> parms = new List<ParmStruct>
            {
                new ParmStruct("@BoxSizeID", SqlDbType.Int, 0, boxSizeID, ParameterDirection.Input)
            };

            DataTable dt = db.GetData("spGetBoxSize", parms);

            return PopulateBoxSizeObj(dt.Rows[0]);
        }

        public bool AddBoxSize(BoxSize info)
        {
            List<ParmStruct> parms = new List<ParmStruct>
            {
                new ParmStruct("@BoxName", SqlDbType.VarChar, 100, info.BoxName, ParameterDirection.Input),
                new ParmStruct("@BoxWeightLB", SqlDbType.Decimal, 0, info.BoxWeightLB, ParameterDirection.Input),
                new ParmStruct("@BoxWeightKG", SqlDbType.Decimal, 0, info.BoxWeightKG, ParameterDirection.Input),
            };

            return db.SendData("spAddBoxSize", parms) > 0;
        }

        public bool EditBoxSize(BoxSize info)
        {
            List<ParmStruct> parms = new List<ParmStruct>
            {
                new ParmStruct("@BoxSizeID", SqlDbType.Int, 0, info.BoxSizeID, ParameterDirection.Input),
                new ParmStruct("@BoxName", SqlDbType.VarChar, 100, info.BoxName, ParameterDirection.Input),
                new ParmStruct("@BoxWeightLB", SqlDbType.Decimal, 0, info.BoxWeightLB, ParameterDirection.Input),
                new ParmStruct("@BoxWeightKG", SqlDbType.Decimal, 0, info.BoxWeightKG, ParameterDirection.Input),
            };

            return db.SendData("spEditBoxSize", parms) > 0;
        }

        public List<Freezer> GetFreezers()
        {
            List<Freezer> info = new List<Freezer>();

            DataTable dt = db.GetData("spGetFreezers");

            foreach (DataRow row in dt.Rows)
            {
                info.Add(PopulateFreezerObj(row));
            }

            return info;
        }

        public Freezer GetFreezer(int freezerID)
        {
            List<ParmStruct> parms = new List<ParmStruct>
            {
                new ParmStruct("@FreezerID", SqlDbType.Int, 0, freezerID, ParameterDirection.Input)
            };

            DataTable dt = db.GetData("spGetFreezer", parms);

            return PopulateFreezerObj(dt.Rows[0]);
        }

        public bool AddFreezer(Freezer info)
        {
            List<ParmStruct> parms = new List<ParmStruct>
            {
                new ParmStruct("@FreezerName", SqlDbType.VarChar, 100, info.FreezerName, ParameterDirection.Input)
            };

            return db.SendData("spAddFreezer", parms) > 0;
        }

        public bool EditFreezer(Freezer info)
        {
            List<ParmStruct> parms = new List<ParmStruct>
            {
                new ParmStruct("@FreezerID", SqlDbType.Int, 0, info.FreezerID, ParameterDirection.Input),
                new ParmStruct("@FreezerName", SqlDbType.VarChar, 100, info.FreezerName, ParameterDirection.Input)
            };

            return db.SendData("spEditFreezer", parms) > 0;
        }

        public List<Product> GetProducts()
        {
            List<Product> info = new List<Product>();

            DataTable dt = db.GetData("spGetProducts");

            foreach (DataRow row in dt.Rows)
            {
                info.Add(PopulateProductObj(row));
            }

            return info;
        }

        public Product GetProduct(int productID)
        {
            List<ParmStruct> parms = new List<ParmStruct>
            {
                new ParmStruct("@ProductID", SqlDbType.Int, 0, productID, ParameterDirection.Input)
            };

            DataTable dt = db.GetData("spGetProduct", parms);

            return PopulateProductObj(dt.Rows[0]);
        }

        public bool AddProduct(Product info)
        {
            List<ParmStruct> parms = new List<ParmStruct>
            {
                new ParmStruct("@EnglishTitle", SqlDbType.VarChar, 100, info.EnglishTitle, ParameterDirection.Input),
                new ParmStruct("@FrenchTitle", SqlDbType.VarChar, 100, info.FrenchTitle, ParameterDirection.Input),
                new ParmStruct("@CertLine1", SqlDbType.VarChar, 100, info.CertLine1, ParameterDirection.Input),
                new ParmStruct("@CertLine2", SqlDbType.VarChar, 100, info.CertLine2, ParameterDirection.Input),
                new ParmStruct("@ProductCode", SqlDbType.VarChar, 10, info.ProductCode, ParameterDirection.Input)
            };

            return db.SendData("spAddProduct", parms) > 0;
        }

        public bool EditProduct(Product info)
        {
            List<ParmStruct> parms = new List<ParmStruct>
            {
                new ParmStruct("@ProductID", SqlDbType.Int, 0, info.ProductID, ParameterDirection.Input),
                new ParmStruct("@EnglishTitle", SqlDbType.VarChar, 100, info.EnglishTitle, ParameterDirection.Input),
                new ParmStruct("@FrenchTitle", SqlDbType.VarChar, 100, info.FrenchTitle, ParameterDirection.Input),
                new ParmStruct("@CertLine1", SqlDbType.VarChar, 100, info.CertLine1, ParameterDirection.Input),
                new ParmStruct("@CertLine2", SqlDbType.VarChar, 100, info.CertLine2, ParameterDirection.Input),
                new ParmStruct("@ProductCode", SqlDbType.VarChar, 10, info.ProductCode, ParameterDirection.Input)
            };

            return db.SendData("spEditProduct", parms) > 0;
        }

        private static BoxSize PopulateBoxSizeObj(DataRow row)
        {
            return new BoxSize
            {
                BoxSizeID = Convert.ToInt32(row["BoxSizeID"]),
                BoxName = row["BoxName"].ToString(),
                BoxWeightLB = Convert.ToDecimal(row["BoxWeightLB"]),
                BoxWeightKG = Convert.ToDecimal(row["BoxWeightKG"]),
                Enabled = Convert.ToBoolean(row["Enabled"])
            };
        }

        private static Freezer PopulateFreezerObj(DataRow row)
        {
            return new Freezer
            {
                FreezerID = Convert.ToInt32(row["FreezerID"]),
                FreezerName = row["FreezerName"].ToString()
            };
        }

        private static Product PopulateProductObj(DataRow row)
        {
            return new Product
            {
                ProductID = Convert.ToInt32(row["ProductID"]),
                EnglishTitle = row["EnglishTitle"].ToString(),
                FrenchTitle = row["FrenchTitle"].ToString(),
                CertLine1 = row["CertLine1"].ToString(),
                CertLine2 = row["CertLine2"].ToString(),
                ProductCode = row["ProductCode"].ToString(),
                Enabled = Convert.ToBoolean(row["Enabled"])
            };
        }
        #endregion
    }
}