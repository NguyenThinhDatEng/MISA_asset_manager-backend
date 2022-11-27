namespace MISA.QLTS.Common.Entitites.DTO
{
    /// <summary>
    /// Kết quả trả về của API lấy danh sách bằng cách lọc và phân trang
    /// </summary>
    public class PagingResult
    {
        #region Property

        /// <summary>
        /// Danh sách tài sản cố định trả về từ API filter
        /// </summary>
        public List<FixedAsset> Data { get; set; }

        /// <summary>
        /// Tổng số bản ghi
        /// </summary>
        public int TotalOfRecords { get; set; }

        /// <summary>
        /// Tổng cột số lượng
        /// </summary>
        //public int TotalOfQuantities { get; set; }
        #endregion
    }
}
