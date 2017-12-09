using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Lab.Repository.DB;

namespace Lab.Repository
{
    /// <summary>
    /// Class WifiSpotRepository.
    /// </summary>
    /// <seealso cref="Lab.Repository.IWifiSpotRepository" />
    public class WifiSpotRepository : IWifiSpotRepository
    {
        private IDatabaseConnectionFactory DatabaseConnectionFactory { get; set; }

        public WifiSpotRepository(IDatabaseConnectionFactory connectionFactory)
        {
            this.DatabaseConnectionFactory = connectionFactory;
        }

        /// <summary>
        /// 取得所有無線網路熱點資料
        /// </summary>
        /// <returns>List&lt;WifiSpotModel&gt;.</returns>
        public List<WifiSpotModel> GetAll()
        {
            var dbConnection = this.DatabaseConnectionFactory.Create();
            using (var conn = dbConnection)
            {
                var sqlCommand = "select * from dbo.NewTaipeiWifiSpot";
                var result = conn.Query<WifiSpotModel>(sqlCommand);
                return result.ToList();
            }
        }

        /// <summary>
        /// 依據鄉鎮市區取得無線網路熱點資料
        /// </summary>
        /// <param name="district">鄉鎮市區名稱</param>
        /// <returns>List&lt;WifiSpotModel&gt;.</returns>
        public List<WifiSpotModel> GetByDistrict(string district)
        {
            if (string.IsNullOrWhiteSpace(district))
            {
                var models = this.GetAll();
                return models;
            }

            var dbConnection = this.DatabaseConnectionFactory.Create();
            using (var conn = dbConnection)
            {
                var sqlCommand = " select * from dbo.NewTaipeiWifiSpot " +
                                 " where District = @District ";

                var result = conn.Query<WifiSpotModel>
                (
                    sql: sqlCommand,
                    param: new
                    {
                        District = district
                    }
                );
                return result.ToList();
            }
        }

        /// <summary>
        /// 依據鄉鎮市區、熱點分類、業者取得無線網路熱點資料
        /// </summary>
        /// <param name="district">鄉鎮市區名稱</param>
        /// <param name="type">熱點分類</param>
        /// <param name="company">業者</param>
        /// <returns>List&lt;WifiSpotModel&gt;.</returns>
        public List<WifiSpotModel> GetByCondition(string district,
                                                  string type,
                                                  string company)
        {
            if (string.IsNullOrWhiteSpace(district)
                && string.IsNullOrWhiteSpace(type)
                && string.IsNullOrWhiteSpace(company))
            {
                var models = this.GetAll();
                return models;
            }

            List<string> conditions = new List<string>();
            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(district))
            {
                conditions.Add(" District = @District ");
                parameters.Add("District", district);
            }
            if (!string.IsNullOrWhiteSpace(type))
            {
                conditions.Add(" Type = @Type ");
                parameters.Add("Type", type);
            }
            if (!string.IsNullOrWhiteSpace(company))
            {
                conditions.Add(" Company = @Company ");
                parameters.Add("Company", company);
            }

            var sqlCommand = " select * from dbo.NewTaipeiWifiSpot ";
            sqlCommand = string.Concat(sqlCommand, " where ", string.Join(" and ", conditions));

            var dbConnection = this.DatabaseConnectionFactory.Create();
            using (var conn = dbConnection)
            {
                var result = conn.Query<WifiSpotModel>
                (
                    sql: sqlCommand,
                    param: parameters
                );

                return result.ToList();
            }
        }

        /// <summary>
        /// 取得類別.
        /// </summary>
        /// <returns>List&lt;System.String&gt;.</returns>
        public List<string> GetTypes()
        {
            var dbConnection = this.DatabaseConnectionFactory.Create();
            using (var conn = dbConnection)
            {
                var sqlCommand = "select distinct Type from dbo.NewTaipeiWifiSpot ";

                var result = conn.Query<string>
                (
                    sql: sqlCommand
                );

                return result.ToList();
            }
        }

        /// <summary>
        /// 取得業者.
        /// </summary>
        /// <returns>List&lt;System.String&gt;.</returns>
        public List<string> GetCompanies()
        {
            var dbConnection = this.DatabaseConnectionFactory.Create();
            using (var conn = dbConnection)
            {
                var sqlCommand = "select distinct Company from dbo.NewTaipeiWifiSpot ";

                var result = conn.Query<string>
                (
                    sql: sqlCommand
                );

                return result.ToList();
            }
        }

        /// <summary>
        /// 取得鄉鎮市區.
        /// </summary>
        /// <returns>List&lt;System.String&gt;.</returns>
        public List<string> GetDistricts()
        {
            var dbConnection = this.DatabaseConnectionFactory.Create();
            using (var conn = dbConnection)
            {
                var sqlCommand = "select distinct District from dbo.NewTaipeiWifiSpot ";

                var result = conn.Query<string>
                (
                    sql: sqlCommand
                );

                return result.ToList();
            }
        }
    }
}