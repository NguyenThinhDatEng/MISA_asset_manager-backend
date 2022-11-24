using Dapper;
using MISA.QLTS.Common.Constants;
using MISA.QLTS.Common.Entitites;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.DL
{
    public class BaseDL<T> : IBaseDL<T>
    {
        #region Method

        /// <summary>
        /// Lấy thông tin toàn bộ bản ghi
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        /// Create by: NVThinh (16/11/2022)
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
        /// Create by: NVThinh (16/11/2022)
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
        /// Kiểm tra trùng mã
        /// </summary>
        /// <param name="recordCode">Mã bản ghi</param>
        /// <param name="recordID">ID bản ghi</param>
        /// <returns>Boolean</returns>
        /// Created by: NVThinh (21/11/2022)
        public bool CheckDuplicateCode(string recordCode, Guid recordID)
        {
            // Chuẩn bị câu lệnh MySQL
            string procedureName = String.Format(Procedure.GET_BY_CODE, typeof(T).Name);

            // Chuẩn bị tham số
            var parameters = new DynamicParameters();
            parameters.Add($"v_{typeof(T).Name}Code", recordCode);

            // Thực hiện gọi vào DB
            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                var res = mySqlConnection.QueryFirstOrDefault<T>(procedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
                // Xử lí kết quả trả về
                if (res == null)
                {
                    return false;
                }
                else
                {
                    // so sánh xem ID truyền vào có trùng với ID lấy lên từ Database không
                    var property = typeof(T).GetProperty("fixed_asset_id");
                    if ((Guid)property.GetValue(res) == recordID)
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
