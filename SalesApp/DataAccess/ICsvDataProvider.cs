namespace SalesApp.DataAccess
{
    public interface ICsvDataProvider
    {
        List<string[]> ReadCsvFile(string filePath);
        void WriteCsvFile(string filePath, List<string[]> data);
    }
}
