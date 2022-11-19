namespace MISA.QLTS.Common.Entitites
{
    /// <summary>
    /// Thong tin tai san
    /// </summary>
    public class FixedAsset
    {
        #region Property
        /// <summary>
        /// ID tai san
        /// </summary>
        public Guid? fixed_asset_id { get; set; }

        /// <summary>
        /// Ma tai san
        /// </summary>
        public string fixed_asset_code { get; set; }

        /// <summary>
        /// Ten tai san
        /// </summary>
        public string fixed_asset_name { get; set; }

        /// <summary>
        /// ID bo phan su dung
        /// </summary>
        public Guid? department_id { get; set; }

        /// <summary>
        /// Ma bo phan su dung
        /// </summary>
        public string department_code { get; set; }

        /// <summary>
        /// Ten bo phan su dung
        /// </summary>
        public string department_name { get; set; }

        /// <summary>
        /// ID loai tai san
        /// </summary>
        public Guid? fixed_asset_category_id { get; set; }

        /// <summary>
        /// Ma loai tai san
        /// </summary>
        public string fixed_asset_category_code { get; set; }

        /// <summary>
        /// Ten loai tai san
        /// </summary>
        public string fixed_asset_category_name { get; set; }

        /// <summary>
        /// Ngay mua
        /// </summary>
        public DateTime purchase_date { get; set; }

        /// <summary>
        /// Năm bắt đầu theo dõi
        /// </summary>
        public int? tracked_year { get; set; }

        /// <summary>
        /// Năm bắt đầu theo dõi
        /// </summary>
        public int? life_time { get; set; }

        /// <summary>
        /// Nguyen gia
        /// </summary>
        public double? cost { get; set; }

        /// <summary>
        /// So luong
        /// </summary>
        public int? quantity { get; set; }

        /// <summary>
        /// Ti le hao mon/khau hao
        /// </summary>
        public float? depreciation_rate { get; set; }

        /// <summary>
        /// Ngay su dung tai san
        /// </summary>
        public DateTime production_date { get; set; }

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
