using Microsoft.AspNetCore.Mvc;
using ProductManagementApi.Entities;
using ProductManagementApi.Dtos;
using ProductManagementApi.Service;

namespace ProductManagementApi.Controller {

    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase {

        private readonly IConfiguration _config;
        private ProductService _productService;

        public ProductController(IConfiguration config) {
            _config = config;
            _productService = new ProductService(config);
        }

        [HttpGet]
        public IEnumerable<ProductForShowDto> GetProducts() {       
            return _productService.GetAllProducts(); 
        }

        [HttpGet("{productId}")]
        public ProductForShowDto GetProductBy(int productId) {
            return _productService.GetProductBy(productId);
        }

        [HttpPost]
        public string AddProduct(ProductToAddDto product) {
            return _productService.AddProduct(product);
        }



    }
}