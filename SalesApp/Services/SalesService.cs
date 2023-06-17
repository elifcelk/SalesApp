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
        private void LoadSalesData()
        {
            var salesData = _dataProvider.ReadCsvFile("Data\\inventory-sales.csv");
            salesData.Remove(salesData[0]);
            _sales = salesData.Select(row => new SalesInventory
            {
                Id = Guid.Parse(row[0]),
                ProductId = int.Parse(row[1]),
                StoreId = int.Parse(row[2]),
                Date = DateTime.Parse(row[3]),
                SalesQuantity = int.Parse(row[4]),
                Stock = int.Parse(row[5])
            }).ToList();
        }

        public List<SalesInventory> GetSalesHistory() //satış geçmişi verilerini döndürür
        {
            return _sales;
        }

        public void AddSale(int productId, int storeId, DateTime date, int quantity, int stock) // yeni bir satış geçmişi ekler
        {
            var newSale = new SalesInventory
            {
                Id = Guid.NewGuid(),
                ProductId = productId,
                StoreId = storeId,
                Date = date,
                SalesQuantity = quantity,
                Stock = stock
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

        public SalesInventory GetSalesHistoryById(Guid id) //belirli satış geçmişini getirir
        {
            var saleHistory = _sales.First(s => s.Id == id);
            return saleHistory;
        }


        public void UpdateSale(Guid id, int productId, int storeId, DateTime date, int quantity, int stock) // belirli bir satış geçmiş kaydını günceller
        {
            var saleToUpdate = _sales.FirstOrDefault(s => s.Id == id);
            if (saleToUpdate != null)
            {
                saleToUpdate.ProductId = productId;
                saleToUpdate.StoreId = storeId;
                saleToUpdate.Date = date;
                saleToUpdate.SalesQuantity = quantity;
                saleToUpdate.Stock = stock;
                SaveSalesData();
            }
        }

        public decimal GetProfitForStore(int storeId) // belirli bir mağaza için elde edilen karı döndürür
        {
            return _sales.Where(s => s.StoreId == storeId).Sum(s => s.Amount); 
        }

        //public string GetMostProfitableStore() // en karlı mağazayı döndürür --satış tablosuna satış fiyatı-maliyet yani ürün başı kar bilgisi eklenirse, kar ile ürün miktarını çarparak satış başı elde edilen kar bulunabilir.
        //{
        //    var storeProfits = _sales.GroupBy(s => s.StoreId)
        //                             .Select(g => new { StoreId = g.Key, Profit = g.Sum(s => s.Amount) })
        //                             .OrderByDescending(s => s.Profit)
        //                             .FirstOrDefault();

        //    return storeProfits?.StoreId;
        //}

        //public string GetBestSellingProduct() //satış mikatarına göre en çok satılan ürünü döndürür
        //{
        //    var bestSellingProduct = _sales.GroupBy(s => s.ProductId)
        //                                   .Select(g => new { ProductId = g.Key, TotalAmount = g.Sum(s => s.Amount) })
        //                                   .OrderByDescending(s => s.TotalAmount)
        //                                   .FirstOrDefault();

        //    return bestSellingProduct?.ProductId;
        //}

        private void SaveSalesData()
        {
            List<string[]> salesDataList = new List<string[]>();

            var listheader = new string[] { "Id,ProductId,StoreId,Date,SalesQuantity,Stock" };

            salesDataList.Add(listheader);

            var salesData = _sales.Select(s => new string[]
            {
                s.Id.ToString(),
                s.ProductId.ToString(),
                s.StoreId.ToString(),
                s.Date.ToString("yyyy-MM-dd"),
                s.SalesQuantity.ToString(),
                s.Stock.ToString()
            }).ToList();

            salesDataList.AddRange(salesData);

            _dataProvider.WriteCsvFile("Data\\inventory-sales.csv", salesDataList);
        }
    }
}
