namespace ProductManagementApi.Entities {

    public class Product {
        
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public DateTime ProductAdded { get; set; }
        public DateTime ProductEdited { get; set; }
        public decimal Price { get; set; }
        public decimal PriceVat { get; set; }
        public int Vat { get; set; }
        public int SubGroupId { get; set; }
        public bool Active { get; set; }
    }
}