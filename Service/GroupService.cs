using System.Data;
using Dapper;
using ProductManagementApi.Context;
using ProductManagementApi.Dtos;

namespace ProductManagementApi.Service {

    public class GroupService {

        private ContextDapper _dapper;

        public GroupService(IConfiguration config) {
            _dapper = new ContextDapper(config);
        }

        public IEnumerable<GroupForShowDto> GetAllGroups() {
            string sqlForParentGroups = "SELECT * FROM AppSchema.ParentGroup";
            IEnumerable<GroupForShowDto> groups = _dapper.FindAll<GroupForShowDto>(sqlForParentGroups);

            foreach (GroupForShowDto group in groups) {
                DynamicParameters dynamicParams = new DynamicParameters();
                dynamicParams.Add("@ParentGroupId", group.ParentGroupId, DbType.Int32);
                string sqlForSubGroups = "EXEC AppSchema.spSubGroup_Get @ParentGroupId = @ParentGroupId";
                IEnumerable<SubGroupForShowDto> subGroups = _dapper.FindAllWithParams<SubGroupForShowDto>(sqlForSubGroups, dynamicParams);
                group.SubGroups = subGroups.ToList();
            }
            return groups;
        }
    }
}