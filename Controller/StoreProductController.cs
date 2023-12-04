using Microsoft.AspNetCore.Mvc;
using ProductManagementApi.Entities;
using ProductManagementApi.Service;
using Swashbuckle.AspNetCore.Annotations;

namespace ProductManagementApi.Controller {

    [ApiController]
    [Route("[controller]")]
    public class StoreProductController: ControllerBase {

        private StoreProductService _storeProductService;

        public StoreProductController(IConfiguration config) {
            _storeProductService = new StoreProductService(config);
        }

        [HttpPatch]
        [SwaggerOperation(
            Summary = "Add product amount to store product"
        )]
        public IActionResult AddProductAmount(StoreProduct storeProduct) {
            try{
                _storeProductService.AddProductAmount(storeProduct);
                return Ok();
            } catch (Exception) {
                return StatusCode(404, "Invalid product or store id");
            }
        }
    }
}