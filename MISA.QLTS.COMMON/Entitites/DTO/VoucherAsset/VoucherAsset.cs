using MISA.QLTS.Common.Attributes;
using MISA.QLTS.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Common.Entitites.DTO.VoucherAsset
{
    /// <summary>
    /// Thông tin cần thiết của tài sản đã chứng từ
    /// </summary>
    public class VoucherAsset
    {
        #region Property

        /// <summary>
        /// ID tai san
        /// </summary>
        public Guid? fixed_asset_id { get; set; }

        /// <summary>
        /// Ma tai san
        /// </summary>
        public string? fixed_asset_code { get; set; }

        /// <summary>
        /// Ten tai san
        /// </summary>
        public string? fixed_asset_name { get; set; }

        /// <summary>
        /// Ten bo phan su dung
        /// </summary>
        public string? department_name { get; set; }

        /// <summary>
        /// Số năm sử dụng
        /// </summary>
        public int? life_time { get; set; }

        /// <summary>
        /// Nguyen gia
        /// </summary>
        public double? cost { get; set; }

        /// <summary>
        /// Ti le hao mon/khau hao
        /// </summary>
        public float? depreciation_rate { get; set; }

        /// <summary>
        /// Ngay su dung tai san
        /// </summary>
        public DateTime production_date { get; set; }

        /// <summary>
        /// Ngân sách tài sản
        /// </summary>
        public string? budgets { get; set; }

        #endregion
    }
}
