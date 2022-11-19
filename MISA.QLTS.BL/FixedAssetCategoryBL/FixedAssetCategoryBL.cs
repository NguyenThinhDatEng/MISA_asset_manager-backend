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
    public class FixedAssetCategoryBL : IFixedAssetCategoryBL
    {
        #region Field
        private IFixedAssetCategoryDL _fixedAssetCategoryDL;
        #endregion

        #region Constructor
        public FixedAssetCategoryBL(IFixedAssetCategoryDL fixedAssetCategoryDL)
        {
            _fixedAssetCategoryDL = fixedAssetCategoryDL;
        }
        #endregion
        public IEnumerable<dynamic> GetAllFixedAssetCategory()
        {
            //Khởi tạo kết nối DB MySQL
            var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString);

            // Chuẩn bị tên Stored procedure
            string procedureName = "Proc_GetAllAssetCategory";

            // Thực hiện gọi vào DB để chạy stored procedure với tham số đầu vào ở trên
            var fixedAssetCategories = mySqlConnection.Query(procedureName, commandType: System.Data.CommandType.StoredProcedure);

            // Xử lý kết quả trả về
            if (fixedAssetCategories != null)
                return fixedAssetCategories;
            return new List<FixedAssetCategoryBL>();
        }
    }
}
