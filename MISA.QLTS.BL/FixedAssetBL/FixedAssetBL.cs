using MISA.QLTS.Common;
using MISA.QLTS.Common.Attributes;
using MISA.QLTS.Common.Entitites;
using MISA.QLTS.Common.Entitites.DTO;
using MISA.QLTS.Common.Enums;
using MISA.QLTS.Common.Resources;
using MISA.QLTS.DL;
using System.Reflection;

namespace MISA.QLTS.BL
{
    public class FixedAssetBL : BaseBL<FixedAsset>, IFixedAssetBL
    {
        #region Field

        private IFixedAssetDL _fixedAssetDL;

        #endregion

        #region Constructor

        public FixedAssetBL(IFixedAssetDL fixedAssetDL) : base(fixedAssetDL)
        {
            _fixedAssetDL = fixedAssetDL;
        }

        #endregion

        #region Method

        #region GET

        /// <summary>
        /// API lấy mã tài sản cố định mới
        /// </summary>
        /// <returns>Mã tài sản cố định mới</returns>
        /// Author: NVThinh 16/11/2022
        public string GetMaxFixedAssetCode()
        {
            return _fixedAssetDL.GetNextCode();
        }

        /// <summary>
        /// API lấy tài sản theo bộ lọc và phân trang
        /// </summary>
        /// <param name="keyword">Từ khóa tìm kiếm</param>
        /// <param name="departmentID">Mảng ID bộ phận sử dụng</param>
        /// <param name="fixedAssetCategoryID">Mảng ID mã bộ phận sử dụng</param>
        /// <param name="offset">vị trí của bản ghi bắt đầu lấy</param>
        /// <param name="limit">số bản ghi lấy ra</param>
        /// <param name="isIncrement">true nếu danh sách là ghi tăng chứng từ</param>
        /// <param name="selectedIDs">mảng chứa các ID đã được lựa chọn để ghi tăng chứng từ</param>
        /// <returns>Danh sách tài sản cố định và tổng số bản ghi</returns>
        /// <author>NVThinh 11/1/2023</author>
        public PagingResult<FixedAsset> GetFixedAssetByFilterAndPaging(string? keyword, Guid? departmentID, Guid? fixedAssetCategoryID, int offset, int limit, bool isIncrement, List<Guid>? selectedIDs)
        {
            return _fixedAssetDL.GetFixedAssetByFilterAndPaging(keyword, departmentID, fixedAssetCategoryID, offset, limit, isIncrement, selectedIDs);
        }

        /// <summary>
        /// Kiểm tra các tài sản đã có chứng từ chưa
        /// </summary>
        /// <param name="fixedAssetIDs">Danh sách ID tài sản</param>
        /// <returns>Mã chứng từ</returns>
        /// <author>NVThinh 16/1/2023</author>
        public string CheckExistedVoucher(List<Guid> fixedAssetIDs) 
        {
            return _fixedAssetDL.CheckExistedVoucher(fixedAssetIDs);
        }

        #endregion

        #region POST

        /// <summary>
        /// API Tạo mới tài sản cố định
        /// </summary>
        /// <param name="fixedAsset">Đối tượng tài sản cố định</param>
        /// <returns>1 đối tượng ServiceRespone</returns>
        /// Author: NVThinh (11/11/2022)
        public ServiceResponse InsertFixedAsset(FixedAsset fixedAsset)
        {
            var validateResult = ValidateRequestData(fixedAsset);
            if (!validateResult.Success)
                return validateResult;
            return _fixedAssetDL.InsertFixedAsset(fixedAsset);
        }

        #endregion

        #region PUT

        /// <summary>
        /// API cập nhật thông tin tài sản
        /// </summary>
        /// <param name="fixedAssetID">ID tài sản được cập nhật</param>
        /// <param name="fixedAsset">Đối tượng tài sản cố định</param>
        /// <returns>ID bản ghi được cập nhật</returns>
        /// Author: NVThinh (11/11/2022)
        public ServiceResponse UpdateFixedAsset(Guid fixedAssetID, FixedAsset fixedAsset)
        {
            var validateResult = ValidateRequestData(fixedAsset);
            if (!validateResult.Success)
                return validateResult;
            return _fixedAssetDL.UpdateFixedAsset(fixedAssetID, fixedAsset);
        }

        #endregion

        /// <summary>
        /// Validate dữ liệu 
        /// </summary>
        /// <param name="fixedAsset">Đối tượng tài sản cố định</param>
        /// <returns>Data transfer object</returns>
        /// Author: NVThinh 23/11/2022
        public ServiceResponse ValidateRequestData(FixedAsset fixedAsset)
        {
            try
            {
                // Khởi tạo mảng các lỗi gặp phải
                var validateFailures = new List<string>();
                // Khởi tạo các biến
                var recordCode = fixedAsset.fixed_asset_code;
                var recordID = fixedAsset.fixed_asset_id;
                recordID = recordID != null ? (Guid)recordID : Guid.Empty;
                // Kiểm tra mã trùng
                if (_fixedAssetDL.CheckDuplicateCode(recordCode, (Guid)recordID, "fixed_asset_id"))
                {
                    validateFailures.Add(Errors.UserMsg_Duplicate_Key);
                }

                // Lấy danh sách các properties của đối tượng
                var properties = typeof(FixedAsset).GetProperties();

                // Duyệt qua từng property và kiểm tra C# attributes
                foreach (var property in properties)
                {
                    // lấy giá trị của đối tượng theo từng property
                    var propertyValue = property.GetValue(fixedAsset);

                    // Kiểm tra xem property có C# attribute bắt buộc không
                    var validationAttribute = (RequiredFieldAttribute?)Attribute.GetCustomAttribute(property, typeof(RequiredFieldAttribute));

                    // Nếu thuộc tính là bắt buộc và giá trị truyền vào là rỗng hoặc null
                    if (validationAttribute != null && !validationAttribute.IsValid(propertyValue?.ToString()))
                    {
                        validateFailures.Add(validationAttribute.ErrorMessage);
                    }

                    // Kiểm tra dữ liệu kiểu thời gian có lớn hơn thời gian hiện tại không
                    var dateAttribute = (DateAttribute?)Attribute.GetCustomAttribute(property, typeof(DateAttribute));
                    // Nếu thuộc tính là kiểu số nguyên và giá trị truyền vào có ký tự không phải số
                    if (dateAttribute != null && !dateAttribute.IsValid(propertyValue?.ToString()))
                    {
                        validateFailures.Add(dateAttribute.ErrorMessage);
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
