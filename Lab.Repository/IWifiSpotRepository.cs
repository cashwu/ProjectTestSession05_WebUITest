using System.Collections.Generic;

namespace Lab.Repository
{
    /// <summary>
    /// 無線網路熱點資訊 Repository
    /// </summary>
    public interface IWifiSpotRepository
    {
        /// <summary>
        /// 取得所有無線網路熱點資料
        /// </summary>
        /// <returns>List&lt;WifiSpotModel&gt;.</returns>
        List<WifiSpotModel> GetAll();

        /// <summary>
        /// 依據鄉鎮市區取得無線網路熱點資料
        /// </summary>
        /// <param name="district">鄉鎮市區名稱</param>
        /// <returns>List&lt;WifiSpotModel&gt;.</returns>
        List<WifiSpotModel> GetByDistrict(string district);

        /// <summary>
        /// 依據鄉鎮市區、熱點分類、業者取得無線網路熱點資料
        /// </summary>
        /// <param name="district">鄉鎮市區名稱</param>
        /// <param name="type">熱點分類</param>
        /// <param name="company">業者</param>
        /// <returns>List&lt;WifiSpotModel&gt;.</returns>
        List<WifiSpotModel> GetByCondition(string district, string type, string company);

        /// <summary>
        /// 取得類別.
        /// </summary>
        /// <returns>List&lt;System.String&gt;.</returns>
        List<string> GetTypes();

        /// <summary>
        /// 取得業者.
        /// </summary>
        /// <returns>List&lt;System.String&gt;.</returns>
        List<string> GetCompanies();

        /// <summary>
        /// 取得鄉鎮市區.
        /// </summary>
        /// <returns>List&lt;System.String&gt;.</returns>
        List<string> GetDistricts();
    }
}