using ProductManagementApi.Entities;

namespace ProductManagementApi.Dtos {

    public class ProductForShowDto {
        
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public DateTime ProductAdded { get; set; }
        public DateTime ProductEdited { get; set; }
        public decimal Price { get; set; }
        public decimal PriceVat { get; set; }
        public int Vat { get; set; }
        public string SubGroupName { get; set; } = string.Empty;
        public bool Active { get; set; }
        public List<Store> Stores { get; set; } = new List<Store>();
    }
}