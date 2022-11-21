using Dapper;
using MISA.QLTS.Common.Entitites;
using MISA.QLTS.Common.Entitites.DTO;
using MySqlConnector;

namespace MISA.QLTS.DL
{
    public class FixedAssetDL : BaseDL<FixedAsset>, IFixedAssetDL
    {
        #region GET

        /// <summary>
        /// API lấy mã tài sản cố định mới
        /// </summary>
        /// <returns>Mã tài sản cố định mới</returns>
        /// created by: NVThinh 16/11/2022
        public string GetMaxFixedAssetCode()
        {

            // Chuẩn bị câu lệnh SQL
            string storedProcedureName = "Proc_GetMaxFixedAssetCode";

            //Khởi tạo kết nối DB MySQL
            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
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

                return newCode;
            }
        }

        #endregion

        #region POST

        /// <summary>
        /// API Tạo mới tài sản cố định
        /// </summary>
        /// <param name="fixedAsset">Đối tượng tài sản cố định</param>
        /// <returns>ID tài sản cố định được thêm</returns>
        /// Created by: NVThinh (11/11/2022)
        public int InsertFixedAsset(FixedAsset fixedAsset)
        {
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

            // Kiểm tra mã trùng
            if (CheckDuplicateCode(fixedAsset.fixed_asset_code, newID) == true)
                return -1;

            //Khởi tạo kết nối DB MySQL
            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                // Thực hiện gọi vào DB để chạy stored procedure với tham số đầu vào ở trên
                var numberOfRowsAffected = mySqlConnection.Execute(procedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                return numberOfRowsAffected;
            }
        }

        /// <summary>
        /// Xóa nhiều bản ghi
        /// </summary>
        /// <param name="listFixedAssetID">Danh sách ID các tài sản cần xóa</param>
        /// <returns>Số lượng tài sản được xóa</returns>
        public bool DeleteMultipleFixedAsset(ListFixedAssetID fixedAssetIDs)
        {
            //Khởi tạo kết nối DB MySQL
            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                mySqlConnection.Open();

                // Khởi tạo transaction
                var transaction = mySqlConnection.BeginTransaction();

                try
                {
                    // Chuẩn bị câu lệnh SQL
                    string procedureName = "Proc_DeleteMultipleFixedAsset";

                    // Chuẩn bị tham số đầu vào
                    var parameters = new DynamicParameters();
                    string IDs = "";
                    var list = fixedAssetIDs.FixedAssetIDs;
                    for (int i = 0; i < list.Count; i++)
                        IDs += "\'" + list[i] + "\',";
                    IDs = IDs.Remove(IDs.Length - 1);
                    Console.WriteLine(IDs);
                    parameters.Add("@IDs", IDs);

                    // Thực hiện gọi vào DB
                    mySqlConnection.Execute(procedureName, parameters, commandType: System.Data.CommandType.StoredProcedure, transaction: transaction);

                    // Cam kết thực hiện thành công
                    transaction.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    return false;
                }
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
        /// Created by: NVThinh (11/11/2022)
        public int UpdateFixedAsset(Guid fixedAssetID, FixedAsset fixedAsset)
        {


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

            // Kiểm tra mã trùng
            if (CheckDuplicateCode(fixedAsset.fixed_asset_code, fixedAssetID) == true)
                return -1;

            //Khởi tạo kết nối DB MySQL
            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                // Thực hiện gọi vào DB để chạy stored procedure với tham số đầu vào ở trên
                var numberOfRowsAffected = mySqlConnection.Execute(procedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                return numberOfRowsAffected;
            }

        }

        #endregion

        #region DELETE

        /// <summary>
        /// API Xóa 01 tài sản
        /// </summary>
        /// <param name="fixedAssetID">ID tài sản cần xóa</param>
        /// <returns>ID tài sản được xóa</return
        public int DeleteFixedAsset(Guid fixedAssetID)
        {
            // Chuẩn bị tên Stored procedure
            string procedureName = "Proc_DeleteAsset";

            // Chuẩn bị tham số đầu vào cho stored procedure
            var parameters = new DynamicParameters();
            parameters.Add("@fixedAssetID", fixedAssetID);

            //Khởi tạo kết nối DB MySQL
            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                // Thực hiện gọi vào DB để chạy stored procedure với tham số đầu vào ở trên
                var numberOfRowsAffected = mySqlConnection.Execute(procedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                return numberOfRowsAffected;
            }
        }

        #endregion
    }
}
