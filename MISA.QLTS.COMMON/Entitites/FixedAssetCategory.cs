namespace MISA.QLTS.Common.Entitites
{
    /// <summary>
    /// Thông tin loại tài sản
    /// </summary>
    public class FixedAssetCategory
    {
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
    }
}
