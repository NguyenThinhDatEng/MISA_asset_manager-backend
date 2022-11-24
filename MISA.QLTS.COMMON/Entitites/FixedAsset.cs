

using MISA.QLTS.Common.Attributes;
using MISA.QLTS.Common.Enums;
using MISA.QLTS.Common.Resources;
using System.ComponentModel.DataAnnotations;

namespace MISA.QLTS.Common.Entitites
{
    /// <summary>
    /// Thong tin tai san
    /// </summary>
    public class FixedAsset : BaseEntity
    {
        #region Property

        /// <summary>
        /// ID tai san
        /// </summary>
        [Key]
        public Guid? fixed_asset_id { get; set; }

        /// <summary>
        /// Ma tai san
        /// </summary>
        [RequiredField(field: "Mã tài sản")]
        [Form("Mã tài sản")] // Định dạng mã tài sản
        public string fixed_asset_code { get; set; }

        /// <summary>
        /// Ten tai san
        /// </summary>
        [RequiredField(field: "Tên tài sản")]
        public string fixed_asset_name { get; set; }

        /// <summary>
        /// ID bo phan su dung
        /// </summary>
        [RequiredField(field: "ID bộ phận sử dụng")]
        public Guid? department_id { get; set; }

        /// <summary>
        /// Ma bo phan su dung
        /// </summary>
        [RequiredField(field: "Mã bộ phận sử dụng")]
        [Form("Mã bộ phận sử dụng")] // Định dạng mã bộ phận sử dụng
        public string department_code { get; set; }

        /// <summary>
        /// Ten bo phan su dung
        /// </summary>
        [RequiredField(field: "Tên bộ phận sử dụng")]
        public string department_name { get; set; }

        /// <summary>
        /// ID loai tai san
        /// </summary>
        [RequiredField(field: "ID loại tài sản")]
        public Guid? fixed_asset_category_id { get; set; }

        /// <summary>
        /// Ma loai tai san
        /// </summary>
        [RequiredField(field: "Mã loại tài sản")]
        [Form("Mã loại tài sản")] // Định dạng mã loại tài sản
        public string fixed_asset_category_code { get; set; }

        /// <summary>
        /// Ten loai tai san
        /// </summary>
        [RequiredField(field: "Tên loại tài sản")]
        public string fixed_asset_category_name { get; set; }

        /// <summary>
        /// Ngay mua
        /// </summary>
        [RequiredField(field: "Ngày mua")]
        [Date(field:"Ngày mua")]
        public DateTime purchase_date { get; set; }

        /// <summary>
        /// Năm bắt đầu theo dõi
        /// </summary>
        [RequiredField(field: "Năm theo dõi", dataType = QLTSType.Number)]
        public int? tracked_year { get; set; }

        /// <summary>
        /// Số năm sử dụng
        /// </summary>
        [RequiredField(field: "Số năm sử dụng", dataType = QLTSType.Number)]
        public int? life_time { get; set; }

        /// <summary>
        /// Nguyen gia
        /// </summary>
        [RequiredField(field: "Nguyên giá", dataType = QLTSType.Number)]
        public double? cost { get; set; }

        /// <summary>
        /// So luong
        /// </summary>
        [RequiredField(field: "Số lượng", dataType = QLTSType.Number)]
        public int? quantity { get; set; }

        /// <summary>
        /// Ti le hao mon/khau hao
        /// </summary>
        [RequiredField(field: "Tỉ lệ hao mòn", dataType = QLTSType.Number)]
        public float? depreciation_rate { get; set; }

        /// <summary>
        /// Ngay su dung tai san
        /// </summary>
        [RequiredField(field: "Ngày sử dụng")]
        [Date(field: "Ngày sử dụng")]
        public DateTime production_date { get; set; } 
        #endregion
    }
}
