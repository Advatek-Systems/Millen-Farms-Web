using Model;
using Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Types;

namespace Service
{
    public class AppService
    {
        private readonly Repo repo;

        public AppService()
        {
            repo = new Repo();
        }

        #region Receiving
        public List<ReceivingScale> GetReceivingBySupplierAndLot(int supplierID, string lotNo)
        {
            return repo.GetReceivingBySupplierAndLot(supplierID, lotNo);
        }

        public List<ReceivingScale> GetReceivingBySupplierAndYear(int supplierID, string year)
        {
            return repo.GetReceivingBySupplierAndYear(supplierID, year);
        }

        public List<ReceivingScale> GetTodaysReceivingScaleInfo()
        {
            return repo.GetTodaysReceivingScaleInfo();
        }

        public List<ReceivingScale> GetReceivingScaleInfoBySearch(string searchString)
        {
            return repo.GetReceivingScaleInfoBySearch(searchString);
        }

        public ReceivingScale GetReceivingDetails(string palletNumber)
        {
            return repo.GetReceivingDetails(palletNumber);
        }

        public bool DeleteReceivingData(string palletNumber)
        {
            return repo.DeleteReceivingData(palletNumber);
        }

        public List<ReceivingScale> GetReceivingByDates(DateTime start, DateTime end)
        {
            return repo.GetReceivingByDates(start, end);
        }

        public bool EditReceiving(ReceivingScale info)
        {
            return repo.EditReceiving(info);
        }
        #endregion

        #region Production
        public List<Production> GetTodaysFreshProductionInformation()
        {
            return repo.GetTodaysFreshProductionInformation();
        }

        public List<Production> GetTodaysFrozenProductionInformation()
        {
            return repo.GetTodaysFrozenProductionInformation();
        }

        public List<Production> GetFreshProductionBySearchString(string searchString)
        {
            return repo.GetFreshProductionBySearchString(searchString);
        }

        public List<Production> GetFrozenProductionBySearchString(string searchString)
        {
            return repo.GetFrozenProductionBySearchString(searchString);
        }

        public List<Production> GetProductionByDates(DateTime start, DateTime end, string freshOrFrozen)
        {
            return repo.GetProductionByDates(start, end, freshOrFrozen);
        }

        public bool DeleteProduction(string serialNo)
        {
            return repo.DeleteProduction(serialNo);
        }
        #endregion

        #region LotLookup
        public List<ReceivingScale> FindReceivingInfo(string lotNo)
        {
            return repo.FindReceivingInfo(lotNo);
        }

        public List<Production> FindProductionInfo(string lotNo)
        {
            return repo.FindProductionInfo(lotNo);
        }
        #endregion

        #region Inventory
        public List<Production> GetCaseInventory()
        {
            return repo.GetCaseInventory();
        }

        public List<ReceivingScale> GetReceivingFrozenInventory()
        {
            return repo.GetReceivingFrozenInventory();
        }

        public List<Production> GetProductionFrozenInventory()
        {
            return repo.GetProductionFrozenInventory();
        }

        public Production GetProductionDetails(string serialNo)
        {
            return repo.GetProductionDetails(serialNo);
        }

        public List<ReceivingScale> GetOutgoingReceiving()
        {
            return repo.GetOutgoingReceiving();
        }

        public List<Production> GetOutgoingProduction()
        {
            return repo.GetOutgoingProduction();
        }
        #endregion

        public void LoadSupplers()
        {
            repo.LoadAll();
        }

        #region Setup
        public bool EnableDisableSupplier(int supplierID, bool value)
        {
            return repo.EnableDisableSupplier(supplierID, value);
        }

        public List<Supplier> GetAllSuppliers()
        {
            return repo.GetAllSuppliers();
        }

        public Supplier GetSupplierByID(int id)
        {
            return repo.GetSupplierByID(id);
        }

        public bool UpdateSupplier(Supplier supplier)
        {
            return repo.UpdateSupplier(supplier);
        }

        public bool AddSupplier(Supplier supplier)
        {
            return repo.AddSupplier(supplier);
        }

        public List<Customer> GetAllCustomers()
        {
            return repo.GetAllCustomers();
        }

        public Customer GetCustomerByID(int id)
        {
            return repo.GetCustomerByID(id);
        }

        public bool UpdateCustomer(Customer customer)
        {
            return repo.UpdateCustomer(customer);
        }

        public bool CreateCustomer(Customer customer)
        {
            return repo.CreateCustomer(customer);
        }

        public List<BoxSize> GetBoxSizes()
        {
            return repo.GetBoxSizes();
        }

        public BoxSize GetBoxSize(int boxSizeID)
        {
            return repo.GetBoxSize(boxSizeID);
        }

        public bool AddBoxSize(BoxSize info)
        {
            return repo.AddBoxSize(info);
        }

        public bool EditBoxSize(BoxSize info)
        {
            return repo.EditBoxSize(info);
        }

        public List<Freezer> GetFreezers()
        {
            return repo.GetFreezers();
        }

        public Freezer GetFreezer(int freezerID)
        {
            return repo.GetFreezer(freezerID);
        }

        public bool AddFreezer(Freezer info)
        {
            return repo.AddFreezer(info);
        }

        public bool EditFreezer(Freezer info)
        {
            return repo.EditFreezer(info);
        }

        public List<Product> GetProducts()
        {
            return repo.GetProducts();
        }

        public Product GetProduct(int productID)
        {
            return repo.GetProduct(productID);
        }

        public bool AddProduct(Product info)
        {
            return repo.AddProduct(info);
        }

        public bool EditProduct(Product info)
        {
            return repo.EditProduct(info);
        }
        #endregion
    }
}
