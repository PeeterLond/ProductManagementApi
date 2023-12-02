namespace ProductManagementApi.Entities {

    public class SubGroup {
        
        public int SubGroupId { get; set; }
        public string SubGroupName { get; set; } = string.Empty;
        public int ParentGroupId { get; set; }

    }
}