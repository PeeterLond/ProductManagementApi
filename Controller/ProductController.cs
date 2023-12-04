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
            Summary = "Get all available products."
        )]
        public IActionResult GetProducts() {  
            try {
                return Ok(_productService.GetAllProducts());
            } catch (Exception) {
                return StatusCode(404, $"No products in database");
            }     
        }

        [HttpGet("{productId}")]
        [SwaggerOperation(
            Summary = "Get product by it's Id."
        )]
        public IActionResult GetProductBy(int productId) {
            try {
                return Ok(_productService.GetProductBy(productId));
            } catch (Exception) {
                return StatusCode(404, $"No product with Id: {productId}");
            }
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Add a product.",
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

        [HttpPut("{productId}")]
        [SwaggerOperation(
            Summary = "Edit a product.",
            Description = "Edits a product on the product table. If the product was inactice, then changes the active field to true. Updates the stores the product is sold in." 
        )]
        public IActionResult EditProduct(ProductToAddDto product, int productId) {
            try {
                return Ok(_productService.EditProduct(product, productId));
            } catch(Exception e) {
                return StatusCode(404, e.Message);
            }
        }

        [HttpDelete("{productId}")]
        [SwaggerOperation(
            Summary = "Delete a product",
            Description = "Set's product active field to false and deletes rows from storeProduct table."
        )]
        public IActionResult DeleteProduct(int productId) {
            if(_productService.DeleteProduct(productId)){
                return Ok();
            }
            return StatusCode(404, $"Product not found with Id: {productId}");
        }
    }
}