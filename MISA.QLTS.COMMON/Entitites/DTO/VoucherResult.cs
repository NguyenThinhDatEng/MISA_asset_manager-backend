using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MISA.QLTS.Common.Entitites.DTO.VoucherAsset;

namespace MISA.QLTS.Common.Entitites.DTO
{
    public class VoucherResult
    {
        /// <summary>
        /// Đối tượng chứng từ
        /// </summary>
        public Voucher voucher { get; set; }

        /// <summary>
        /// Danh sách các voucher detail cần tạo mới
        /// </summary>
        public List<VoucherDetail> voucherDetailList { get; set; }

        /// <summary>
        /// Danh sách các tài sản cùng nguyên giá cần cập nhật
        /// </summary>
        public List<Cost> costList { get; set; }
    }
}
