using System.Data;
using Dapper;
using ProductManagementApi.Context;
using ProductManagementApi.Entities;

namespace ProductManagementApi.Service {

    public class StoreProductService {

        private ContextDapper _dapper;

        public StoreProductService(IConfiguration config) {
            _dapper = new ContextDapper(config);
        }

        public void AddProductAmount(StoreProduct storeProduct) {
            DynamicParameters findStoreProductParams = new DynamicParameters();
            findStoreProductParams.Add("@StoreIdParam", storeProduct.StoreId, DbType.Int32);
            findStoreProductParams.Add("@ProductIdParam", storeProduct.ProductId, DbType.Int32);
            StoreProduct storeProducts = _dapper.FindSingleWithParams<StoreProduct>("EXEC AppSchema.spStoreProduct_Get @StoreId = @StoreIdParam, @ProductId = @ProductIdParam", findStoreProductParams);
            
            DynamicParameters storeProductParams = new DynamicParameters();
            storeProductParams.Add("@StoreId", storeProduct.StoreId, DbType.Int32);
            storeProductParams.Add("@ProductId", storeProduct.ProductId, DbType.Int32);
            storeProductParams.Add("@ProductAmount", storeProduct.ProductAmount, DbType.Int32);
            

            _dapper.ExecuteSpWithParams("AppSchema.spStoreProduct_Patch", storeProductParams);
        }
    }
}