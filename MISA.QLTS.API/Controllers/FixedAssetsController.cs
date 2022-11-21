using Dapper;
using Microsoft.AspNetCore.Mvc;
using MISA.QLTS.BL;
using MISA.QLTS.Common.Enums;
using MISA.QLTS.Common.Entitites;
using MISA.QLTS.Common.Entitites.DTO;
using MySqlConnector;
using MISA.QLTS.DL;
using MISA.QLTS.API.Controllers;
using MISA.QLTS.Common;
using System.Reflection;

namespace MISA.QLTS.COMMON.Controllers
{
    [ApiController]
    public class FixedAssetsController : BasesController<FixedAsset>
    {
        #region Field

        private IFixedAssetBL _fixedAssetBL;

        #endregion

        #region Constructor

        public FixedAssetsController(IFixedAssetBL fixedAssetBL) : base(fixedAssetBL)
        {
            _fixedAssetBL = fixedAssetBL;
        }

        #endregion

        #region Method

        #region GET

        /// <summary>
        /// API lấy tài sản theo bộ lọc và phân trang
        /// </summary>
        /// <param name="keyword">Từ khóa tìm kiếm</param>
        /// <param name="departmentID">ID bộ phận sử dụng</param>
        /// <param name="fixedAssetCategoryID">ID mã bộ phận sử dụng</param>
        /// <param name="offset">vị trí của bản ghi bắt đầu lấy</param>
        /// <param name="limit">số bản ghi lấy ra</param>
        /// <returns>Danh sách tài sản cố định và tổng số bản ghi</returns>
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
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult
                {
                    ErrorCode = QLTSErrorCode.Exception,
                    DevMsg = Resources.DevMsg_Exception,
                    UserMsg = Resources.UserMsg_Exception,
                    MoreInfo = ex.Message,
                    TraceID = HttpContext.TraceIdentifier
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
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult
                {
                    ErrorCode = QLTSErrorCode.Exception,
                    DevMsg = Resources.DevMsg_Exception,
                    UserMsg = Resources.UserMsg_Exception,
                    MoreInfo = ex.Message,
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
                {
                    if (numberOfRowsAffected == -1)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ErrorResult
                        {
                            ErrorCode = QLTSErrorCode.DuplicateKey,
                            DevMsg = Resources.DevMsg_Exception,
                            UserMsg = Resources.UserMsg_Duplicate_Key,
                        });
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ErrorResult
                        {
                            ErrorCode = QLTSErrorCode.BadRequest,
                            DevMsg = Resources.DevMsg_Bad_Request,
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult
                {
                    ErrorCode = QLTSErrorCode.Exception,
                    DevMsg = Resources.DevMsg_Exception,
                    UserMsg = Resources.UserMsg_Fail,
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
                    return StatusCode(StatusCodes.Status400BadRequest, new ErrorResult
                    {
                        ErrorCode = QLTSErrorCode.BadRequest,
                        DevMsg = Resources.DevMsg_Bad_Request,
                        UserMsg = Resources.UserMsg_Bad_Request,
                    });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult
                {
                    ErrorCode = QLTSErrorCode.Exception,
                    DevMsg = Resources.DevMsg_Exception,
                    UserMsg = Resources.UserMsg_Exception,
                    MoreInfo = ex.Message,
                    TraceID = HttpContext.TraceIdentifier
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
                {
                    if (numberOfRowsAffected == -1)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ErrorResult
                        {
                            ErrorCode = QLTSErrorCode.DuplicateKey,
                            DevMsg = Resources.DevMsg_Exception,
                            UserMsg = Resources.UserMsg_Duplicate_Key
                        });
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ErrorResult
                        {
                            ErrorCode = QLTSErrorCode.BadRequest,
                            DevMsg = Resources.DevMsg_Exception,
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ErrorCode = QLTSErrorCode.Exception,
                    DevMsg = Resources.DevMsg_Exception,
                    UserMsg = Resources.UserMsg_Fail,
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
                    return StatusCode(StatusCodes.Status400BadRequest, new ErrorResult
                    {
                        ErrorCode = QLTSErrorCode.BadRequest,
                        DevMsg = Resources.DevMsg_Bad_Request,
                        UserMsg = Resources.UserMsg_Fail,
                    });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult
                {
                    ErrorCode = QLTSErrorCode.Exception,
                    DevMsg = Resources.DevMsg_Exception,
                    UserMsg = Resources.UserMsg_Exception,
                    MoreInfo = ex.Message,
                    TraceID = HttpContext.TraceIdentifier
                });
            }
        }

        #endregion

        #endregion
    }
}
