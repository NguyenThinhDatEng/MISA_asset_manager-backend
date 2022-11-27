using Dapper;
using MISA.QLTS.Common.Entitites;
using MISA.QLTS.Common.Entitites.DTO;
using MySqlConnector;

namespace MISA.QLTS.DL
{
    public class FixedAssetDL : BaseDL<FixedAsset>, IFixedAssetDL
    {
        #region Method

        #region GET

        /// <summary>
        /// API lấy mã tài sản cố định mới
        /// </summary>
        /// <returns>Mã tài sản cố định mới</returns>
        /// Author: NVThinh 16/11/2022
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

        /// <summary>
        /// API lấy tài sản theo bộ lọc và phân trang
        /// </summary>
        /// <param name="keyword">Từ khóa tìm kiếm</param>
        /// <param name="departmentID">Mảng ID bộ phận sử dụng</param>
        /// <param name="fixedAssetCategoryID">Mảng ID mã bộ phận sử dụng</param>
        /// <param name="offset">vị trí của bản ghi bắt đầu lấy</param>
        /// <param name="limit">số bản ghi lấy ra</param>
        /// <returns>Danh sách tài sản cố định và tổng số bản ghi</returns>
        /// <author>NVThinh 27/11/2022</author>
        public PagingResult GetFixedAssetByFilterAndPaging(
            string? keyword,
            Guid? departmentID,
            Guid? fixedAssetCategoryID,
            int offset = 0,
            int limit = 20)
        {
            // Chuẩn bị tên Stored procedure
            string procedureName = "Proc_GetFixedAssetPaging";

            // Chuẩn bị tham số đầu vào cho stored procedure
            var parameters = new DynamicParameters();
            parameters.Add("@v_Offset", offset);
            parameters.Add("@v_Limit", limit);
            parameters.Add("@v_Sort", "fixed_asset_code DESC");

            // Chuẩn bị cho điều kiện where
            string whereClause = "";
            var orConditions = new List<string>();
            var andConditions = new List<string>();

            // Tìm kiếm
            if (keyword != null && keyword != "")
            {
                orConditions.Add($"fixed_asset_code LIKE '%{keyword}%'");
                orConditions.Add($"fixed_asset_name LIKE '%{keyword}%'");
            }
            if (orConditions.Count > 0)
            {
                whereClause = $"({string.Join(" OR ", orConditions)})";
            }

            // Lọc theo loại tài sản
            if (fixedAssetCategoryID != Guid.Empty && fixedAssetCategoryID != null)
            {
                andConditions.Add($"fixed_asset_category_id like '%{fixedAssetCategoryID}%'");
            }

            // Lọc theo mã phòng ban
            if (departmentID != Guid.Empty && departmentID != null)
            {
                andConditions.Add($"department_id LIKE '%{departmentID}%'");
            }

            if (andConditions.Count > 0)
            {
                if (keyword != null)
                    whereClause += $" AND {string.Join(" AND ", andConditions)}";
                else
                    whereClause += $"{string.Join(" AND ", andConditions)}";
            }

            parameters.Add("@v_Where", whereClause);


            // Khởi tạo kết nối tới DB MySQL
            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                // Thực hiện gọi vào DB để chạy stored procedure với tham số đầu vào ở trên
                var multipleResults = mySqlConnection.QueryMultiple(procedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                // Xử lý kết quả trả về từ DB
                // Thành công
                if (multipleResults != null)
                {
                    var employees = multipleResults.Read<FixedAsset>().ToList();
                    var totalOfRecords = multipleResults.Read<int>().Single();
                    //var totalOfQuantities = multipleResults.Read<int>().Single();
                    return new PagingResult
                    {
                        Data = employees,
                        TotalOfRecords = totalOfRecords,
                        //TotalOfQuantities = totalOfQuantities,
                    };
                }
            }
            // Thất bại
            return null;
        }

        #endregion

        #region POST

