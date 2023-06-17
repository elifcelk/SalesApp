using SalesApp.DataAccess;
using SalesApp.Models;

namespace SalesApp.Services
{
    public class ProductService
    {
        private readonly ICsvDataProvider _dataProvider;
        private List<Product> _products;

        public ProductService(ICsvDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
            LoadProductsData();
        }
        private void LoadProductsData()
        {
            var productsData = _dataProvider.ReadCsvFile("Data\\products.csv");
            productsData.Remove(productsData[0]);
            _products = productsData.Select(row => new Product
            {
                Id = int.Parse(row[0]),
                Name = row[1],
                Cost = decimal.Parse(row[2]),
                SalesPrice = decimal.Parse(row[3])
            }).ToList();
        }

        public List<Product> GetProducts() //tüm ürünleri döndürür
        {
            return _products;
        }
    }
}
