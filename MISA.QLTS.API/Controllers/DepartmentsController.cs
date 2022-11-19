using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.QLTS.BL;
using MISA.QLTS.Common.Enums;
using MISA.QLTS.DL;
using MySqlConnector;

namespace MISA.QLTS.COMMON.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        #region Field
        private IDepartmentBL _departmentBL;
        #endregion

        #region Constructor
        public DepartmentsController(IDepartmentBL departmentBL)
        {
            _departmentBL = departmentBL;
        }
        #endregion

        #region Method
        /// <summary>
        /// Lấy tất cả bản ghi bộ phận sử dụng
        /// </summary>
        /// <returns>Thong tin cua tat ca bo phan su dung</returns>
        /// Author: Nguyen Van Thinh 11/11/2022
        [HttpGet]
        public IActionResult GetAllDepartment()
        {
            try
            {
                // Gán dữ liệu trả về
                var departments = _departmentBL.GetAllDepartment();
                // Thành công
                if (departments != null)
                    return StatusCode(StatusCodes.Status200OK, departments);
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
        #endregion
    }
}
