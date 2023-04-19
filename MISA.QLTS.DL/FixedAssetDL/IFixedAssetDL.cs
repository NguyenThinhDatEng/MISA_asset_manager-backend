using MISA.QLTS.Common.Entitites;
using MISA.QLTS.Common.Entitites.DTO;

namespace MISA.QLTS.DL
{
    public interface IFixedAssetDL : IBaseDL<FixedAsset>
    {
        /// <summary>
        /// API lấy mã tài sản cố định mới
        /// </summary>
        /// <returns>Mã tài sản tiếp theo</returns>
        /// Author: NVThinh 16/11/2022
        public string GetNextCode();

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
            List<Guid>? selectedIDs);

        /// <summary>
        /// Kiểm tra các tài sản đã có chứng từ chưa
        /// </summary>
        /// <param name="fixedAssetIDs">Danh sách các ID tài sản</param>
        /// <returns>Mã chứng từ</returns>
        /// <author>NVThinh 16/1/2023</author>
        public string CheckExistedVoucher(List<Guid> fixedAssetIDs);

        /// <summary>
        /// API Tạo mới tài sản cố định
        /// </summary>
        /// <param name="fixedAsset">Đối tượng tài sản cố định</param>
        /// <returns>ID tài sản cố định được thêm</returns>
        /// Author: NVThinh (11/11/2022)
        public ServiceResponse InsertFixedAsset(FixedAsset fixedAsset);

        /// <summary>
        /// API cập nhật thông tin tài sản
        /// </summary>
        /// <param name="fixedAssetID">ID tài sản được cập nhật</param>
        /// <param name="fixedAsset">Đối tượng tài sản cố định</param>
        /// <returns>ID bản ghi được cập nhật</returns>
        /// Author: NVThinh (11/11/2022)
        public ServiceResponse UpdateFixedAsset(Guid fixedAssetID, FixedAsset fixedAsset);
    }
}
