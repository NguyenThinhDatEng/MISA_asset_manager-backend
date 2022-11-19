using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.QLTS.Common.Enums;
using MISA.QLTS.DL;
using MySqlConnector;

namespace MISA.QLTS.COMMON.Controllers
{
    #region Method
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FixedAssetCategoriesController : ControllerBase
    {
        #region Field
        private IFixedAssetCategoryBL _fixedAssetCategoryBL;
        #endregion

        #region Constructor
        public FixedAssetCategoriesController(IFixedAssetCategoryBL fixedAssetCategoryBL)
        {
            _fixedAssetCategoryBL = fixedAssetCategoryBL;
        }
        #endregion

        /// <summary>
        /// Lấy thông tin tất cả loại tài sản
        /// </summary>
        /// <returns>Danh sách loại tài sản</returns>
        /// Create by: NVThinh (16/11/2022)
        [HttpGet]
        public IActionResult GetAllFixedAssets()
        {
            try
            {
                // Gán dữ liệu trả về từ Business layer
                var fixedAssetCategories = _fixedAssetCategoryBL.GetAllFixedAssetCategory();
                // Xử lý kết quả trả về
                if (fixedAssetCategories != null)
                // Thành công
                    return StatusCode(StatusCodes.Status200OK, fixedAssetCategories);
                // Thất bại
                return StatusCode(StatusCodes.Status400BadRequest, new
                {
                    ErrorCode = QLTSErrorCode.NotFound,
                    DevMsg = "Bad request",
                    UserMsg = "Không tìm thấy dữ liệu"
                });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ErrorCode = QLTSErrorCode.Exception,
                    DevMsg = "Catched an exception",
                    User = "Vui lòng liên hệ MISA",
                    MoreInfo = "https://openapi.misa.com.vn/errorcode/e001",
                    TraceId = HttpContext.TraceIdentifier
                });
            }
        }
    } 
    #endregion
}
