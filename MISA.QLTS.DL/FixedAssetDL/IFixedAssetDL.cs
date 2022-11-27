using MISA.QLTS.Common.Entitites;
using MISA.QLTS.Common.Entitites.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.DL
{
    public interface IFixedAssetDL : IBaseDL<FixedAsset>
    {
        /// <summary>
        /// API lấy mã tài sản cố định mới
        /// </summary>
        /// <returns>Mã tài sản cố định mới</returns>
        /// Author: NVThinh 16/11/2022
        public string GetMaxFixedAssetCode();

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
            int limit = 20);

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

        /// <summary>
        /// API Xóa 01 tài sản
        /// </summary>
        /// <param name="fixedAssetID">ID tài sản cần xóa</param>
        /// <returns>ID tài sản được xóa</return
        /// <author>NVThinh 27/11/2022</author>
        public int DeleteFixedAsset(Guid fixedAssetID);

        /// <summary>
        /// Xóa nhiều bản ghi
        /// </summary>
        /// <param name="fixedAssetIDs">Danh sách ID các tài sản cần xóa</param>
        /// <returns>Số lượng tài sản được xóa</returns>
        /// <author>NVThinh 27/11/2022</author>
        public ServiceResponse DeleteMultipleFixedAsset(ListFixedAssetID fixedAssetIDs);
    }
}
