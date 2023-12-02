using System.Data;
using Dapper;
using ProductManagementApi.Context;
using ProductManagementApi.Dtos;
using ProductManagementApi.Entities;

namespace ProductManagementApi.Service {

    public class GroupService {

        private ContextDapper _dapper;

        public GroupService(IConfiguration config) {
            _dapper = new ContextDapper(config);
        }

        public IEnumerable<GroupForShowDto> GetAllGroups() {
            string sql = "SELECT * FROM AppSchema.ParentGroup";
            IEnumerable<GroupForShowDto> groupsForShow = _dapper.FindAll<GroupForShowDto>(sql);

            foreach (GroupForShowDto groupForShow in groupsForShow) {
                DynamicParameters dynamicParams = new DynamicParameters();
                dynamicParams.Add("@ParentGroupId", groupForShow.ParentGroupId, DbType.Int32);
                string sqlForSubGroups = "SELECT * FROM AppSchema.SubGroup WHERE ParentGroupId = @ParentGroupId";
                IEnumerable<SubGroup> subGroups = _dapper.FindAllWithParams<SubGroup>(sqlForSubGroups, dynamicParams);
                groupForShow.SubGroups = subGroups.ToList();
            }
            return groupsForShow;

        }
    }
}