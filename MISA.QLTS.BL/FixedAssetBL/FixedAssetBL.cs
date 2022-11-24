using MISA.QLTS.Common;
using MISA.QLTS.Common.Attributes;
using MISA.QLTS.Common.Entitites;
using MISA.QLTS.Common.Entitites.DTO;
using MISA.QLTS.Common.Enums;
using MISA.QLTS.Common.Resources;
using MISA.QLTS.DL;
using System.ComponentModel.DataAnnotations;
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
        /// created by: NVThinh 16/11/2022
        public string GetMaxFixedAssetCode()
        {
            return _fixedAssetDL.GetMaxFixedAssetCode();
        }

        /// <summary>
        /// API lấy tài sản theo bộ lọc và phân trang
        /// </summary>
        /// <param name="keyword">Từ khóa tìm kiếm</param>
        /// <param name="departmentID">Mảng ID bộ phận sử dụng</param>
        /// <param name="fixedAssetCategoryID">Mảng ID mã bộ phận sử dụng</param>
        /// <param name="offset">vị trí của bản ghi bắt đầu lấy</param>
        /// <param name="limit">số bản ghi lấy ra</param>
        /// <returns>Danh sách tài sản cố định và tổng số bản ghi</returns>
        public PagingResult GetFixedAssetByFilterAndPaging(string? keyword, Guid? departmentID, Guid? fixedAssetCategoryID, int offset = 0, int limit = 20)
        {
            return _fixedAssetDL.GetFixedAssetByFilterAndPaging(keyword, departmentID, fixedAssetCategoryID, offset, limit);
        }

        #endregion

        #region POST

        /// <summary>
        /// API Tạo mới tài sản cố định
        /// </summary>
        /// <param name="fixedAsset">Đối tượng tài sản cố định</param>
        /// <returns>ID tài sản cố định được thêm</returns>
        /// Created by: NVThinh (11/11/2022)
        public ServiceResponse InsertFixedAsset(FixedAsset fixedAsset)
        {
            var validateResult = ValidateRequestData(fixedAsset);
            if (!validateResult.Success)
                return validateResult;
            return _fixedAssetDL.InsertFixedAsset(fixedAsset);
        }

        /// <summary>
        /// Xóa nhiều bản ghi
        /// </summary>
        /// <param name="listFixedAssetID">Danh sách ID các tài sản cần xóa</param>
        /// <returns>Số lượng tài sản được xóa</returns>
        /// Created by: NVThinh (11/11/2022)
        public ServiceResponse DeleteMultipleFixedAsset(ListFixedAssetID fixedAssetIDs)
        {
            return _fixedAssetDL.DeleteMultipleFixedAsset(fixedAssetIDs);
        }

        #endregion

        #region PUT

        /// <summary>
        /// API cập nhật thông tin tài sản
        /// </summary>
        /// <param name="fixedAssetID">ID tài sản được cập nhật</param>
        /// <param name="fixedAsset">Đối tượng tài sản cố định</param>
        /// <returns>ID bản ghi được cập nhật</returns>
        /// Created by: NVThinh (11/11/2022)
        public int UpdateFixedAsset(Guid fixedAssetID, FixedAsset fixedAsset)
        {
            return _fixedAssetDL.UpdateFixedAsset(fixedAssetID, fixedAsset);
        }

        #endregion

        #region DELETE

        /// <summary>
        /// API Xóa 01 tài sản
        /// </summary>
        /// <param name="fixedAssetID">ID tài sản cần xóa</param>
        /// <returns>ID tài sản được xóa</returns>
        /// Created by: NVThinh (11/11/2022)
        public int DeleteFixedAsset(Guid fixedAssetID)
        {
            return _fixedAssetDL.DeleteFixedAsset(fixedAssetID);
        }

        #endregion

        /// <summary>
        /// Validate dữ liệu 
        /// </summary>
        /// <param name="fixedAsset">Đối tượng tài sản cố định</param>
        /// <returns>Data transfer object</returns>
        /// Created by: NVThinh 23/11/2022
        public ServiceResponse ValidateRequestData(FixedAsset fixedAsset)
        {
            try
            {
                // Lấy danh sách các properties của đối tượng
                var properties = typeof(FixedAsset).GetProperties(); // an array

                // Duyệt qua từng property và kiểm tra C# attributes
                var validateFailures = new List<string>(); // mảng các lỗi gặp phải
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

                    // Kiểm tra mã dữ liệu có đúng định dạng không
                    var formAttribute = (FormAttribute?)Attribute.GetCustomAttribute(property, typeof(FormAttribute));
                    // Nếu property có thuộc tính Form và có định dạng hợp lệ
                    if (formAttribute != null && !formAttribute.IsValid(propertyValue?.ToString()))
                    {
                        validateFailures.Add(formAttribute.ErrorMessage);
                    }
                }
                // Xử lý kết quả
                if (validateFailures.Count > 0)
                {
                    return new ServiceResponse
                    {
                        Success = false,
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
                    Data = new List<string> { ex.Message, Errors.UserMsg_Multiple_Attributes_Same_Type }
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Data = new List<string> { ex.Message, Errors.UserMsg_Wrong_Data_Type }
                };
            }
        }

        #endregion
    }
}
