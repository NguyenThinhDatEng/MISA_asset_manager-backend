using MISA.QLTS.Common.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Common.Entitites
{
    public class VoucherDetail: BaseEntity
    {
        #region Property

        /// <summary>
        /// ID chi tiết chứng từ
        /// </summary>
        [Key]
        public Guid? voucher_detail_id { get; set; }

        /// <summary>
        /// ID chứng từ
        /// </summary>
        [RequiredField(field: "ID chứng từ")]
        public Guid? voucher_id { get; set; }

        /// <summary>
        /// ID tài sản cố định
        /// </summary>
        [RequiredField(field: "ID tài sản")]
        public Guid? fixed_asset_id { get; set; }

        /// <summary>
        /// Ngân sách
        /// </summary>
        public string? budgets { get; set; }

        #endregion
    }
}
