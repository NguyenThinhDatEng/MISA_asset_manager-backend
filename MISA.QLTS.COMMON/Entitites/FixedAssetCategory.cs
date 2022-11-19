namespace MISA.QLTS.Common.Entitites
{
    /// <summary>
    /// Thông tin loại tài sản
    /// </summary>
    public class FixedAssetCategory
    {
        #region Property
        /// <summary>
        /// ID loại tài sản
        /// </summary>
        public Guid fixed_asset_category_id { get; set; }

        /// <summary>
        /// Mã loại tài sản
        /// </summary>
        public string fixed_asset_category_code { get; set; }

        /// <summary>
        /// Tên loại tài sản
        /// </summary>
        public string fixed_asset_category_name { get; set; }

        /// <summary>
        /// Năm bắt đầu theo dõi
        /// </summary>
        public int? life_time { get; set; }

        /// <summary>
        /// Ti le hao mon
        /// </summary>
        public float? depreciation_rate { get; set; }

        /// <summary>
        /// Ngay tao thong tin
        /// </summary>
        public DateTime created_date { get; set; }

        /// <summary>
        /// Nguoi tao thong tin
        /// </summary>
        public string created_by { get; set; }

        /// <summary>
        /// Ngay chinh sua thong tin
        /// </summary>
        public DateTime modified_date { get; set; }

        /// <summary>
        /// Nguoi chinh sua thong tin
        /// </summary>
        public string modified_by { get; set; }
        #endregion
    }
}
