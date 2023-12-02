using Microsoft.AspNetCore.Mvc;
using ProductManagementApi.Context;
using ProductManagementApi.Dtos;
using ProductManagementApi.Service;

namespace ProductManagementApi.Controller {

    [ApiController]
    [Route("[controller]")]
    public class GroupController: ControllerBase {

        private GroupService _groupService;

        public GroupController(IConfiguration config) {
            _groupService = new GroupService(config);
        }

        [HttpGet]
        public IEnumerable<GroupForShowDto> GetGroups() {
            return _groupService.GetAllGroups();
        }


    }
}