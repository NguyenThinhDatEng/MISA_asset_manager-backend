using Dapper;
using Microsoft.AspNetCore.Mvc;
using MISA.QLTS.BL;
using MISA.QLTS.Common.Enums;
using MISA.QLTS.Common.Entitites;
using MISA.QLTS.Common.Entitites.DTO;
using MySqlConnector;

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

        #region Property
        // Khởi tạo kết nối tới DB MySQL
        public string connectionString = "Server=localhost;Port=3306;Database=misa.web09.hcsn.nvthinh;Uid=root;Pwd=12345678;";
        #endregion

        #region Method

        #region Create
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
                //Khởi tạo kết nối DB MySQL
                var mySqlConnection = new MySqlConnection(connectionString);

                // Chuẩn bị tên Stored procedure
                string procedureName = "Proc_CreateAsset";

                // Chuẩn bị tham số đầu vào cho stored procedure
                var parameters = new DynamicParameters();
                var newID = Guid.NewGuid();
                Console.WriteLine(newID);
                parameters.Add("@fixed_asset_id", newID);
                parameters.Add("@fixed_asset_code", fixedAsset.fixed_asset_code);
                parameters.Add("@fixed_asset_name", fixedAsset.fixed_asset_name);
                parameters.Add("@department_id", fixedAsset.department_id);
                parameters.Add("@department_code", fixedAsset.department_code);
                parameters.Add("@department_name", fixedAsset.department_name);
                parameters.Add("@fixed_asset_category_id", fixedAsset.fixed_asset_category_id);
                parameters.Add("@fixed_asset_category_code", fixedAsset.fixed_asset_category_code);
                parameters.Add("@fixed_asset_category_name", fixedAsset.fixed_asset_category_name);
                parameters.Add("@purchase_date", fixedAsset.purchase_date);
                parameters.Add("@cost", fixedAsset.cost);
                parameters.Add("@quantity", fixedAsset.quantity);
                parameters.Add("@depreciation_rate", fixedAsset.depreciation_rate);
                parameters.Add("@tracked_year", fixedAsset.tracked_year);
                parameters.Add("@life_time", fixedAsset.life_time);
                parameters.Add("@production_date", fixedAsset.production_date);
                parameters.Add("@created_by", "Nguyen Van Thinh");
                parameters.Add("@created_date", DateTime.Now);
                parameters.Add("@modified_by", "Nguyen Van Thinh");
                parameters.Add("@modified_date", DateTime.Now);

                // Thực hiện gọi vào DB để chạy stored procedure với tham số đầu vào ở trên
                var numberOfRowsAffected = mySqlConnection.Execute(procedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                // Xử lý kết quả trả về từ DB (GridReader)
                if (numberOfRowsAffected > 0)
                    return StatusCode(StatusCodes.Status201Created, new
                    {
                        numberOfRowsAffected = numberOfRowsAffected,
                    });
                else
                    return StatusCode(StatusCodes.Status400BadRequest, new
                    {
                        errorcode = 2,
                        devmsg = "Bad request",
                        traceId = HttpContext.TraceIdentifier,
                    });
            }
            catch (MySqlException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    errorcode = ex.ErrorCode,
                    devmsg = "catched an MySQLException",
                    moreinfo = ex.Message,
                    traceId = HttpContext.TraceIdentifier,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    errorcode = 1,
                    devmsg = "catched an exception",
                    user = "Thêm mới nhân viên thất bại",
                    moreinfo = ex.Message,
                    traceId = HttpContext.TraceIdentifier,
                });
            }
        }
        #endregion

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
                    DevMsg = "",
                    UserMsg = "",
                    traceId = HttpContext.TraceIdentifier,
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
                var fixedAsset = _fixedAssetBL.GetFixedAssetByID(fixedAssetID);
                if (fixedAsset != null)
                    return StatusCode(StatusCodes.Status200OK, fixedAsset);
                return StatusCode(StatusCodes.Status404NotFound, new
                {
                    errorcode = QLTSErrorCode.NotFound,
                    devmsg = "Not Found",
                    user = "Không tìm thấy bản ghi"
                });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ErrorCode = QLTSErrorCode.Exception,
                    DevMsg = "Catched an exception",
                    User = "Vui lòng liên hệ MISA",
                    traceId = HttpContext.TraceIdentifier,
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
                var mySqlConnection = new MySqlConnection(connectionString);

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
                    User = "Vui lòng liên hệ MISA",
                    MoreInfo = "https://openapi.misa.com.vn/errorcode/e001",
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
                //Khởi tạo kết nối DB MySQL
                var mySqlConnection = new MySqlConnection(connectionString);

                // Chuẩn bị câu lệnh SQL
                string storedProcedureName = "Proc_GetMaxFixedAssetCode";

                //Thực hiện gọi vào DB
                var maxCode = mySqlConnection.QueryFirstOrDefault<string>(storedProcedureName, commandType: System.Data.CommandType.StoredProcedure);

                //Xử lý kết quả trả về
                // Lấy chiều dài phần số của mã tài sản
                int length = maxCode.Length - 2;
                // Lấy phần số của mã tài sản
                int newID = Int32.Parse(maxCode.Substring(2)) + 1;
                // Lấy chiều dài phần số của mã mới
                int newLength = newID.ToString().Length;
                // Chuyển đổi phần số sang chuỗi
                string newCode = newID.ToString();
                // Thêm kí tự '0' đằng trước phần số mới nếu chiều dài chuỗi mới nhỏ hơn chuỗi cũ
                int numberOfZero = length - newLength;
                for (int i = 0; i < numberOfZero; i++)
                {
                    newCode = '0' + newCode;
                }
                // Thêm tiền tố vào mã mới
                newCode = newCode.Insert(0, "TS");

                return StatusCode(StatusCodes.Status201Created, newCode);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ErrorCode = 1,
                    DevMsg = "Catched an exception",
                    User = "Vui lòng liên hệ MISA",
                    MoreInfo = "https://openapi.misa.com.vn/errorcode/e001",
                });
            }
        }
        #endregion

        #region Update
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
                //Khởi tạo kết nối DB MySQL
                var mySqlConnection = new MySqlConnection(connectionString);

                // Chuẩn bị tên Stored procedure
                string procedureName = "Proc_UpdateAsset";

                // Chuẩn bị tham số đầu vào cho stored procedure
                var parameters = new DynamicParameters();
                parameters.Add("@fixed_asset_id", fixedAssetID);
                parameters.Add("@fixed_asset_code", fixedAsset.fixed_asset_code);
                parameters.Add("@fixed_asset_name", fixedAsset.fixed_asset_name);
                parameters.Add("@department_id", fixedAsset.department_id);
                parameters.Add("@department_code", fixedAsset.department_code);
                parameters.Add("@department_name", fixedAsset.department_name);
                parameters.Add("@fixed_asset_category_id", fixedAsset.fixed_asset_category_id);
                parameters.Add("@fixed_asset_category_code", fixedAsset.fixed_asset_category_code);
                parameters.Add("@fixed_asset_category_name", fixedAsset.fixed_asset_category_name);
                parameters.Add("@purchase_date", fixedAsset.purchase_date);
                parameters.Add("@cost", fixedAsset.cost);
                parameters.Add("@quantity", fixedAsset.quantity);
                parameters.Add("@depreciation_rate", fixedAsset.depreciation_rate);
                parameters.Add("@tracked_year", fixedAsset.tracked_year);
                parameters.Add("@life_time", fixedAsset.life_time);
                parameters.Add("@production_date", fixedAsset.production_date);
                parameters.Add("@created_by", null);
                parameters.Add("@created_date", DateTime.Now);
                parameters.Add("@modified_by", null);
                parameters.Add("@modified_date", DateTime.Now);

                // Thực hiện gọi vào DB để chạy stored procedure với tham số đầu vào ở trên
                var numberOfRowsAffected = mySqlConnection.Execute(procedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                // Xử lý kết quả trả về từ DB (GridReader)
                if (numberOfRowsAffected > 0)
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        Fixed_Asset_ID = fixedAssetID,
                    });
                else
                    return StatusCode(StatusCodes.Status400BadRequest, new
                    {
                        errorcode = 2,
                        devmsg = "Bad request",
                        traceId = HttpContext.TraceIdentifier,
                    });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ErrorCode = 1,
                    DevMsg = "Catched an exception",
                    User = "Vui lòng liên hệ MISA",
                    MoreInfo = "https://openapi.misa.com.vn/errorcode/e001",
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
                //Khởi tạo kết nối DB MySQL
                var mySqlConnection = new MySqlConnection(connectionString);

                // Chuẩn bị tên Stored procedure
                string procedureName = "Proc_DeleteAsset";

                // Chuẩn bị tham số đầu vào cho stored procedure
                var parameters = new DynamicParameters();
                parameters.Add("@fixedAssetID", fixedAssetID);

                // Thực hiện gọi vào DB để chạy stored procedure với tham số đầu vào ở trên
                var numberOfRowsAffected = mySqlConnection.Execute(procedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                // Xử lý kết quả trả về từ DB (GridReader)
                if (numberOfRowsAffected > 0)
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        numberOfRowsAffected = numberOfRowsAffected,
                    });
                else
                    return StatusCode(StatusCodes.Status400BadRequest, new
                    {
                        errorcode = 2,
                        devmsg = "Bad request",
                        traceId = HttpContext.TraceIdentifier,
                    });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ErrorCode = 1,
                    DevMsg = "Catched an exception",
                    User = "Vui lòng liên hệ MISA",
                    MoreInfo = "https://openapi.misa.com.vn/errorcode/e001",
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
                //Khởi tạo kết nối DB MySQL
                var mySqlConnection = new MySqlConnection(connectionString);

                // Chuẩn bị câu lệnh SQL
                string procedureName = "Proc_DeleteMultipleFixedAsset";

                //Chuẩn bị tham số đầu vào
                var parameters = new DynamicParameters();
                string IDs = "";
                var list = fixedAssetIDs.FixedAssetIDs;
                for (int i = 0; i < list.Count; i++)
                    IDs += "\'" + list[i] + "\',";
                IDs = IDs.Remove(IDs.Length - 1);
                Console.WriteLine(IDs);
                parameters.Add("@IDs", IDs);
                //Thực hiện gọi vào DB
                var numberOfRowsAffected = mySqlConnection.Execute(procedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                // Xử lý kết quả trả về từ DB (GridReader)
                if (numberOfRowsAffected > 0)
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        numberOfRowsAffected = numberOfRowsAffected,
                    });
                else
                    return StatusCode(StatusCodes.Status400BadRequest, new
                    {
                        errorcode = 2,
                        devmsg = "Bad request",
                        traceId = HttpContext.TraceIdentifier,
                    });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ErrorCode = 1,
                    DevMsg = "Catched an exception",
                    User = "Vui lòng liên hệ MISA",
                    MoreInfo = "https://openapi.misa.com.vn/errorcode/e001",
                });
            }
        }
        #endregion

        #endregion
    }
}
