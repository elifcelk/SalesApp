using Microsoft.AspNetCore.Mvc;
using SalesApp.DataAccess;
using SalesApp.Services;

namespace SalesApp.Controllers
{
    public class SalesController : Controller
    {
        private readonly SalesService _salesService;
        private readonly ProductService _productService;
        private readonly StoreService _storeService;
        private readonly ICsvDataProvider _csvDataProvider;

        public SalesController(ICsvDataProvider csvDataProvider)
        {
            _csvDataProvider = csvDataProvider;
            _salesService = new SalesService(_csvDataProvider);
            _productService = new ProductService(_csvDataProvider);
            _storeService = new StoreService(_csvDataProvider);
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetAllSalesHistory()
        {
            var salesHistory = _salesService.GetSalesHistory();

            return View(salesHistory);
        }

        public IActionResult DeleteSaleHistory(Guid id)
        {
            _salesService.DeleteSale(id);

            return RedirectToAction("GetAllSalesHistory");
        }

        public IActionResult AddSaleHistory()
        {
            ViewBag.Products = _productService.GetProducts();
            ViewBag.Stores = _storeService.GetStores();
            return View();
        }

        [HttpPost]
        public IActionResult DoAddSaleHistory(int productId, int storeId,DateTime date,int quantity, int stock)
        {
            _salesService.AddSale(productId, storeId, date, quantity, stock);

            return RedirectToAction("GetAllSalesHistory");
        }

        public IActionResult UpdateSaleHistory(Guid id)
        {
            ViewBag.Products = _productService.GetProducts();
            ViewBag.Stores = _storeService.GetStores();
            var saleHistory = _salesService.GetSalesHistoryById(id);
            return View(saleHistory);
        }

        [HttpPost]
        public IActionResult DoUpdateSaleHistory(Guid id,int productId, int storeId, DateTime date, int quantity, int stock)
        {
            _salesService.UpdateSale(id,productId, storeId, date, quantity, stock);

            return RedirectToAction("GetAllSalesHistory");
        }

        public IActionResult GetProfitForStore()
        {
            ViewBag.Stores = _storeService.GetStores();
            return View();
        }

        public IActionResult GetProfitForStore(int storeId)
        {
            ViewBag.Stores = _storeService.GetStores();
            return View();
        }

    }
}