        /// <summary>
        /// API Tạo mới tài sản cố định
        /// </summary>
        /// <param name="fixedAsset">Đối tượng tài sản cố định</param>
        /// <returns>ID tài sản cố định được thêm</returns>
        /// Author: NVThinh (11/11/2022)
        public ServiceResponse InsertFixedAsset(FixedAsset fixedAsset)
        {
            // Chuẩn bị tên Stored procedure
            string procedureName = "Proc_CreateAsset";

            // Chuẩn bị tham số đầu vào cho stored procedure
            var newID = Guid.NewGuid();
            fixedAsset.fixed_asset_id = newID;
            fixedAsset.created_by = "Nguyễn Văn Thịnh";
            //var parameters = new DynamicParameters();
            //Console.WriteLine(newID);
            //parameters.Add("@fixed_asset_id", newID);
            //parameters.Add("@fixed_asset_code", fixedAsset.fixed_asset_code);
            //parameters.Add("@fixed_asset_name", fixedAsset.fixed_asset_name);
            //parameters.Add("@department_id", fixedAsset.department_id);
            //parameters.Add("@department_code", fixedAsset.department_code);
            //parameters.Add("@department_name", fixedAsset.department_name);
            //parameters.Add("@fixed_asset_category_id", fixedAsset.fixed_asset_category_id);
            //parameters.Add("@fixed_asset_category_code", fixedAsset.fixed_asset_category_code);
            //parameters.Add("@fixed_asset_category_name", fixedAsset.fixed_asset_category_name);
            //parameters.Add("@purchase_date", fixedAsset.purchase_date);
            //parameters.Add("@cost", fixedAsset.cost);
            //parameters.Add("@quantity", fixedAsset.quantity);
            //parameters.Add("@depreciation_rate", fixedAsset.depreciation_rate);
            //parameters.Add("@tracked_year", fixedAsset.tracked_year);
            //parameters.Add("@life_time", fixedAsset.life_time);
            //parameters.Add("@production_date", fixedAsset.production_date);
            

            //Khởi tạo kết nối DB MySQL
            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                // Thực hiện gọi vào DB để chạy stored procedure với tham số đầu vào ở trên
                var numberOfRowsAffected = mySqlConnection.Execute(procedureName, fixedAsset, commandType: System.Data.CommandType.StoredProcedure);

                if (numberOfRowsAffected > 0)

                    return new ServiceResponse
                    {
                        Success = true
                    };
                else
                {
                    return new ServiceResponse
                    {
                        Success = false
                    };
                }
            }
        }

        /// <summary>
        /// Xóa nhiều bản ghi
        /// </summary>
        /// <param name="fixedAssetIDs">Danh sách ID các tài sản cần xóa</param>
        /// <returns>Số lượng tài sản được xóa</returns>
        /// <author>NVThinh 27/11/2022</author>
        public ServiceResponse DeleteMultipleFixedAsset(ListFixedAssetID fixedAssetIDs)
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

                    return new ServiceResponse { Success = true };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    return new ServiceResponse { Success = false, Data = new List<string> { ex.Message } };
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
        /// Author: NVThinh (11/11/2022)
        public ServiceResponse UpdateFixedAsset(Guid fixedAssetID, FixedAsset fixedAsset)
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

            //Khởi tạo kết nối DB MySQL
            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                // Thực hiện gọi vào DB để chạy stored procedure với tham số đầu vào ở trên
                var numberOfRowsAffected = mySqlConnection.Execute(procedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                if (numberOfRowsAffected > 0)

                    return new ServiceResponse
                    {
                        Success = true
                    };
                else
                {
                    return new ServiceResponse
                    {
                        Success = false
                    };
                }
            }
        }

        #endregion

        #region DELETE

        /// <summary>
        /// API Xóa 01 tài sản
        /// </summary>
        /// <param name="fixedAssetID">ID tài sản cần xóa</param>
        /// <returns>ID tài sản được xóa</return
        /// <author>NVThinh 27/11/2022</author>
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

        #endregion
    }
}
