using Microsoft.AspNetCore.Mvc;
using ProductManagementApi.Dtos;
using ProductManagementApi.Service;
using Swashbuckle.AspNetCore.Annotations;

namespace ProductManagementApi.Controller {

    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase {

        private ProductService _productService;

        public ProductController(IConfiguration config) {
            _productService = new ProductService(config);
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Returns all available products."
        )]
        public IEnumerable<ProductForShowDto> GetProducts() {       
            return _productService.GetAllProducts(); 
        }

        [HttpGet("{productId}")]
        [SwaggerOperation(
            Summary = "Returns available product by it's Id."
        )]
        public ProductForShowDto GetProductBy(int productId) {
            return _productService.GetProductBy(productId);
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Adds a product.",
            Description = "Adds a product to product table. For price, priceVat and vat you can enter only 2 values, the 3rd is automaticaly calculated." +
            " Also adds the product to storeProduct table by providing a list of storeIds."
        )]
        public IActionResult AddProduct(ProductToAddDto product) {
            try {
                return Ok(_productService.AddProduct(product));
            } catch(Exception e) {
                return StatusCode(404, e.Message);
            }
        }
    }
}