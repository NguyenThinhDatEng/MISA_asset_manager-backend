using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Common.Entitites.DTO.VoucherAsset
{
    /// <summary>
    /// Thông tin nguyên giá của tài sản
    /// </summary>
    public class Cost
    {
        /// <summary>
        /// ID tài sản cố định
        /// </summary>
        public Guid fixed_asset_id { get; set; }

        /// <summary>
        /// Nguyên giá tài sản cố định
        /// </summary>
        public double cost { get; set; }

    }
}
