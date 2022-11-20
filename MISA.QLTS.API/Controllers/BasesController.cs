using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.QLTS.BL;
using MISA.QLTS.Common.Entitites.DTO;
using MISA.QLTS.Common.Entitites;
using MISA.QLTS.Common.Enums;
using MISA.QLTS.DL;
using MySqlConnector;

namespace MISA.QLTS.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasesController<T> : ControllerBase
    {
        #region Field

        private IBaseBL<T> _baseBL;

        #endregion

        #region Constructor

        public BasesController(IBaseBL<T> baseBL)
        {
            _baseBL = baseBL;
        }

        #endregion

        #region GET

        /// <summary>
        /// API Lấy tất cả bản ghi
        /// </summary>
        /// <returns>Danh sách tất cả bản ghi</returns>
        /// Author: Nguyen Van Thinh 11/11/2022
        [HttpGet]
        public IActionResult GetAllRecords()
        {
            try
            {
                // Gọi đến Business Layer
                var fixedAssetList = _baseBL.GetAllRecords();
                // Thành công
                if (fixedAssetList != null)
                    return StatusCode(StatusCodes.Status200OK, fixedAssetList);
                // Thất bại
                return StatusCode(StatusCodes.Status404NotFound, new
                {
                    ErrorCode = QLTSErrorCode.NotFound,
                    DevMsg = "Not Found",
                    UserMsg = "Không tìm thấy dữ liệu"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ErrorCode = QLTSErrorCode.Exception,
                    DevMsg = "Catched an exception!",
                    UserMsg = "Vui lòng liên hệ MISA",
                    MoreInfo = ex.Message,
                    TraceID = HttpContext.TraceIdentifier,
                });
            }
        }

        /// <summary>
        /// Lấy thông tin bản ghi theo ID
        /// </summary>
        /// <param name="recordID">ID bản ghi muốn lấy</param>
        /// <returns>Thông tin bản ghi theo ID</returns>
        /// Create by: NVThinh (16/11/2022)
        [HttpGet("{recordID}")]
        public IActionResult GetFixedAssetByID([FromRoute] Guid recordID)
        {
            try
            {
                // Gọi đến Business Layer
                var record = _baseBL.GetByID(recordID);

                // Xử lý kết quả trả về
                if (record != null)
                    // Thành công
                    return StatusCode(StatusCodes.Status200OK, record);
                // Thất bại
                return StatusCode(StatusCodes.Status404NotFound, new
                {
                    ErrorCode = QLTSErrorCode.NotFound,
                    DevMsg = "Not Found",
                    UserMsg = "Không tìm thấy bản ghi"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ErrorCode = QLTSErrorCode.Exception,
                    DevMsg = "Catched an exception!",
                    UserMsg = "Vui lòng liên hệ MISA",
                    moreInfo = ex.Message,
                    TraceID = HttpContext.TraceIdentifier,
                });
            }

        }

        #endregion
    }
}
