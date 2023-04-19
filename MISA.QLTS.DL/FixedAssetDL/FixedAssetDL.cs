using Dapper;
using MISA.QLTS.Common.Entitites;
using MISA.QLTS.Common.Entitites.DTO;
using MISA.QLTS.Common.Enums;
using MISA.QLTS.Common.Resources;
using MySqlConnector;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace MISA.QLTS.DL
{
    public class FixedAssetDL : BaseDL<FixedAsset>, IFixedAssetDL

    {
        #region Method

        #region GET

        /// <summary>
        /// API lấy tài sản theo bộ lọc và phân trang
        /// </summary>
        /// <param name="keyword">Từ khóa tìm kiếm</param>
        /// <param name="departmentID">Mảng ID bộ phận sử dụng</param>
        /// <param name="fixedAssetCategoryID">Mảng ID mã bộ phận sử dụng</param>
        /// <param name="offset">vị trí của bản ghi bắt đầu lấy</param>
        /// <param name="limit">số bản ghi lấy ra</param>
        /// <param name="isIncrement">true nếu danh sách là ghi tăng chứng từ</param>
        /// <param name="selectedIDs">mảng chứa các ID đã được lựa chọn để ghi tăng chứng từ</param>
        /// <returns>Danh sách tài sản cố định và tổng số bản ghi</returns>
        /// <author>NVThinh 11/1/2023</author>
        public PagingResult<FixedAsset> GetFixedAssetByFilterAndPaging(
            string? keyword,
            Guid? departmentID,
            Guid? fixedAssetCategoryID,
            int offset,
            int limit,
            bool isIncrement,
            List<Guid>? selectedIDs)
        {
            // Chuẩn bị tên Stored procedure
            string procedureName = "Proc_GetFixedAssetPaging";

            // Chuẩn bị tham số đầu vào cho stored procedure
            var parameters = new DynamicParameters();
            parameters.Add("@v_Offset", offset);
            parameters.Add("@v_Limit", limit);
            parameters.Add("@v_Sort", "created_date DESC");

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

            // Nếu lọc dữ liệu trong ghi tăng tài sản
            if (isIncrement == true)
            {
                andConditions.Add("fixed_asset_id not in (select fixed_asset_id from voucher_detail)");
                if (selectedIDs != null && selectedIDs.Count > 0)
                {
                    var jsonString = JsonSerializer.Serialize(selectedIDs);
                    jsonString = jsonString.Substring(1, jsonString.Length - 2);
                    andConditions.Add($"fixed_asset_id not in ({jsonString})");
                }
            }

            // Nối chuỗi
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
                    // Khởi tạo danh sách tài sản
                    var fixedAssetList = multipleResults.Read<FixedAsset>().ToList();
                    // Khởi tạo tổng số bản ghi thu được
                    var totalOfRecords = multipleResults.Read<int>().Single();
                    return new PagingResult<FixedAsset>
                    {
                        Data = fixedAssetList,
                        TotalOfRecords = totalOfRecords,
                    };
                }
            }
            // Thất bại
            return new PagingResult<FixedAsset>
            {
                TotalOfRecords = -1,
            };
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
            fixedAsset.fixed_asset_id = Guid.NewGuid();
            fixedAsset.created_by = "Nguyễn Văn Thịnh";

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
            fixedAsset.modified_by = "Nguyễn Văn Thịnh";

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

        #endregion

        /// <summary>
        /// Kiểm tra các tài sản đã có chứng từ chưa
        /// </summary>
        /// <param name="fixedAssetIDs">Danh sách các ID tài sản</param>
        /// <returns>Mã chứng từ</returns>
        /// <author>NVThinh 16/1/2023</author>
        public string CheckExistedVoucher(List<Guid> fixedAssetIDs)
        {
            // Chuẩn bị tên Stored procedure
            string procedureName = "Proc_CheckExistedVoucher";

            // Chuẩn bị tham số đầu vào cho stored procedure
            var parameters = new DynamicParameters();
            parameters.Add("@fixedAssetIDs", JsonSerializer.Serialize(fixedAssetIDs));

            //Khởi tạo kết nối DB MySQL
            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                // Thực hiện gọi vào DB để chạy stored procedure với tham số đầu vào ở trên
                var voucherCode = mySqlConnection.QueryFirstOrDefault<string>(procedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
                // Trả về
                return voucherCode;
            }
        } 

        /// <summary>
        /// Lọc mảng tài sản (loại bỏ những đối tượng có id nằm trong những id được chọn)
        /// </summary>
        /// <param name="fixedAssetList">Danh sách tài sản cố định</param>
        /// <param name="selectedIDs">Danh sách ID được chọn</param>
        /// <returns>Danh sách tài sản đã được lọc</returns>
        /// <author>NVThinh 11/1/2023</author>
        private List<FixedAsset> _filterList(List<FixedAsset> fixedAssetList, List<Guid> selectedIDs)
        {
            // Lọc dữ liệu (loại bỏ các tài sản đã được chọn)
            int i = 0;
            while (selectedIDs.Count > 0 && i < fixedAssetList.Count)
            {
                for (int j = 0; j < selectedIDs.Count; j++)
                {
                    if (fixedAssetList[i].fixed_asset_id == selectedIDs[j])
                    {
                        selectedIDs.RemoveAt(j);
                        fixedAssetList.RemoveAt(i);
                        i--;
                        break;
                    }
                }
                i++;
            }
            // Trả về mảng đã được lọc
            return fixedAssetList;
        }

        #endregion
    }
}
