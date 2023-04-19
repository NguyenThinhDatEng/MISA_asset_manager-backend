using MISA.QLTS.Common.Entitites;
using MISA.QLTS.Common.Entitites.DTO;
using MISA.QLTS.Common.Entitites.DTO.VoucherAsset;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.DL
{
    public interface IVoucherDL : IBaseDL<Voucher>
    {
        /// <summary>
        /// API lấy chứng từ theo bộ lọc và phân trang
        /// </summary>
        /// <param name="keyword">Từ khóa tìm kiếm</param>
        /// <param name="offset">vị trí của bản ghi bắt đầu lấy</param>
        /// <param name="limit">số bản ghi lấy ra</param>
        /// <returns>Danh sách chứng từ và tổng số bản ghi thu được</returns>
        /// <author>NVThinh 9/1/2023</author>
        public PagingResult<Voucher> GetVouchersByFilterAndPaging(
            string? keyword,
            int offset,
            int limit);

        /// <summary>
        /// API lấy tất cả tài sản đã đăng ký chứng từ
        /// </summary>
        /// <param name="voucher_id">ID voucher</param>
        /// <returns>Danh sách các tài sản đăng ký voucher đó</returns>
        /// <author>NVThinh 10/1/2023</author>
        public List<VoucherAsset> GetVoucherDetail(Guid voucher_id);

        /// <summary>
        /// Thêm mới chứng từ
        /// </summary>
        /// <param name="voucherResult">Đối tượng chứa chứng từ và mảng các voucher detail</param>
        /// <returns></returns>
        /// <author>NVThinh 12/1/2023</author>
        public ServiceResponse InsertVoucher(VoucherResult voucherResult);

        /// <summary>
        /// Cập nhật chứng từ
        /// </summary>
        /// <param name="voucherResult">Mảng các chi tiết chứng từ</param>
        /// <returns>1 đối tượng ServiceRespone</returns>
        /// <author>NVThinh 13/1/2023</author>
        public ServiceResponse UpdateVoucher( VoucherResult voucherResult);
    }
}
