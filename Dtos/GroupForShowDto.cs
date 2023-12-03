using ProductManagementApi.Entities;

namespace ProductManagementApi.Dtos {

    public class GroupForShowDto {
        
        public int ParentGroupId { get; set; }
        public string ParentGroupName { get; set; } = string.Empty;
        public List<SubGroupForShowDto> SubGroups { get; set; } = new List<SubGroupForShowDto>();
    }
}