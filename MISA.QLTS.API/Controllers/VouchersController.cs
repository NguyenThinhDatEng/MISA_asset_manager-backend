using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MISA.QLTS.BL;
using MISA.QLTS.Common.Entitites;
using MISA.QLTS.Common.Entitites.DTO;
using MISA.QLTS.Common.Enums;
using MISA.QLTS.Common.Resources;

namespace MISA.QLTS.API.Controllers
{
    [Authorize]
    public class VouchersController : BasesController<Voucher>
    {
        #region Field

        private IVoucherBL _voucherBL;

        #endregion

        #region Constructor

        public VouchersController(IVoucherBL voucherBL) : base(voucherBL)
        {
            _voucherBL = voucherBL;
        }

        #endregion

        #region Method

        #region GET

        /// <summary>
        /// API lấy chứng từ theo bộ lọc và phân trang
        /// </summary>
        /// <param name="keyword">Từ khóa tìm kiếm</param>
        /// <param name="offset">vị trí của bản ghi bắt đầu lấy</param>
        /// <param name="limit">số bản ghi lấy ra</param>
        /// <returns>Danh sách tài sản cố định và tổng số bản ghi</returns>
        /// <author>NVThinh 11/1/2023</author>
        [HttpGet("filter")]
        public IActionResult GetVouchersByFilterAndPaging(
            [FromQuery] string? keyword,
            [FromQuery] int offset = 0,
            [FromQuery] int limit = 20)
        {
            try
            {
                // Gọi đến Business layer
                var pagingResult = _voucherBL.GetVouchersByFilterAndPaging(keyword, offset, limit);

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
        /// API lấy tất cả tài sản đã đăng ký chứng từ
        /// </summary>
        /// <param name="voucher_id">ID voucher</param>
        /// <returns>Danh sách các tài sản đăng ký voucher đó</returns>
        /// <author>NVThinh 10/1/2023</author>
        [HttpGet("{voucher_id}/detail")]
        public IActionResult GetVoucherDetail([FromRoute] Guid voucher_id) {
            try
            {
                var result = _voucherBL.GetVoucherDetail(voucher_id);
                return StatusCode(StatusCodes.Status200OK, result);
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
        /// Thêm mới chứng từ
        /// </summary>
        /// <param name="voucherResult">Đối tượng chứa chứng từ và mảng các voucher detail</param>
        /// <returns></returns>
        /// <author>NVThinh 12/1/2023</author>
        [HttpPost]
        public IActionResult InsertVoucher(VoucherResult voucherResult)
        {
            try
            {
                var result = _voucherBL.InsertVoucher(voucherResult);
                if (!result.Success)
                {
                    if (result.ErrorCode == QLTSErrorCode.BadRequest)
                        return StatusCode(StatusCodes.Status400BadRequest, new ErrorResult
                        {
                            ErrorCode = result.ErrorCode,
                            DevMsg = Errors.DevMsg_Bad_Request,
                            UserMsg = Errors.UserMsg_Bad_Request,
                            MoreInfo = result.Data
                        });
                    else
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult
                        {
                            ErrorCode = result.ErrorCode,
                            DevMsg = Errors.DevMsg_Exception,
                            UserMsg = Errors.UserMsg_Exception,
                            MoreInfo = result.Data
                        });
                    }
                }
                return StatusCode(StatusCodes.Status200OK, result);
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

        #region PUT
        /// <summary>
        /// Thêm mới chứng từ
        /// </summary>
        /// <param name="voucherResult">Đối tượng chứa chứng từ và mảng các voucher detail</param>
        /// <returns></returns>
        /// <author>NVThinh 12/1/2023</author>
        [HttpPut]
        public IActionResult UpdateVoucher(VoucherResult voucherResult)
        {
            try
            {
                var result = _voucherBL.UpdateVoucher(voucherResult);
                if (!result.Success)
                {
                    if (result.ErrorCode == QLTSErrorCode.BadRequest)
                        return StatusCode(StatusCodes.Status400BadRequest, new ErrorResult
                        {
                            ErrorCode = result.ErrorCode,
                            DevMsg = Errors.DevMsg_Bad_Request,
                            UserMsg = Errors.UserMsg_Bad_Request,
                            MoreInfo = result.Data
                        });
                    else
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult
                        {
                            ErrorCode = result.ErrorCode,
                            DevMsg = Errors.DevMsg_Exception,
                            UserMsg = Errors.UserMsg_Exception,
                            MoreInfo = result.Data
                        });
                    }
                }
                return StatusCode(StatusCodes.Status200OK, result);
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

        #endregion
    }
}
