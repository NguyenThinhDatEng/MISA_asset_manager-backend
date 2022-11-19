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
    public class FixedAssetDL : IFixedAssetDL
    {
        /// <summary>
        /// Lấy thông tin tất cả tài sản
        /// </summary>
        /// <returns>Danh sách tài sản</returns>
        /// Create by: NVThinh (16/11/2022)
        public IEnumerable<dynamic> GetAllFixedAsset()
        {
            //Khởi tạo kết nối DB MySQL
            var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString);

            // Chuẩn bị tên Stored procedure
            string procedureName = "Proc_GetAllAsset";

            // Thực hiện gọi vào DB để chạy stored procedure với tham số đầu vào ở trên
            var fixedAssets = mySqlConnection.Query(procedureName, commandType: System.Data.CommandType.StoredProcedure);

            // Xử lý kết quả trả về từ Database
            if (fixedAssets != null)
                return fixedAssets;
            return new List<FixedAsset>();
        }

        /// <summary>
        /// Lấy thông tin 1 tài sản theo ID
        /// </summary>
        /// <param name="fixedAssetID">ID tài sản muốn lấy</param>
        /// <returns>Thông tin 1 nhân viên theo ID</returns>
        /// Create by: NVThinh (16/11/2022)
        public FixedAsset GetFixedAssetByID(Guid fixedAssetID)
        {
            //Khởi tạo kết nối DB MySQL
            var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString);

            // Chuẩn bị câu lệnh SQL
            string procedureName = "Proc_GetFixedAssetByID";

            //Chuẩn bị tham số đầu vào
            var parameters = new DynamicParameters();
            parameters.Add("@fixedAssetID", fixedAssetID);

            //Thực hiện gọi vào DB
            var fixedAsset = mySqlConnection.QueryFirstOrDefault<FixedAsset>(procedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

            //Xử lý kết quả trả về
            // Thành công
            if (fixedAsset != null)
                return fixedAsset;
            // Thất bại
            return null;
        }
    }
}
