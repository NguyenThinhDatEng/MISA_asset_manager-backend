using Microsoft.AspNetCore.Mvc;
using MISA.QLTS.BL;
using MISA.QLTS.Common.Enums;
using MISA.QLTS.Common.Entitites;
using MISA.QLTS.Common.Entitites.DTO;
using MISA.QLTS.API.Controllers;
using MISA.QLTS.Common.Resources;

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
            [FromQuery] Guid departmentID,
            [FromQuery] Guid fixedAssetCategoryID,
            [FromQuery] int offset = 0,
            [FromQuery] int limit = 20)
        {
            try
            {
                // Gọi đến Business layer
                var pagingResult = _fixedAssetBL.GetFixedAssetByFilterAndPaging(keyword, departmentID, fixedAssetCategoryID, offset, limit);

                // Nếu thành công
                if (pagingResult != null)
                    return StatusCode(StatusCodes.Status200OK, pagingResult);
                // Thất bại
                return StatusCode(StatusCodes.Status404NotFound, new ErrorResult
                {
                    ErrorCode = QLTSErrorCode.NotFound,
                    DevMsg = Errors.DevMsg_Not_Found,
                    UserMsg = Errors.UserMsg_Not_Found,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult
                {
                    ErrorCode = QLTSErrorCode.Exception,
                    DevMsg = Errors.DevMsg_Exception,
                    UserMsg = Errors.UserMsg_Exception,
                    MoreInfo = new List<string> { ex.Message },
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
                    DevMsg = Errors.DevMsg_Exception,
                    UserMsg = Errors.UserMsg_Exception,
                    MoreInfo = new List<string> { ex.Message },
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
                var serviceResponse = _fixedAssetBL.InsertFixedAsset(fixedAsset);

                // Validate dữ liệu đầu vào
                if (!serviceResponse.Success)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ErrorResult
                    {
                        ErrorCode = QLTSErrorCode.BadRequest,
                        DevMsg = Errors.DevMsg_Bad_Request,
                        UserMsg = Errors.UserMsg_Bad_Request,
                        MoreInfo = serviceResponse.Data
                    });
                }

                // Thành công
                return StatusCode(StatusCodes.Status201Created, fixedAsset);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult
                {
                    ErrorCode = QLTSErrorCode.Exception,
                    DevMsg = Errors.DevMsg_Exception,
                    UserMsg = Errors.UserMsg_Fail,
                    MoreInfo = new List<string> { ex.Message },
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
                var serviceResponse = _fixedAssetBL.DeleteMultipleFixedAsset(fixedAssetIDs);

                // Xử lý kết quả trả về từ DB (GridReader)
                if (serviceResponse.Success)
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        FixedAssetIDList = fixedAssetIDs,
                    });
                else
                    return StatusCode(StatusCodes.Status400BadRequest, new ErrorResult
                    {
                        ErrorCode = QLTSErrorCode.BadRequest,
                        DevMsg = Errors.DevMsg_Bad_Request,
                        UserMsg = Errors.UserMsg_Bad_Request,
                        MoreInfo = serviceResponse.Data
                    });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult
                {
                    ErrorCode = QLTSErrorCode.Exception,
                    DevMsg = Errors.DevMsg_Exception,
                    UserMsg = Errors.UserMsg_Exception,
                    MoreInfo = new List<string> { ex.Message },
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
                // Validate dữ liệu đầu vào
                var validateResult = _fixedAssetBL.ValidateRequestData(fixedAsset);
                if (!validateResult.Success)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ErrorResult
                    {
                        ErrorCode = QLTSErrorCode.BadRequest,
                        DevMsg = Errors.DevMsg_Bad_Request,
                        UserMsg = Errors.UserMsg_Bad_Request,
                        MoreInfo = validateResult.Data
                    });
                }

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
                            DevMsg = Errors.DevMsg_Exception,
                            UserMsg = Errors.UserMsg_Duplicate_Key
                        });
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ErrorResult
                        {
                            ErrorCode = QLTSErrorCode.BadRequest,
                            DevMsg = Errors.DevMsg_Exception,
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ErrorCode = QLTSErrorCode.Exception,
                    DevMsg = Errors.DevMsg_Exception,
                    UserMsg = Errors.UserMsg_Fail,
                    MoreInfo = new List<string> { ex.Message },
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
                        DevMsg = Errors.DevMsg_Bad_Request,
                        UserMsg = Errors.UserMsg_Fail,
                    });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult
                {
                    ErrorCode = QLTSErrorCode.Exception,
                    DevMsg = Errors.DevMsg_Exception,
                    UserMsg = Errors.UserMsg_Exception,
                    MoreInfo = new List<string> { ex.Message },
                    TraceID = HttpContext.TraceIdentifier
                });
            }
        }

        #endregion

        #endregion
    }
}
