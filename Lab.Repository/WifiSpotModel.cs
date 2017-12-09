using System;

namespace Lab.Repository
{
    public class WifiSpotModel
    {
        /// <summary>
        /// 熱點代碼
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 熱點名稱
        /// </summary>
        public string Spot_Name { get; set; }

        /// <summary>
        /// 熱點類別
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 業者
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// 鄉鎮市區
        /// </summary>
        public string District { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 機關構名稱
        /// </summary>
        public string Apparatus_Name { get; set; }

        /// <summary>
        /// 緯度
        /// </summary>
        public string Latitude { get; set; }

        /// <summary>
        /// 經度
        /// </summary>
        public string Longitude { get; set; }

        /// <summary>
        /// twd97X座標
        /// </summary>
        public string TWD97X { get; set; }

        /// <summary>
        /// twd97Y座標
        /// </summary>
        public string TWD97Y { get; set; }

        /// <summary>
        /// wgs84a緯度
        /// </summary>
        public string WGS84aX { get; set; }

        /// <summary>
        /// wgs84a經度
        /// </summary>
        public string WGS84aY { get; set; }
    }
}