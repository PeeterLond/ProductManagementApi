
namespace ProductManagementApi.Dtos {

    public class ProductToAddDto {
        
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal PriceVat { get; set; }
        public int Vat { get; set; }
        public int SubGroupId { get; set; }
        public List<int> StoreIds { get; set; } = new List<int>();
    }
}