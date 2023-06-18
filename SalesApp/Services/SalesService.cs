using SalesApp.DataAccess;
using SalesApp.Models;

namespace SalesApp.Services
{
    public class SalesService
    {
        private readonly ICsvDataProvider _dataProvider;
        private List<SalesInventory> _sales;

        public SalesService(ICsvDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
            LoadSalesData();
        }
        private void LoadSalesData()//inventory-sales.csv içindeki tüm verileri SalesInventory nesnesine işler ve liste yapar.
        {
            var salesData = _dataProvider.ReadCsvFile("Data\\inventory-sales.csv");
            salesData.Remove(salesData[0]); //burada dosyadaki ilk satır olan başlık kısmını çıkardım. 
            _sales = salesData.Select(row => new SalesInventory
            {
                Id = Guid.Parse(row[0]),
                ProductId = int.Parse(row[1]),
                StoreId = int.Parse(row[2]),
                Date = DateTime.Parse(row[3]),
                SalesQuantity = int.Parse(row[4]),
                Stock = int.Parse(row[5]),
                Profit = decimal.Parse(row[6])
            }).ToList();
        }

        public List<SalesInventory> GetSalesHistory() //satış geçmişi verilerini döndürür
        {
            return _sales;
        }

        public void AddSale(int productId, int storeId, DateTime date, int quantity, int stock, string unitprofit) // yeni bir satış geçmişi ekler
        {
            var newSale = new SalesInventory
            {
                Id = Guid.NewGuid(),
                ProductId = productId,
                StoreId = storeId,
                Date = date,
                SalesQuantity = quantity,
                Stock = stock,
                Profit = Convert.ToDecimal(unitprofit) * quantity
            };

            _sales.Add(newSale);
            SaveSalesData();
        }

        public void DeleteSale(Guid id) // belirli bir satış geçmiş kaydını siler
        {
            var saleToDelete = _sales.FirstOrDefault(s => s.Id == id);
            if (saleToDelete != null)
            {
                _sales.Remove(saleToDelete);
                SaveSalesData();
            }
        }

        public SalesInventory GetSalesHistoryById(Guid id) //id ile belirli satış geçmişini getirir
        {
            var saleHistory = _sales.First(s => s.Id == id);
            return saleHistory;
        }


        public void UpdateSale(Guid id, int productId, int storeId, DateTime date, int quantity, int stock,string unitprofit) // belirli bir satış geçmiş kaydını günceller
        {
            var saleToUpdate = _sales.FirstOrDefault(s => s.Id == id);
            if (saleToUpdate != null)
            {
                saleToUpdate.ProductId = productId;
                saleToUpdate.StoreId = storeId;
                saleToUpdate.Date = date;
                saleToUpdate.SalesQuantity = quantity;
                saleToUpdate.Stock = stock;
                saleToUpdate.Profit = Convert.ToDecimal(unitprofit) * quantity;
                SaveSalesData();
            }
        }

        public decimal GetProfitForStore(int storeId) // belirli bir mağaza için elde edilen karı döndürür
        {
            return _sales.Where(s => s.StoreId == storeId).Sum(s => s.Profit);
        }

        public StoreProfitModel GetMostProfitableStore() // en karlı mağazayı döndürür
        {
            var storeProfits = _sales.GroupBy(s => s.StoreId)
                                     .Select(g => new StoreProfitModel { StoreId = g.Key, Profit = g.Sum(s => s.Profit) })
                                     .OrderByDescending(s => s.Profit)
                                     .FirstOrDefault();

            return storeProfits;
        }

        public ProductSalesQuantityModel GetBestSellingProduct() //satış miktarına göre en çok satılan ürünü döndürür
        {
            var bestSellingProduct = _sales.GroupBy(s => s.ProductId)
                                           .Select(g => new ProductSalesQuantityModel { ProductId = g.Key, SalesQuantity = g.Sum(s => s.SalesQuantity) })
                                           .OrderByDescending(s => s.SalesQuantity)
                                           .FirstOrDefault();

            return bestSellingProduct;
        }

        private void SaveSalesData() //verilerde değişiklik olduğunda inventory-sales.csv dosyasında değişiklikleri yapar.
        {
            List<string[]> salesDataList = new List<string[]>();

            var listheader = new string[] { "Id,ProductId,StoreId,Date,SalesQuantity,Stock,Profit" }; //burada dosyaya ilk satır olarak başlıkları ekledim.

            salesDataList.Add(listheader);

            var salesData = _sales.Select(s => new string[]
            {
                s.Id.ToString(),
                s.ProductId.ToString(),
                s.StoreId.ToString(),
                s.Date.ToString("yyyy-MM-dd"),
                s.SalesQuantity.ToString(),
                s.Stock.ToString(),
                s.Profit.ToString()
            }).ToList();

            salesDataList.AddRange(salesData);

            _dataProvider.WriteCsvFile("Data\\inventory-sales.csv", salesDataList);
        }
    }
}
