using Microsoft.AspNetCore.Mvc;
using MISA.QLTS.BL;
using MISA.QLTS.Common.Entitites.DTO;
using MISA.QLTS.Common.Enums;
using MISA.QLTS.Common.Resources;

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

        #region Method

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
                });
            }
        }

        /// <summary>
        /// Lấy thông tin bản ghi theo ID
        /// </summary>
        /// <param name="recordID">ID bản ghi muốn lấy</param>
        /// <returns>Thông tin bản ghi theo ID</returns>
        /// Author: NVThinh (16/11/2022)
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
                return StatusCode(StatusCodes.Status404NotFound, new ErrorResult
                {
                    ErrorCode = QLTSErrorCode.NotFound,
                    DevMsg = Errors.DevMsg_Not_Found,
                    UserMsg = Errors.UserMsg_Not_Found
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
                });
            }

        }

        /// <summary>
        /// API lấy mã record mới
        /// </summary>
        /// <returns>Mã record mới</returns>
        /// Author: NVThinh 9/1/2023
        [HttpGet("newCode")]
        public IActionResult GetNextCode()
        {
            try
            {
                // Gọi đến Business Layer
                var newCode = _baseBL.GetNextCode();
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
                });
            }
        }

        #region POST
        /// <summary>
        /// Xóa nhiều bản ghi
        /// </summary>
        /// <param name="recordIDs">Danh sách ID các bản ghi cần xóa</param>
        /// <returns>Object chứa các thông tin trả về client</returns>
        /// <author>NVThinh 10/1/2023</author>
        [HttpPost("DeleteBatch")]
        public IActionResult DeleteMultipleFixedAsset([FromBody] List<Guid> recordIDs)
        {
            try
            {
                // Gọi đến Business Layer
                var serviceResponse = _baseBL.DeleteMultipleFixedAsset(recordIDs);

                // Xử lý kết quả trả về từ DB (GridReader)
                if (serviceResponse.Success)
                    return StatusCode(StatusCodes.Status200OK, recordIDs);
                else
                {
                    if (serviceResponse.ErrorCode == QLTSErrorCode.BadRequest)
                        return StatusCode(StatusCodes.Status400BadRequest, new ErrorResult
                        {
                            ErrorCode = QLTSErrorCode.BadRequest,
                            DevMsg = Errors.DevMsg_Bad_Request,
                            UserMsg = Errors.UserMsg_Bad_Request,
                            MoreInfo = serviceResponse.Data
                        });
                    else
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult
                        {
                            ErrorCode = serviceResponse.ErrorCode,
                            DevMsg = Errors.DevMsg_Exception,
                            UserMsg = Errors.UserMsg_Exception,
                            MoreInfo = serviceResponse.Data
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult
                {
                    ErrorCode = QLTSErrorCode.Exception,
                    DevMsg = Errors.DevMsg_Exception,
                    UserMsg = Errors.UserMsg_Exception,
                    MoreInfo = new List<string> { ex.Message }
                });
            }
        }
        #endregion

        #region DELETE
        /// <summary>
        /// API Xóa 1 bản ghi theo ID
        /// </summary>
        /// <param name="recordID">ID bản ghi cần xóa</param>
        /// <returns>ID bản ghi được xóa</return
        /// <author>NVThinh 10/1/2023</author>
        [HttpDelete("{recordID}")]
        public IActionResult DeleteFixedAsset([FromRoute] Guid recordID)
        {
            try
            {
                // Gọi đến Business Layer
                var numberOfRowsAffected = _baseBL.DeleteByID(recordID);

                // Xử lý kết quả trả về
                if (numberOfRowsAffected > 0)
                    // Thành công
                    return StatusCode(StatusCodes.Status200OK, recordID);
                else
                    // Thất bại
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
                    MoreInfo = new List<string> { ex.Message }
                });
            }
        }
        #endregion

        #endregion
    }
}
