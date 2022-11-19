namespace MISA.QLTS.Common.Entitites.DTO
{
    /// <summary>
    /// Kết quả trả về của API lấy danh sách bằng cách lọc và phân trang
    /// </summary>
    public class PagingResult
    {
        #region Property
        public List<FixedAsset> Data { get; set; }

        public int totalOfRecords { get; set; } 
        #endregion
    }
}
