using Microsoft.AspNetCore.Mvc;
using ProductManagementApi.Entities;
using ProductManagementApi.Service;
using Swashbuckle.AspNetCore.Annotations;

namespace ProductManagementApi.Controller {

    [ApiController]
    [Route("[controller]")]
    public class StoreController: ControllerBase {

        private StoreService _storeService;

        public StoreController(IConfiguration config) {
            _storeService = new StoreService(config);
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Get all available stores."
        )]
        public IActionResult GetAllStores() {
            try {
                return Ok(_storeService.GetAllStores());
            } catch (Exception) {
                return StatusCode(404, "No stores in database");
            } 
        }
    }
}