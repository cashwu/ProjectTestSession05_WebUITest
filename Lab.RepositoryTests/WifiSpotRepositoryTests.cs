using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using Dapper;
using FluentAssertions;
using Lab.Repository;
using Lab.Repository.DB;
using Lab.Repository.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

// ReSharper disable ExpressionIsAlwaysNull

namespace Lab.RepositoryTests
{
    [TestClass()]
    [DeploymentItem(@"TestData\create_table.sql")]
    [DeploymentItem(@"TestData\insert_data.sql")]
    public class WifiSpotRepositoryTests
    {
        private IDatabaseConnectionFactory DatabaseConnectionFactory { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            this.DatabaseConnectionFactory = Substitute.For<IDatabaseConnectionFactory>();

            this.DatabaseConnectionFactory.Create().Returns
            (
                new SqlConnection(TestHook.ConnectionString)
            );
        }

        private WifiSpotRepository GetSystemUnderTest()
        {
            var sut = new WifiSpotRepository(this.DatabaseConnectionFactory);
            return sut;
        }

        public TestContext TestContext { get; set; }

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            PrepareData();
        }

        private static void PrepareData()
        {
            using (var conn = new SqlConnection(TestHook.ConnectionString))
            {
                conn.Open();

                var sqlCommand = File.ReadAllText(@"create_table.sql");
                conn.Execute(sqlCommand);
            }

            using (var conn = new SqlConnection(TestHook.ConnectionString))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    var script = File.ReadAllText(@"insert_data.sql");
                    conn.Execute(sql: script, transaction: trans);
                    trans.Commit();
                }
            }
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            DropTable();
        }

        private static void DropTable()
        {
            using (var conn = new SqlConnection(TestHook.ConnectionString))
            {
                conn.Open();

                var sqlCommand = new StringBuilder();
                sqlCommand.AppendLine(@"IF OBJECT_ID('dbo.NewTaipeiWifiSpot', 'U') IS NOT NULL");
                sqlCommand.AppendLine(@"  DROP TABLE dbo.NewTaipeiWifiSpot; ");

                conn.Execute(sqlCommand.ToString());
            }
        }

        //-----------------------------------------------------------------------------------------

        [TestMethod]
        [TestCategory("WifiSpotRepository")]
        [TestProperty("WifiSpotRepository", "GetAll")]
        public void GetAll_取得所有資料()
        {
            // arrange
            int expected = 1462;

            var sut = this.GetSystemUnderTest();

            // act
            var actual = sut.GetAll();

            // assert
            actual.Any().Should().BeTrue();
            actual.Should().HaveCount(expected);
        }

        [TestMethod]
        [TestCategory("WifiSpotRepository")]
        [TestProperty("WifiSpotRepository", "GetByDistrict")]
        public void GetByDistrict_district輸入空白_應取得所有資料()
        {
            // arrange
            string district = "";
            int expected = 1462;

            var sut = this.GetSystemUnderTest();

            // act
            var actual = sut.GetByDistrict(district);

            // assert
            actual.Any().Should().BeTrue();
            actual.Should().HaveCount(expected);
        }

        [TestMethod]
        [TestCategory("WifiSpotRepository")]
        [TestProperty("WifiSpotRepository", "GetByDistrict")]
        public void GetByDistrict_district為null_應取得所有資料()
        {
            // arrange
            string district = null;
            int expected = 1462;

            var sut = this.GetSystemUnderTest();

            // act
            var actual = sut.GetByDistrict(district);

            // assert
            actual.Any().Should().BeTrue();
            actual.Should().HaveCount(expected);
        }

        [TestMethod]
        [TestCategory("WifiSpotRepository")]
        [TestProperty("WifiSpotRepository", "GetByDistrict")]
        public void GetByDistrict_district為中和區_應取得中和區所有資料()
        {
            // arrange
            string district = "中和區";

            var sut = this.GetSystemUnderTest();

            // act
            var actual = sut.GetByDistrict(district);

            // assert
            actual.Any().Should().BeTrue();
            actual.All(x => x.District == district).Should().BeTrue();
        }

        [TestMethod]
        [TestCategory("WifiSpotRepository")]
        [TestProperty("WifiSpotRepository", "GetByCondition")]
        public void GetByCondition_沒有輸入任何條件_應取得所有資料()
        {
            string district = "";
            string type = "";
            string company = "";

            int expected = 1462;

            var sut = this.GetSystemUnderTest();

            // act
            var actual = sut.GetByCondition(district, type, company);

            // assert
            actual.Any().Should().BeTrue();
            actual.Should().HaveCount(expected);
        }

        [TestMethod]
        [TestCategory("WifiSpotRepository")]
        [TestProperty("WifiSpotRepository", "GetByCondition")]
        public void GetByCondition_district為永和區_type為空白_company為空白_應取得永和區所有資料()
        {
            string district = "永和區";
            string type = "";
            string company = "";

            var sut = this.GetSystemUnderTest();

            // act
            var actual = sut.GetByCondition(district, type, company);

            // assert
            actual.Any().Should().BeTrue();
            actual.All(x => x.District == district).Should().BeTrue();
        }

        [TestMethod]
        [TestCategory("WifiSpotRepository")]
        [TestProperty("WifiSpotRepository", "GetByCondition")]
        public void GetByCondition_district為永和區_type為NewTaipei_company為空白_應取得永和區以及類別為NewTaipei所有資料()
        {
            string district = "永和區";
            string type = "NewTaipei";
            string company = "";

            var sut = this.GetSystemUnderTest();

            // act
            var actual = sut.GetByCondition(district, type, company);

            // assert
            actual.Any().Should().BeTrue();
            actual.All(x => x.District == district && x.Type == type).Should().BeTrue();
        }

        [TestMethod]
        [TestCategory("WifiSpotRepository")]
        [TestProperty("WifiSpotRepository", "GetByCondition")]
        public void GetByCondition_district為板橋區_type為NewTaipei_company為大同資訊_應取得板橋區以及類別為NewTaipei以及業者為大同資訊所有資料()
        {
            string district = "板橋區";
            string type = "NewTaipei";
            string company = "大同資訊";

            var sut = this.GetSystemUnderTest();

            // act
            var actual = sut.GetByCondition(district, type, company);

            // assert
            actual.Any().Should().BeTrue();

            actual.All(x => x.District == district && x.Type == type && x.Company == company)
                  .Should().BeTrue();
        }

        [TestMethod]
        [TestCategory("WifiSpotRepository")]
        [TestProperty("WifiSpotRepository", "GetByCondition")]
        public void GetByCondition_district為八里區_type為NewTaipei_company為大同資訊_應取得八里區以及類別為NewTaipei以及業者為中華電信所有資料()
        {
            string district = "八里區";
            string type = "";
            string company = "中華電信";

            var sut = this.GetSystemUnderTest();

            // act
            var actual = sut.GetByCondition(district, type, company);

            // assert
            actual.Any().Should().BeTrue();

            actual.All(x => x.District == district && x.Company == company)
                  .Should().BeTrue();
        }

        [TestMethod]
        [TestCategory("WifiSpotRepository")]
        [TestProperty("WifiSpotRepository", "GetByCondition")]
        public void GetByCondition_district為空白_type為NewTaipei_company為空白_應取得類別為NewTaipei所有資料()
        {
            string district = "";
            string type = "NewTaipei";
            string company = "";

            int expected = 1462;

            var sut = this.GetSystemUnderTest();

            // act
            var actual = sut.GetByCondition(district, type, company);

            // assert
            actual.Any().Should().BeTrue();
            actual.All(x => x.Type == type).Should().BeTrue();
            actual.Should().HaveCount(expected);
        }

        [TestMethod]
        public void GetTypes_取得所有WifiSpot的Type類別資料()
        {
            // arrange
            int expected = 1;

            var sut = this.GetSystemUnderTest();

            // act
            var actual = sut.GetTypes();

            // assert
            actual.Any().Should().BeTrue();
            actual.Should().HaveCount(expected);
        }

        [TestMethod]
        public void GetCompanies_取得所有業者名稱()
        {
            // arrange
            int expected = 3;

            var sut = this.GetSystemUnderTest();

            // act
            var actual = this.GetSystemUnderTest().GetCompanies();
        
            // assert
            actual.Any().Should().BeTrue();
            actual.Should().HaveCount(expected);
            actual.Should().OnlyHaveUniqueItems();
        }

        [TestMethod]
        public void GetDistricts_取得所以鄉鎮市區資料()
        {
            // arrange
            int expected = 29;

            var sut = this.GetSystemUnderTest();

            // act
            var actual = sut.GetDistricts();

            // assert
            actual.Any().Should().BeTrue();
            actual.Should().HaveCount(expected);
            actual.Should().OnlyHaveUniqueItems();
        }
    }
}