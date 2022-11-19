using Dapper;
using Microsoft.AspNetCore.Mvc;
using MISA.QLTS.BL;
using MISA.QLTS.Common.Enums;
using MISA.QLTS.Common.Entitites;
using MISA.QLTS.Common.Entitites.DTO;
using MySqlConnector;
using MISA.QLTS.DL;

namespace MISA.QLTS.COMMON.Controllers
{
    [Route("api/v1/[controller]")]  // attribute 01
    [ApiController] // attribute 02
    public class FixedAssetsController : ControllerBase
    {
        #region Field

        private IFixedAssetBL _fixedAssetBL;

        #endregion

        #region Constructor

        public FixedAssetsController(IFixedAssetBL fixedAssetBL)
        {
            _fixedAssetBL = fixedAssetBL;
        }

        #endregion

        #region Method

        #region GET

        /// <summary>
        /// API Lấy tất cả tài sản cố định
        /// </summary>
        /// <returns>Danh sách tất cả tài sản cố định</returns>
        /// Author: Nguyen Van Thinh 11/11/2022
        [HttpGet]
        public IActionResult GetAllFixedAssets()
        {
            try
            {
                // Gọi đến Business Layer
                var fixedAssetList = _fixedAssetBL.GetAllFixedAsset();
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
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ErrorCode = QLTSErrorCode.Exception,
                    DevMsg = "Catched an exception!",
                    UserMsg = "Vui lòng liên hệ MISA",
                    TraceID = HttpContext.TraceIdentifier,
                });
            }
        }

        /// <summary>
        /// API Lấy thông tin 1 tài sản cố định theo ID
        /// </summary>
        /// <param name="fixedAssetID">ID tài sản muốn lấy thông tin</param>
        /// <returns>Thông tin tài sản</returns>
        [HttpGet("{fixedAssetID}")]
        public IActionResult GetFixedAssetByID([FromRoute] Guid fixedAssetID)
        {
            try
            {
                // Gọi đến Business Layer
                var fixedAsset = _fixedAssetBL.GetFixedAssetByID(fixedAssetID);
                // Xử lý kết quả trả về
                if (fixedAsset != null)
                    // Thành công
                    return StatusCode(StatusCodes.Status200OK, fixedAsset);
                // Thất bại
                return StatusCode(StatusCodes.Status404NotFound, new
                {
                    ErrorCode = QLTSErrorCode.NotFound,
                    DevMsg = "Not Found",
                    UserMsg = "Không tìm thấy bản ghi"
                });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ErrorCode = QLTSErrorCode.Exception,
                    DevMsg = "Catched an exception!",
                    UserMsg = "Vui lòng liên hệ MISA",
                    TraceID = HttpContext.TraceIdentifier,
                });
            }

        }

        /// <summary>
        /// API lấy tài sản theo bộ lọc và phân trang
        /// </summary>
        /// <param name="keyword">Từ khóa tìm kiếm</param>
        /// <param name="departmentID">ID bộ phận sử dụng</param>
        /// <param name="fixedAssetCategoryID">ID mã bộ phận sử dụng</param>
        /// <param name="offset">vị trí của bản ghi bắt đầu lấy</param>
        /// <param name="limit">số bản ghi lấy ra</param>
        /// <returns>Danh sách tài sản cố định</returns>
        /// <returns>Số bản ghi phù hợp</returns>
        [HttpGet("filter")]
        public IActionResult GetFixedAssetByFilterAndPaging(
            [FromQuery] string? keyword,
            [FromQuery] Guid? departmentID,
            [FromQuery] Guid? fixedAssetCategoryID,
            [FromQuery] int offset = 0,
            [FromQuery] int limit = 20)
        {
            try
            {
                //Khởi tạo kết nối DB MySQL
                var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString);

                // Chuẩn bị câu lệnh SQL
                string procedureName = "Proc_GetAssetPaging";

                // Chuẩn bị tham số đầu vào cho stored procedure
                var parameters = new DynamicParameters();


                return StatusCode(StatusCodes.Status200OK, new PagingResult
                {
                    Data = new List<FixedAsset>(),
                    totalOfRecords = 20
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ErrorCode = 1,
                    DevMsg = "Catched an exception",
                    UserMsg = "Vui lòng liên hệ MISA",
                    MoreInfo = "https://openapi.misa.com.vn/ErrorCode/e001",
                });
            }
        }

        /// <summary>
        /// API lấy mã tài sản cố định mới
        /// </summary>
        /// <returns>Mã tài sản cố định mới</returns>
        /// created by: NVThinh 16/11/2022
        [HttpGet("newAssetCode")]
        public IActionResult GetMaxFixedAssetCode()
        {
            try
            {
                // Gọi đến Business Layer
                var newCode = _fixedAssetBL.GetMaxFixedAssetCode();
                // Trả về cho Client
                return StatusCode(StatusCodes.Status201Created, newCode);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ErrorCode = QLTSErrorCode.Exception,
                    DevMsg = "Catched an exception",
                    UserMsg = "Vui lòng liên hệ MISA",
                    TraceID = HttpContext.TraceIdentifier,
                });
            }
        }

        #endregion

        #region POST
        /// <summary>
        /// API Tạo mới tài sản cố định
        /// </summary>
        /// <param name="fixedAsset">Đối tượng tài sản cố định</param>
        /// <returns>ID tài sản cố định được thêm</returns>
        /// Created by: NVThinh (11/11/2022)
        [HttpPost]
        public IActionResult InsertFixedAsset([FromBody] FixedAsset fixedAsset)
        {
            try
            {
                // Gọi đến Business Layer
                var numberOfRowsAffected = _fixedAssetBL.InsertFixedAsset(fixedAsset);

                // Xử lý kết quả trả về từ DB (GridReader)
                if (numberOfRowsAffected > 0)
                    // Thành công
                    return StatusCode(StatusCodes.Status201Created, new
                    {
                        NumberOfRowsAffected = numberOfRowsAffected,
                    });
                else
                    // Thất bại
                    return StatusCode(StatusCodes.Status400BadRequest, new
                    {
                        ErrorCode = QLTSErrorCode.BadRequest,
                        DevMsg = "Bad request",
                        UserMsg = "",
                        TraceID = HttpContext.TraceIdentifier,
                    });
            }
            catch (MySqlException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ErrorCode = QLTSErrorCode.Exception,
                    DevMsg = "catched an MySQLException",
                    moreinfo = ex.Message,
                    TraceID = HttpContext.TraceIdentifier,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ErrorCode = QLTSErrorCode.Exception,
                    DevMsg = "catched an exception",
                    UserMsg = "Thêm mới nhân viên thất bại",
                    MoreInfo = ex.Message,
                    TraceID = HttpContext.TraceIdentifier,
                });
            }
        }

        /// <summary>
        /// Xóa nhiều bản ghi
        /// </summary>
        /// <param name="listFixedAssetID">Danh sách ID các tài sản cần xóa</param>
        /// <returns>Số lượng tài sản được xóa</returns>
        /// Created by: NVThinh (11/11/2022)
        [HttpPost("DeleteBatch")]
        public IActionResult DeleteMultipleFixedAsset([FromBody] ListFixedAssetID fixedAssetIDs)
        {
            try
            {
                // Gọi đến Business Layer
                var isSuccessful = _fixedAssetBL.DeleteMultipleFixedAsset(fixedAssetIDs);

                // Xử lý kết quả trả về từ DB (GridReader)
                if (isSuccessful)
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        FixedAssetIDList = fixedAssetIDs,
                    });
                else
                    return StatusCode(StatusCodes.Status400BadRequest, new
                    {
                        ErrorCode = QLTSErrorCode.BadRequest,
                        DevMsg = "Bad request",
                        UserMsg = "",
                    });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ErrorCode = QLTSErrorCode.Exception,
                    DevMsg = "Catched an exception",
                    UserMsg = "Vui lòng liên hệ MISA",
                    MoreInfo = ex.Message,
                });
            }
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
        [HttpPut("{fixedAssetID}")]
        public IActionResult UpdateFixedAsset([FromRoute] Guid fixedAssetID, [FromBody] FixedAsset fixedAsset)
        {
            try
            {
                // Gọi đến BL
                var numberOfRowsAffected = _fixedAssetBL.UpdateFixedAsset(fixedAssetID, fixedAsset);

                // Xử lý kết quả trả về từ DB (GridReader)
                if (numberOfRowsAffected > 0)
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        FixedAssetID = fixedAssetID,
                    });
                else
                    return StatusCode(StatusCodes.Status400BadRequest, new
                    {
                        ErrorCode = QLTSErrorCode.BadRequest,
                        DevMsg = "Bad request",
                        UserMsg = "",
                    });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ErrorCode = QLTSErrorCode.Exception,
                    DevMsg = "Catched an exception",
                    UserMsg = "Cập nhật thông tin thất bại",
                    MoreInfo = ex.Message,
                    TraceID = HttpContext.TraceIdentifier,
                });
            }
        }

        #endregion

        #region DELETE

        /// <summary>
        /// API Xóa 01 tài sản
        /// </summary>
        /// <param name="fixedAssetID">ID tài sản cần xóa</param>
        /// <returns>ID tài sản được xóa</returns>
        /// Created by: NVThinh (11/11/2022)
        [HttpDelete("{fixedAssetID}")]
        public IActionResult DeleteFixedAsset([FromRoute] Guid fixedAssetID)
        {
            try
            {
                // Gọi đến Business Layer
                var numberOfRowsAffected = _fixedAssetBL.DeleteFixedAsset(fixedAssetID);

                // Xử lý kết quả trả về
                if (numberOfRowsAffected > 0)
                    // Thành công
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        FixedAssetID = fixedAssetID,
                    });
                else
                    return StatusCode(StatusCodes.Status400BadRequest, new
                    {
                        ErrorCode = QLTSErrorCode.BadRequest,
                        DevMsg = "Bad request",
                        UserMsg = "",
                        TraceID = HttpContext.TraceIdentifier,
                    });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ErrorCode = QLTSErrorCode.Exception,
                    DevMsg = "Catched an exception",
                    UserMsg = "Vui lòng liên hệ MISA",
                    MoreInfo = ex.Message,
                });
            }
        }

        #endregion

        #endregion
    }
}
