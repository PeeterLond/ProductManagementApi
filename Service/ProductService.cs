using System.Data;
using Dapper;
using ProductManagementApi.Context;
using ProductManagementApi.Dtos;

namespace ProductManagementApi.Service {

    public class ProductService {

        private ContextDapper _dapper;

        public ProductService(IConfiguration config) {
            
            _dapper = new ContextDapper(config);
        }

        public IEnumerable<ProductForShowDto> GetAllProducts() {
            string sqlForProducts = "EXEC AppSchema.spProduct_Get";
            IEnumerable<ProductForShowDto> products = _dapper.FindAll<ProductForShowDto>(sqlForProducts);

            foreach (ProductForShowDto product in products){
                DynamicParameters dynamicParams = new DynamicParameters();
                dynamicParams.Add("@ProductIdParam", product.ProductId, DbType.Int32);
                string sqlForStores = "EXEC AppSchema.spStore_GetByProduct @ProductId = @ProductIdParam";
                IEnumerable<StoreForFhowDto> stores =_dapper.FindAllWithParams<StoreForFhowDto>(sqlForStores, dynamicParams);
                product.Stores = stores.ToList();             
            }
            return products;
        }

        public ProductForShowDto GetProductBy(int productId){
            DynamicParameters dynamicParams = new DynamicParameters();
            dynamicParams.Add("@ProductIdParam", productId, DbType.Int32);
            string sqlForProduct = "EXEC AppSchema.spProduct_Get @ProductId = @ProductIdParam";
            ProductForShowDto product = _dapper.FindSingleWithParams<ProductForShowDto>(sqlForProduct, dynamicParams);

            string sqlForStores = "EXEC AppSchema.spStore_GetByProduct @ProductId = @ProductIdParam";
            IEnumerable<StoreForFhowDto> stores =_dapper.FindAllWithParams<StoreForFhowDto>(sqlForStores, dynamicParams);
                
            product.Stores = stores.ToList();

            return product;
        }

        public ProductForShowDto AddProduct(ProductToAddDto product) {
            ValidateCorrectPriceInput(product);
            ValidateCorrectStoreIdInput(product);
            DynamicParameters productParams = new DynamicParameters();
            productParams.Add("@ProductName", product.ProductName, DbType.String);
            productParams.Add("@Price", product.Price, DbType.Decimal);
            productParams.Add("@PriceVat", product.PriceVat, DbType.Decimal);
            productParams.Add("@Vat", product.Vat, DbType.Int32);
            productParams.Add("@SubGroupId", product.SubGroupId, DbType.Int32);
            productParams.Add("@OutputProductId", DbType.Int32, direction: ParameterDirection.Output);
            
            _dapper.ExecuteSpWithParams("AppSchema.spProduct_Add", productParams);
            int productId = productParams.Get<int>("@OutputProductId");

            AddStoreProducts(productId, product.StoreIds);

            return GetProductBy(productId);
        }

        public ProductForShowDto EditProduct(ProductToAddDto product, int productId) {
            ValidateCorrectPriceInput(product);
            ValidateCorrectStoreIdInput(product);
            DynamicParameters productParams = new DynamicParameters();
            productParams.Add("ProductId", productId, DbType.Int32);
            productParams.Add("@ProductName", product.ProductName, DbType.String);
            productParams.Add("@Price", product.Price, DbType.Decimal);
            productParams.Add("@PriceVat", product.PriceVat, DbType.Decimal);
            productParams.Add("@Vat", product.Vat, DbType.Int32);
            productParams.Add("@SubGroupId", product.SubGroupId, DbType.Int32);

            _dapper.ExecuteSpWithParams("AppSchema.spProduct_Edit", productParams);
            DynamicParameters deleteParams = new DynamicParameters();
            deleteParams.Add("@StoreProductId", productId, DbType.Int32);
            _dapper.ExecuteSpWithParams("AppSchema.spStoreProduct_Delete", deleteParams);

            AddStoreProducts(productId, product.StoreIds);

            return GetProductBy(productId);
        }

        public bool DeleteProduct(int productId) {
            DynamicParameters productParams = new DynamicParameters();
            productParams.Add("@ProductId", productId, DbType.Int32);
            return _dapper.ExecuteSpWithParams("AppSchema.spProduct_Delete", productParams) > 0;
        }

        private void ValidateCorrectPriceInput(ProductToAddDto product) {
            if(product.Price == 0 && product.PriceVat > 0 && product.Vat > 0) {
                product.Price =  product.PriceVat / (1 + ((decimal)product.Vat / 100));
            } else if (product.PriceVat == 0 && product.Price > 0 && product.Vat > 0) {
                product.PriceVat = product.Price * (1 + ((decimal)product.Vat / 100));
            } else if (product.Vat == 0 && product.Price > 0 && product.PriceVat > 0){
                product.Vat = Decimal.ToInt32((product.PriceVat - product.Price) * 100 / product.Price);
            } else if(product.Vat > 0 && product.Price > 0 && product.PriceVat > 0){
                return;
            } else {
                throw new Exception("Please provide atleat 2 values: Price, PriceVat, Vat");
            }
        }

        private void ValidateCorrectStoreIdInput(ProductToAddDto product) {
            IEnumerable<int> stores = _dapper.FindAll<int>("SELECT StoreId FROM AppSchema.Store");
            foreach(int storeId in product.StoreIds) {
                if(!stores.Contains(storeId)) {
                    throw new Exception("Please enter a valid store Id.");
                }
            }
        }

        private void AddStoreProducts(int productId, List<int> storeIds) {
            foreach(int storeId in storeIds) {
                DynamicParameters storeParams = new DynamicParameters();
                storeParams.Add("@StoreId", storeId, DbType.Int32);
                storeParams.Add("@ProductId", productId, DbType.Int32);
                _dapper.ExecuteSpWithParams("AppSchema.spStoreProduct_Add", storeParams);
            }
        }
    }
}