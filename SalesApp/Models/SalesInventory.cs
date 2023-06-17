namespace SalesApp.Models
{
    public class SalesInventory
    {
        public Guid Id { get; set; }
        public int ProductId { get; set; }
        public int StoreId { get; set; }
        public DateTime Date { get; set; }
        //public decimal Amount { get; set; }
        public int SalesQuantity { get; set; }
        public int Stock { get; set; }
    }
}
