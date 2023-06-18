using SalesApp.DataAccess;
using SalesApp.Models;

namespace SalesApp.Services
{
    public class StoreService
    {
        private readonly ICsvDataProvider _dataProvider;
        private List<Store> _stores;

        public StoreService(ICsvDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
            LoadStoresData();
        }
        private void LoadStoresData()//stores.csv içindeki tüm verileri Product nesnesine işler ve liste yapar.
        {
            var storesData = _dataProvider.ReadCsvFile("Data\\stores.csv");
            storesData.Remove(storesData[0]);
            _stores = storesData.Select(row => new Store
            {
                Id = int.Parse(row[0]),
                Name = row[1],
            }).ToList();
        }

        public List<Store> GetStores() //tüm mağazaları döndürür
        {
            return _stores;
        }

        public string GetNameById(int storeId) //mağaza Id sine göre mağaza adını döndürür
        {
            return _stores.Where(s => s.Id == storeId).Select(s => s.Name).First();
        }
    }
}
