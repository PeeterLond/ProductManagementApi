using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace ProductManagementApi.Context {

    public class ContextDapper {

        private readonly IConfiguration _config;


        public ContextDapper(IConfiguration config) {
            _config = config;
        }

        public IEnumerable<T> FindAll<T>(string sql) {
            IDbConnection connection = GetConnection();
            return connection.Query<T>(sql);
        }

        public IEnumerable<T> FindAllWithParams<T>(string sql, DynamicParameters param) {
            IDbConnection connection = GetConnection();
            return connection.Query<T>(sql, param);
        }

        public T FindSingleWithParams<T>(string sql, DynamicParameters param) {
            IDbConnection connection = GetConnection();
            return connection.QuerySingle<T>(sql, param);
        }

        public T FindSingle<T>(string sql) {
            IDbConnection connection = GetConnection();
            return connection.QuerySingle<T>(sql);
        }

        public int ExecuteWithParams(string sql, DynamicParameters param) {
            IDbConnection connection = GetConnection();
            return connection.Execute(sql, param);
        }

        public int ExecuteSpWithParams(string storedProcedure, DynamicParameters parameters) {
            IDbConnection connection = GetConnection();
            return connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
        }

        private IDbConnection GetConnection() {
            return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        }
    }  
}