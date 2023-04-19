using Microsoft.AspNetCore.Mvc;
using MISA.QLTS.BL;
using MISA.QLTS.Common.Enums;
using MISA.QLTS.Common.Entitites;
using MISA.QLTS.Common.Entitites.DTO;
using MISA.QLTS.API.Controllers;
using MISA.QLTS.Common.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace MISA.QLTS.COMMON.Controllers
{
    [Authorize]
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
        /// <param name="departmentID">Mảng ID bộ phận sử dụng</param>
        /// <param name="fixedAssetCategoryID">Mảng ID mã bộ phận sử dụng</param>
        /// <param name="offset">vị trí của bản ghi bắt đầu lấy</param>
        /// <param name="limit">số bản ghi lấy ra</param>
        /// <param name="isIncrement">true nếu danh sách là ghi tăng chứng từ</param>
        /// <param name="selectedIDs">mảng chứa các ID đã được lựa chọn để ghi tăng chứng từ</param>
        /// <returns>Danh sách tài sản cố định và tổng số bản ghi</returns>
        /// <author>NVThinh 11/1/2023</author>
        [HttpPost("filter")]
        public IActionResult GetFixedAssetByFilterAndPaging(
            [FromQuery] string? keyword,
            [FromQuery] Guid departmentID,
            [FromQuery] Guid fixedAssetCategoryID,
            [FromQuery] int offset = 0,
            [FromQuery] int limit = 20,
            [FromQuery] bool isIncrement = false,
            [FromBody] List<Guid>? selectedIDs = null)
        {
            try
            {
                // Gọi đến Business layer
                var pagingResult = _fixedAssetBL.GetFixedAssetByFilterAndPaging(keyword, departmentID, fixedAssetCategoryID, offset, limit, isIncrement, selectedIDs);

                // Nếu thành công
                if (pagingResult.TotalOfRecords > -1)
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
                    MoreInfo = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// API lấy mã tài sản cố định mới
        /// </summary>
        /// <returns>Mã tài sản cố định mới</returns>
        /// Author: NVThinh 16/11/2022
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
                });
            }
        }

        /// <summary>
        /// Kiểm tra các tài sản đã có chứng từ chưa
        /// </summary>
        /// <param name="fixedAssetIDs">Danh sách ID tài sản</param>
        /// <returns>Mã chứng từ</returns>
        /// <author>NVThinh 16/1/2023</author>
        [HttpPost("checkVoucher")]
        public IActionResult CheckExistedVoucher([FromBody] List<Guid> fixedAssetIDs)
        {
            try
            {
                // Gọi đến Business Layer
                var voucherCode = _fixedAssetBL.CheckExistedVoucher(fixedAssetIDs);
                // Trả về cho Client
                return StatusCode(StatusCodes.Status200OK, voucherCode);
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

        #endregion

        #region POST
        /// <summary>
        /// API Tạo mới tài sản cố định
        /// </summary>
        /// <param name="fixedAsset">Đối tượng tài sản cố định</param>
        /// <returns>ID tài sản cố định được thêm</returns>
        /// Author: NVThinh (11/11/2022)
        [HttpPost]
        public IActionResult InsertFixedAsset([FromBody] FixedAsset fixedAsset)
        {
            try
            {
                // Gọi đến Business Layer
                var serviceResponse = _fixedAssetBL.InsertFixedAsset(fixedAsset);

                // Có lỗi khi validate dữ liệu
                if (!serviceResponse.Success)
                {
                    if (serviceResponse.ErrorCode == QLTSErrorCode.BadRequest)
                    return StatusCode(StatusCodes.Status400BadRequest, new ErrorResult
                    {
                        ErrorCode = serviceResponse.ErrorCode,
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

                // Thành công
                return StatusCode(StatusCodes.Status201Created);
            }
            catch(ArgumentException ex)
            {
                // Exception thiếu tham số đầu vào
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult
                {
                    ErrorCode = QLTSErrorCode.Exception,
                    DevMsg = Errors.DevMsg_Exception,
                    UserMsg = Errors.UserMsg_Exception,
                    MoreInfo = new List<string> { ex.Message}
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult
                {
                    ErrorCode = QLTSErrorCode.Exception,
                    DevMsg = Errors.DevMsg_Exception,
                    UserMsg = Errors.UserMsg_Fail,
                    MoreInfo = new List<string> { ex.Message },
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
        /// Author: NVThinh (11/11/2022)
        [HttpPut("{fixedAssetID}")]
        public IActionResult UpdateFixedAsset([FromRoute] Guid fixedAssetID, [FromBody] FixedAsset fixedAsset)
        {
            try
            {
                // Gọi đến Business Layer
                var serviceResponse = _fixedAssetBL.UpdateFixedAsset(fixedAssetID, fixedAsset);

                // Có lỗi khi validate dữ liệu
                if (!serviceResponse.Success)
                {
                    if (serviceResponse.ErrorCode == QLTSErrorCode.BadRequest)
                        return StatusCode(StatusCodes.Status400BadRequest, new ErrorResult
                        {
                            ErrorCode = serviceResponse.ErrorCode,
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

                // Thành công
                return StatusCode(StatusCodes.Status200OK);
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

        #endregion
    }
}
