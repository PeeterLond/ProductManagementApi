using Microsoft.AspNetCore.Mvc;
using ProductManagementApi.Dtos;
using ProductManagementApi.Service;
using Swashbuckle.AspNetCore.Annotations;

namespace ProductManagementApi.Controller {

    [ApiController]
    [Route("[controller]")]
    public class GroupController: ControllerBase {

        private GroupService _groupService;

        public GroupController(IConfiguration config) {
            _groupService = new GroupService(config);
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Get all available product groups and their subgroups."
        )]
        public IActionResult GetAllGroups() {
            try {
                return Ok(_groupService.GetAllGroups());
            } catch (Exception) {
                return StatusCode(404, "No groups in database");
            } 
        }
    }
}