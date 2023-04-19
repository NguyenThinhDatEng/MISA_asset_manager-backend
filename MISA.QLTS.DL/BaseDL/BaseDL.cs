using Dapper;
using MISA.QLTS.Common.Constants;
using MISA.QLTS.Common.Entitites.DTO;
using MISA.QLTS.Common.Enums;
using MISA.QLTS.Common.Resources;
using MySqlConnector;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace MISA.QLTS.DL
{
    public class BaseDL<T> : IBaseDL<T>
    {
        #region Method

        #region GET

        /// <summary>
        /// Lấy thông tin toàn bộ bản ghi
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        /// Author: NVThinh (16/11/2022)
        public IEnumerable<T> GetAllRecords()
        {
            // Chuẩn bị tên Stored procedure
            string procedureName = String.Format(Procedure.GET_ALL, typeof(T).Name);

            // Khởi tạo biến kết quả trả về
            var records = new List<T>();

            //Khởi tạo kết nối DB MySQL
            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                // Thực hiện gọi vào DB để chạy stored procedure với tham số đầu vào ở trên
                records = (List<T>)mySqlConnection.Query<T>(procedureName, commandType: System.Data.CommandType.StoredProcedure);
            }

            return records;
        }

        /// <summary>
        /// Lấy thông tin bản ghi theo ID
        /// </summary>
        /// <param name="recordID">ID bản ghi muốn lấy</param>
        /// <returns>Thông tin bản ghi theo ID</returns>
        /// Author: NVThinh (16/11/2022)
        public T GetByID(Guid recordID)
        {
            // Chuẩn bị câu lệnh SQL
            string procedureName = String.Format(Procedure.GET_BY_ID, typeof(T).Name);

            //Chuẩn bị tham số đầu vào
            var parameters = new DynamicParameters();
            parameters.Add($"v_{typeof(T).Name}ID", recordID);

            //Khởi tạo kết nối DB MySQL
            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                //Thực hiện gọi vào DB
                var record = mySqlConnection.QueryFirstOrDefault<T>(procedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                //Xử lý kết quả trả về
                return record;
            }
        }

        /// <summary>
        /// API lấy mã record mới
        /// </summary>
        /// <returns>Mã record mới</returns>
        /// Author: NVThinh 9/1/2023
        public string GetNextCode()
        {

            // Chuẩn bị câu lệnh SQL
            string procedureName = String.Format(Procedure.GET_LAST_CODE, typeof(T).Name);

            //Khởi tạo kết nối DB MySQL
            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                //Thực hiện gọi vào DB
                var lastCode = mySqlConnection.QueryFirstOrDefault<string>(procedureName, commandType: System.Data.CommandType.StoredProcedure);

                //Xử lý kết quả trả về
                string lastNum = Regex.Match(lastCode, @"(\d+)(?!.*\d)").Value;
                int lenOfLastNum = lastNum.Length; // chiều dài mã số

                // Mã không có bất kỳ ký tự số nào
                if (lastNum == "") return lastCode + '1';

                int numVal = Int32.Parse(lastNum); // Ép sang kiểu số
                int lastIndex = lastCode.LastIndexOf(lastNum); // Chỉ số xuất hiện "lastNum" tính từ cuối lên

                // Tạo mã số mới
                string newNumCode = (numVal + 1).ToString(); // mã số mới = mã số cũ + 1
                int lenOfNewNumCode = newNumCode.Length; // Lấy chiều dài mã số mới
                // Tạo mã số mới hoàn chỉnh
                if (lenOfNewNumCode < lenOfLastNum) // Ex: old code: 00023, new code: 24 => new code: '000' + '24'
                    newNumCode = lastCode.Substring(lastIndex, lenOfLastNum - lenOfNewNumCode) + newNumCode;

                // xử lý kết quả đầu ra
                // Xóa mã số cũ
                lastCode = lastCode.Remove(lastIndex, lenOfLastNum);
                // Insert mã số mới
                lastCode = lastCode.Insert(lastIndex, newNumCode);

                // Trả về
                return lastCode;
            }
        }

        #endregion

        #region POST

        /// <summary>
        /// Xóa nhiều bản ghi
        /// </summary>
        /// <param name="recordIDs">Danh sách ID các bản ghi cần xóa</param>
        /// <returns>Object chứa các thông tin trả về client</returns>
        /// <author>NVThinh 10/1/2023</author>
        public ServiceResponse DeleteMultipleFixedAsset(List<Guid> recordIDs)
        {
            // Thiết lập tên procedure
            string procedureName = String.Format(Procedure.DELETE_MULTIPLE, typeof(T).Name);

            // Chuyển đổi list sang json
            var IDs = JsonSerializer.Serialize(recordIDs);

            // Chuẩn bị tham số đầu vào
            var parameters = new DynamicParameters();
            parameters.Add("IDs", IDs);

            //Khởi tạo kết nối DB MySQL
            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                mySqlConnection.Open();

                // Khởi tạo transaction
                var transaction = mySqlConnection.BeginTransaction();

                try
                {
                    // Thực hiện gọi vào DB
                    var numberOfAffectedRows = mySqlConnection.Execute(procedureName, parameters, commandType: System.Data.CommandType.StoredProcedure, transaction: transaction);

                    if (numberOfAffectedRows == recordIDs.Count)

                    {
                        // Cam kết thực hiện thành công
                        transaction.Commit();
                    }
                    else
                    {
                        // Thực hiện thất bại, khôi phục dữ liệu
                        transaction.Rollback();
                        return new ServiceResponse { Success = false, ErrorCode = QLTSErrorCode.BadRequest, Data = new List<string> { Errors.UserMsg_Fail_Delelte_Batch } };
                    }

                    return new ServiceResponse { Success = true };
                }
                catch (Exception ex)
                {
                    return new ServiceResponse { Success = false, Data = new List<string> { ex.Message }, ErrorCode = QLTSErrorCode.Exception };
                }
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
        public int DeleteByID(Guid recordID)
        {
            // Chuẩn bị tên Stored procedure
            string procedureName = String.Format(Procedure.DELETE_BY_ID, typeof(T).Name);

            // Chuẩn bị tham số đầu vào cho stored procedure
            var parameters = new DynamicParameters();
            parameters.Add("@ID", recordID);

            //Khởi tạo kết nối DB MySQL
            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                // Thực hiện gọi vào DB để chạy stored procedure với tham số đầu vào ở trên
                var numberOfRowsAffected = mySqlConnection.Execute(procedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                return numberOfRowsAffected;
            }
        }

        #endregion 

        /// <summary>
        /// Kiểm tra trùng mã
        /// </summary>
        /// <param name="recordCode">Mã bản ghi</param>
        /// <param name="recordID">ID bản ghi</param>
        /// <param name="idType">tên id của bảng</param>
        /// <returns>Boolean</returns>
        /// Author: NVThinh (21/11/2022)
        public bool CheckDuplicateCode(string recordCode, Guid recordID, string idType)
        {
            // Chuẩn bị câu lệnh MySQL
            string procedureName = String.Format(Procedure.GET_BY_CODE, typeof(T).Name);

            // Chuẩn bị tham số
            var parameters = new DynamicParameters();
            parameters.Add($"v_{typeof(T).Name}Code", recordCode);

            // Thực hiện gọi vào DB
            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                // Tìm mã bản ghi trong DB
                var code = mySqlConnection.QueryFirstOrDefault<T>(procedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
                // Xử lí kết quả trả về
                // Nếu mã chưa tồn tại 
                if (code == null)
                {
                    return false;
                }
                else
                {
                    // Nếu mã đã tồn tại so sánh xem ID truyền vào có trùng với ID lấy lên từ Database không
                    var property = typeof(T).GetProperty(idType);
                    if ((Guid)property.GetValue(code) == recordID)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        #endregion
    }
}
