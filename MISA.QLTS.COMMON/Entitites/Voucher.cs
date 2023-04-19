using MISA.QLTS.Common.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MISA.QLTS.Common.Entitites
{
    public class Voucher : BaseEntity
    {
        #region Property

        /// <summary>
        /// ID chứng từ
        /// </summary>
        [Key]
        public Guid? voucher_id { get; set; }

        /// <summary>
        /// Mã chứng từ
        /// </summary>
        [RequiredField(field: "Mã chứng từ")]
        public string? voucher_code { get; set; }

        /// <summary>
        /// Ngày chứng từ
        /// </summary>
        [RequiredField(field: "Ngày chứng từ")]
        public DateTime voucher_date { get; set; }

        /// <summary>
        /// Ngày ghi tăng
        /// </summary>
        [RequiredField(field: "Ngày ghi tăng")]
        public DateTime increment_date { get; set; }

        /// <summary>
        /// Tổng nguyên giá
        /// </summary>
        public double total_of_cost { get; set; }

        /// <summary>
        /// Ghi chú
        /// </summary>
        public string? description { get; set; }

        #endregion
    }
}
