using Dapper;
using MISA.QLTS.Common.Entitites;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.DL
{
    public class DepartmentDL : IDepartmentDL
    {
        /// <summary>
        /// Lấy thông tin tất cả bộ phận sử dụng
        /// </summary>
        /// <returns>Danh sách bộ phận sử dụng</returns>
        /// Create by: NVThinh (16/11/2022)
        public IEnumerable<dynamic> GetAllDepartment()
        {
            //Khởi tạo kết nối DB MySQL
            var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString);

            // Chuẩn bị tên Stored procedure
            string procedureName = "Proc_GetAllDepartment";

            // Thực hiện gọi vào DB để chạy stored procedure với tham số đầu vào ở trên
            var departments = mySqlConnection.Query(procedureName, commandType: System.Data.CommandType.StoredProcedure);

            if (departments != null)
            // Xử lý kết quả trả về từ Database
                return departments;
            return new List<Department>();
        }
    }
}
