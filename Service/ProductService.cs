using System.Data;
using AutoMapper;
using Dapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ProductManagementApi.Context;
using ProductManagementApi.Dtos;
using ProductManagementApi.Entities;

namespace ProductManagementApi.Service {

    public class ProductService {

        private ContextDapper _dapper;
        private IMapper _mapper;

        public ProductService(IConfiguration config) {
            
            _dapper = new ContextDapper(config);
            _mapper = new Mapper(new MapperConfiguration(cfg => {
                cfg.CreateMap<Product, ProductForShowDto>();
            }));
        }

        public IEnumerable<ProductForShowDto> GetAllProducts() {
            string sql = "SELECT * FROM AppSchema.Product";
            IEnumerable<Product> products = _dapper.FindAll<Product>(sql);
            List<ProductForShowDto> productsForShow = new List<ProductForShowDto>();

            foreach (Product product in products){
                ProductForShowDto productForShow = _mapper.Map<ProductForShowDto>(product);

                DynamicParameters dynamicParams = new DynamicParameters();
                dynamicParams.Add("@SubGroupId", product.SubGroupId, DbType.Int32);
                string sqlForSubgroup = @"SELECT SubGroupName FROM AppSchema.SubGroup 
                    WHERE SubGroupId = @SubGroupId";
                string subGroupName = _dapper.FindSingleWithParams<string>(sqlForSubgroup, dynamicParams);
                
                DynamicParameters dynamicParams2 = new DynamicParameters();
                dynamicParams2.Add("@ProductId", product.ProductId, DbType.Int32);
                string sqlForStoreProducts = @"SELECT * FROM AppSchema.StoreProduct 
                    WHERE ProductId = @ProductId";
                IEnumerable<StoreProduct> storeProducts =_dapper.FindAllWithParams<StoreProduct>(sqlForStoreProducts, dynamicParams2);
                
                List<Store> stores = new List<Store>();
                foreach (StoreProduct storeProduct in storeProducts){
                    DynamicParameters dynamicParams3 = new DynamicParameters();
                    dynamicParams3.Add("@StoreId", storeProduct.StoreId, DbType.Int32);
                    string sqlForStores = "SELECT * FROM AppSchema.Store WHERE StoreId = @StoreId";
                    Store store = _dapper.FindSingleWithParams<Store>(sqlForStores, dynamicParams3);
                    stores.Add(store);
                }
                
                productForShow.SubGroupName = subGroupName;
                productForShow.Stores = stores;
                productsForShow.Add(productForShow);
            }
            return productsForShow;
        }

        public ProductForShowDto GetProductBy(int productId){
            DynamicParameters dynamicParams = new DynamicParameters();
            dynamicParams.Add("@ProductId", productId, DbType.Int32);
            string sql = "SELECT * FROM AppSchema.Product WHERE ProductId = @ProductId";
            Product product = _dapper.FindSingleWithParams<Product>(sql, dynamicParams);

            DynamicParameters dynamicParams2 = new DynamicParameters();
            dynamicParams2.Add("@SubGroupId", product.SubGroupId, DbType.Int32);
            string sqlForSubgroup = @"SELECT SubGroupName FROM AppSchema.SubGroup 
                WHERE SubGroupId = @SubGroupId";
            string subGroupName = _dapper.FindSingleWithParams<string>(sqlForSubgroup, dynamicParams2);

            DynamicParameters dynamicParams3 = new DynamicParameters();
            dynamicParams3.Add("@ProductId", product.ProductId, DbType.Int32);
            string sqlForStoreProducts = @"SELECT * FROM AppSchema.StoreProduct 
                WHERE ProductId = @ProductId";
            IEnumerable<StoreProduct> storeProducts =_dapper.FindAllWithParams<StoreProduct>(sqlForStoreProducts, dynamicParams3);
                
            List<Store> stores = new List<Store>();
            foreach (StoreProduct storeProduct in storeProducts){
                DynamicParameters dynamicParams4 = new DynamicParameters();
                dynamicParams4.Add("@StoreId", storeProduct.StoreId, DbType.Int32);
                string sqlForStores = "SELECT * FROM AppSchema.Store WHERE StoreId = @StoreId";
                Store store = _dapper.FindSingleWithParams<Store>(sqlForStores, dynamicParams4);
                stores.Add(store);
            }
            ProductForShowDto productForShow = _mapper.Map<ProductForShowDto>(product);
            productForShow.SubGroupName = subGroupName;
            productForShow.Stores = stores;

            return productForShow;
        }


        public string AddProduct(ProductToAddDto product) {
            DynamicParameters dynamicParams = new DynamicParameters();
            dynamicParams.Add("@ProductName", product.ProductName, DbType.String);
            dynamicParams.Add("@Price", product.Price, DbType.Decimal);
            dynamicParams.Add("@PriceVat", product.PriceVat, DbType.Decimal);
            dynamicParams.Add("@Vat", product.Vat, DbType.Int32);
            dynamicParams.Add("@SubGroupId", product.SubGroupId, DbType.Int32);

            string sql = @"INSERT INTO AppSchema.Product 
                (ProductName, ProductAdded, ProductEdited, Price, PriceVat, Vat, SubGroupId, Active)
                VALUES (@ProductName, GETDATE(), GETDATE(), @Price, @PriceVat, @Vat, @SubGroupId, 1 )";
            
            _dapper.ExecuteWithParams(sql, dynamicParams);
            int productId = _dapper.FindSingle<int>($"SELECT ProductId FROM AppSchema.Product WHERE ProductName = '{product.ProductName}'");

            foreach(int storeId in product.StoreIds) {
                DynamicParameters dynamicParams2 = new DynamicParameters();
                dynamicParams2.Add("@StoreId", storeId, DbType.Int32);
                dynamicParams2.Add("@ProductId", productId, DbType.Int32);
                string sqlStoreProduct = @"INSERT INTO AppSchema.StoreProduct (StoreId, ProductId)
                    VALUES (@StoreId, @ProductId)";
                _dapper.ExecuteWithParams(sqlStoreProduct, dynamicParams2);
            }
            return "all good";

        }
    }
}