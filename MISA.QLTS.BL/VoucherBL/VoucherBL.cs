using MISA.QLTS.Common.Attributes;
using MISA.QLTS.Common.Entitites;
using MISA.QLTS.Common.Entitites.DTO;
using MISA.QLTS.Common.Entitites.DTO.VoucherAsset;
using MISA.QLTS.Common.Enums;
using MISA.QLTS.Common.Resources;
using MISA.QLTS.DL;
using System.Reflection;

namespace MISA.QLTS.BL
{
    public class VoucherBL : BaseBL<Voucher>, IVoucherBL
    {
        #region Field

        private IVoucherDL _voucherDL;

        #endregion

        public VoucherBL(IVoucherDL voucherDL) : base(voucherDL)
        {
            _voucherDL = voucherDL;
        }

        #region Method

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
            int limit)
        {
            return _voucherDL.GetVouchersByFilterAndPaging(keyword, offset, limit);
        }

        /// <summary>
        /// API lấy tất cả tài sản đã đăng ký chứng từ
        /// </summary>
        /// <param name="voucher_id">ID voucher</param>
        /// <returns>Danh sách các tài sản đăng ký voucher đó</returns>
        /// <author>NVThinh 10/1/2023</author>
        public List<VoucherAsset> GetVoucherDetail(Guid voucher_id)
        {
            return _voucherDL.GetVoucherDetail(voucher_id);
        }

        /// <summary>
        /// Thêm mới chứng từ
        /// </summary>
        /// <param name="voucherResult">Đối tượng chứa chứng từ và mảng các voucher detail</param>
        /// <returns>1 đối tượng ServiceRespone</returns>
        /// <author>NVThinh 12/1/2023</author>
        public ServiceResponse InsertVoucher(VoucherResult voucherResult)
        {
            var validateResult = ValidateRequestData(voucherResult.voucher);
            if (!validateResult.Success)
                return validateResult;
            return _voucherDL.InsertVoucher(voucherResult);
        }

        /// <summary>
        /// Cập nhật chứng từ
        /// </summary>
        /// <param name="voucherResult">Đối tượng chứa chứng từ và mảng các voucher detail</param>
        /// <returns>1 đối tượng ServiceRespone</returns>
        /// <author>NVThinh 13/1/2023</author>
        public ServiceResponse UpdateVoucher(VoucherResult voucherResult)
        {
            var validateResult = ValidateRequestData(voucherResult.voucher);
            if (!validateResult.Success)
                return validateResult;
            return _voucherDL.UpdateVoucher(voucherResult);
        }

        /// <summary>
        /// Validate dữ liệu 
        /// </summary>
        /// <param name="voucher">Đối tượng cần validate</param>
        /// <returns>Data transfer object</returns>
        /// Author: NVThinh 23/11/2022
        public ServiceResponse ValidateRequestData(Voucher voucher)
        {
            try
            {
                // Khởi tạo mảng các lỗi gặp phải
                var validateFailures = new List<string>();
                // Khởi tạo các biến
                var recordID = voucher.voucher_id;
                var recordCode = voucher.voucher_code;
                recordID = recordID != null ? (Guid)recordID : Guid.Empty;
                // Kiểm tra mã trùng
                if (_voucherDL.CheckDuplicateCode(recordCode, (Guid)recordID, "voucher_id"))
                {
                    validateFailures.Add(Errors.UserMsg_Duplicate_Key);
                }

                // Lấy danh sách các properties của đối tượng
                var properties = typeof(Voucher).GetProperties();

                // Duyệt qua từng property và kiểm tra C# attributes
                foreach (var property in properties)
                {
                    // lấy giá trị của đối tượng theo từng property
                    var propertyValue = property.GetValue(voucher);

                    // Kiểm tra xem property có C# attribute bắt buộc không
                    var validationAttribute = (RequiredFieldAttribute?)Attribute.GetCustomAttribute(property, typeof(RequiredFieldAttribute));

                    // Nếu thuộc tính là bắt buộc và giá trị truyền vào là rỗng hoặc null
                    if (validationAttribute != null && !validationAttribute.IsValid(propertyValue?.ToString()))
                    {
                        validateFailures.Add(validationAttribute.ErrorMessage);
                    }
                }
                // Xử lý kết quả
                if (validateFailures.Count > 0)
                {
                    return new ServiceResponse
                    {
                        Success = false,
                        ErrorCode = QLTSErrorCode.BadRequest,
                        Data = validateFailures,
                    };
                }
                return new ServiceResponse { Success = true };
            }
            catch (AmbiguousMatchException ex)
            {
                return new ServiceResponse
                {
                    Success = false,
                    ErrorCode = QLTSErrorCode.Exception,
                    Data = new List<string> { ex.Message, Errors.UserMsg_Multiple_Attributes_Same_Type }
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse
                {
                    Success = false,
                    ErrorCode = QLTSErrorCode.Exception,
                    Data = new List<string> { ex.Message, Errors.UserMsg_Exception }
                };
            }
        }

        #endregion
    }
}
