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
        //anasayfa
        public IActionResult Index()
        {
            return View();
        }

        //satış geçmişi listesi
        public IActionResult GetAllSalesHistory()
        {
            var salesHistory = _salesService.GetSalesHistory();

            return View(salesHistory);
        }
        //belirli bir satış geçmişini silme 
        public IActionResult DeleteSaleHistory(Guid id)
        {
            _salesService.DeleteSale(id);

            return RedirectToAction("GetAllSalesHistory");
        }
        //yeni bir satış geçmişi kaydı ekleme ekranı
        public IActionResult AddSaleHistory()
        {
            ViewBag.Products = _productService.GetProducts(); //view içindeki dropdownlara ürünler ve mağazaları göndermek içindir.
            ViewBag.Stores = _storeService.GetStores();
            return View();
        }
        //yeni bir satış geçmişi kaydı ekleme işlemi
        [HttpPost]
        public IActionResult DoAddSaleHistory(int productId, int storeId,DateTime date,int quantity, int stock,string unitprofit)
        {
            _salesService.AddSale(productId, storeId, date, quantity, stock, unitprofit);

            return RedirectToAction("GetAllSalesHistory");
        }
        //belirli bir satış geçmişini güncelleme ekranı
        public IActionResult UpdateSaleHistory(Guid id)
        {
            ViewBag.Products = _productService.GetProducts();
            ViewBag.Stores = _storeService.GetStores();
            var saleHistory = _salesService.GetSalesHistoryById(id); //id ile ilgili satış geçmişini getirmek içindir.
            return View(saleHistory);
        }
        //belirli bir satış geçmişini güncelleme işlemi
        [HttpPost]
        public IActionResult DoUpdateSaleHistory(Guid id,int productId, int storeId, DateTime date, int quantity, int stock, string unitprofit)
        {
            _salesService.UpdateSale(id,productId, storeId, date, quantity, stock ,unitprofit);

            return RedirectToAction("GetAllSalesHistory");
        }
        //seçilen mağazanın kar miktarını getirir
        public IActionResult GetProfitForStore(int storeId = 1)
        {
            ViewBag.Stores = _storeService.GetStores();
            ViewBag.SelectedStoreId = storeId; //viewda kontrol yapmak içindir.
            var result = _salesService.GetProfitForStore(storeId); //mağaza id sine göre kar miktarını getirir.
            ViewBag.Result = result;
            return View();
        }
        //en karlı mağazayı getirir
        public IActionResult GetMostProfitableStore()
        {
            var storeProfit = _salesService.GetMostProfitableStore(); //en karlı mağazayı getirir
            storeProfit.StoreName = _storeService.GetNameById(storeProfit.StoreId); //mağaza id sine göre mağaza adını getirir.
            return View(storeProfit);
        }
        //en çok satılan ürünü getirir
        public IActionResult GetBestSellingProduct()
        {
            var bestsellingProduct = _salesService.GetBestSellingProduct(); //en çok satılan ürünü getirir.
            bestsellingProduct.ProductName = _productService.GetNameById(bestsellingProduct.ProductId); //ürün id sine göre ürün adını getirir.
            return View(bestsellingProduct);
        }
    }
}

