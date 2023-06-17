namespace SalesApp.DataAccess
{
    public class CsvDataProvider : ICsvDataProvider
    {
        private readonly string _salesFilePath;
        private readonly string _storesFilePath;
        private readonly string _productsFilePath;

        public CsvDataProvider()
        {
        }
        public CsvDataProvider(string salesFilePath, string storesFilePath, string productsFilePath)
        {
            _salesFilePath = salesFilePath;
            _storesFilePath = storesFilePath;
            _productsFilePath = productsFilePath;
        }

        public List<string[]> ReadCsvFile(string filePath)
        {
            var lines = new List<string[]>();

            try
            {
                using (var reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        var values = line.Split(',');
                        lines.Add(values);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CSV dosyası okunurken bir hata oluştu: {ex.Message}");
            }

            return lines;
        }

        public void WriteCsvFile(string filePath, List<string[]> data)
        {
            try
            {
                using (var writer = new StreamWriter(filePath))
                {
                    foreach (var values in data)
                    {
                        var line = string.Join(",", values);
                        writer.WriteLine(line);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CSV dosyasına veri girişi yapılırken bir hata oluştu: {ex.Message}");
            }
        }
    }
}
