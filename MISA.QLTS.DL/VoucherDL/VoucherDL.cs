using Dapper;
using MISA.QLTS.Common.Entitites;
using MISA.QLTS.Common.Entitites.DTO;
using MISA.QLTS.Common.Entitites.DTO.VoucherAsset;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace MISA.QLTS.DL
{
    public class VoucherDL : BaseDL<Voucher>, IVoucherDL
    {

        #region Method


        #region GET
        /// <summary>
        /// API lấy chứng từ theo bộ lọc và phân trang
        /// </summary>
        /// <param name="keyword">Từ khóa tìm kiếm</param>
        /// <param name="offset">vị trí của bản ghi bắt đầu lấy</param>
        /// <param name="limit">số bản ghi lấy ra</param>
        /// <returns>Danh sách chứng từ và tổng số bản ghi thu được</returns>
        /// <author>NVThinh 9/1/2023</author>
        public PagingResult<Voucher> GetVouchersByFilterAndPaging(
            string? keyword,
            int offset,
            int limit)
        {
            // Chuẩn bị tên Stored procedure
            string procedureName = "Proc_GetVoucherPaging";

            // Chuẩn bị tham số đầu vào cho stored procedure
            var parameters = new DynamicParameters();
            parameters.Add("@v_Offset", offset);
            parameters.Add("@v_Limit", limit);
            parameters.Add("@v_Sort", "");

            // Chuẩn bị cho điều kiện where
            string whereClause = "";
            var orConditions = new List<string>();

            // Tìm kiếm
            if (keyword != null && keyword != "")
            {
                orConditions.Add($"voucher_code LIKE '%{keyword}%'");
                orConditions.Add($"description LIKE '%{keyword}%'");
            }
            if (orConditions.Count > 0)
            {
                whereClause = $"({string.Join(" OR ", orConditions)})";
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
                    var vouchers = multipleResults.Read<Voucher>().ToList();
                    var totalOfRecords = multipleResults.Read<int>().Single();
                    return new PagingResult<Voucher>
                    {
                        Data = vouchers,
                        TotalOfRecords = totalOfRecords,
                    };
                }
            }
            // Thất bại
            return new PagingResult<Voucher>
            {
                TotalOfRecords = -1
            };
        }

        /// <summary>
        /// API lấy tất cả tài sản đã đăng ký chứng từ
        /// </summary>
        /// <param name="voucher_id">ID voucher</param>
        /// <returns>Danh sách các tài sản đăng ký voucher đó</returns>
        /// <author>NVThinh 10/1/2023</author>
        public List<VoucherAsset> GetVoucherDetail(Guid voucher_id)
        {
            // Chuẩn bị tên Stored procedure
            string procedureName = "Proc_GetVoucherDetail";

            // Chuẩn bị tham số đầu vào
            var parameters = new DynamicParameters();
            parameters.Add("voucher_id", voucher_id);

            //Khởi tạo kết nối DB MySQL
            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                // Thực hiện gọi vào DB để chạy stored procedure với tham số đầu vào ở trên
                var records = (List<VoucherAsset>)mySqlConnection.Query<VoucherAsset>(procedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                // Xử lý kết quả trả về từ DB
                return records;
            }
        } 
        #endregion

        #region POST

        /// <summary>
        /// Thêm mới chứng từ
        /// </summary>
        /// <param name="voucherResult">Đối tượng chứa chứng từ và mảng các voucher detail</param>
        /// <returns></returns>
        /// <author>NVThinh 12/1/2023</author>
        public ServiceResponse InsertVoucher(VoucherResult voucherResult)
        {
            // Chuẩn bị tên procedure
            string storedProcedureName = "Proc_CreateVoucher";
            // Chuẩn bị tham số đầu vào
            var newVoucherID = Guid.NewGuid();
            voucherResult.voucher.voucher_id = newVoucherID;
            foreach(var obj in voucherResult.voucherDetailList)
            {
                obj.voucher_id = newVoucherID;
                obj.voucher_detail_id = Guid.NewGuid();
            }
            var parameters = new DynamicParameters();
            parameters.Add("@voucherID", newVoucherID);
            parameters.Add("@voucher", JsonSerializer.Serialize(voucherResult));

            // Khởi tạo kết nối tới DB MySQL
            using (var mysqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                // Khai báo tên prodecure

                mysqlConnection.Open();

                // Bắt đầu transaction.
                using (var transaction = mysqlConnection.BeginTransaction())
                {
                    try
                    {
                        var numberOfAffectedRows = mysqlConnection.QueryFirstOrDefault<int>(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure, transaction: transaction);
                        
                        if (numberOfAffectedRows == voucherResult.voucherDetailList?.Count)
                        {
                            transaction.Commit();
                            return new ServiceResponse
                            {
                                Success = true,
                            };
                        }
                        else
                        {
                            transaction.Rollback();
                            return new ServiceResponse { Success = false };
                        }
                    }
                    catch
                    {
                        transaction.Rollback();
                        return new ServiceResponse{ Success = false };
                    }
                    finally
                    {
                        mysqlConnection.Close();
                    }
                }
            }
        }
        #endregion

        #region PUT

        /// <summary>
        /// Cập nhật chứng từ
        /// </summary>
        /// <param name="voucherID">Mã chứng từ</param>
        /// <param name="voucherDetails">Mảng các chi tiết chứng từ</param>
        /// <returns>1 đối tượng ServiceRespone</returns>
        /// <author>NVThinh 13/1/2023</author>
        public ServiceResponse UpdateVoucher(VoucherResult voucherResult)
        {
            try
            {
                // Chuẩn bị tên procedure
                string storedProcedureName = "Proc_UpdateVoucher";
                // Chuẩn bị tham số đầu vào
                // Cập nhật các voucher detail
                foreach (var obj in voucherResult.voucherDetailList)
                {
                    obj.voucher_detail_id = Guid.NewGuid();
                    obj.voucher_id = voucherResult.voucher.voucher_id;
                }

                // Khởi tạo tham số
                var parameters = new DynamicParameters();
                parameters.Add("@voucherID", voucherResult.voucher.voucher_id);
                parameters.Add("@voucher", JsonSerializer.Serialize(voucherResult));

                // Khởi tạo kết nối tới DB MySQL
                using (var mysqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
                {
                    // Mở connection
                    mysqlConnection.Open();

                    // Bắt đầu transaction.
                    using (var transaction = mysqlConnection.BeginTransaction())
                    {
                        try
                        {
                            // Chọc vào DB
                            mysqlConnection.Execute(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure, transaction: transaction);
                            // Commit nếu không có exception
                            transaction.Commit();
                            // Trả về
                            return new ServiceResponse
                            {
                                Success = true,
                                Data = new List<string> { voucherResult.voucher.voucher_id.ToString() }
                            };
                        }
                        catch (Exception ex)
                        {
                            // Rollback nếu có exception
                            transaction.Rollback();
                            // Trả về
                            return new ServiceResponse { Success = false, Data = new List<string> { ex.Message } };
                        }
                        finally
                        {
                            // Đóng connection
                            mysqlConnection.Close();
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #endregion
    }
}
