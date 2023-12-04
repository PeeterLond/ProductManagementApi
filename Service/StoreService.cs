using ProductManagementApi.Context;
using ProductManagementApi.Entities;

namespace ProductManagementApi.Service {

    public class StoreService {

        private ContextDapper _dapper;

        public StoreService(IConfiguration config) {
            _dapper = new ContextDapper(config);
        }

        public IEnumerable<Store> GetAllStores() {
            string sqlForStores = "SELECT * FROM AppSchema.Store";
            return _dapper.FindAll<Store>(sqlForStores);
        }
    }
}