using MISA.QLTS.Common.Entitites;
using MISA.QLTS.Common.Entitites.DTO;
using MISA.QLTS.DL;

namespace MISA.QLTS.BL
{
    public class FixedAssetBL : IFixedAssetBL
    {
        #region Field

        private IFixedAssetDL _fixedAssetDL;

        #endregion

        #region Constructor

        public FixedAssetBL(IFixedAssetDL fixedAssetDL)
        {
            _fixedAssetDL = fixedAssetDL;
        }

        #endregion

        #region GET
        /// <summary>
        /// Lấy thông tin tất cả tài sản
        /// </summary>
        /// <returns>Danh sách tài sản</returns>
        /// Create by: NVThinh (16/11/2022)
        public IEnumerable<dynamic> GetAllFixedAsset()
        {
            return _fixedAssetDL.GetAllFixedAsset();
        }

        /// <summary>
        /// Lấy thông tin 1 tài sản theo ID
        /// </summary>
        /// <param name="fixedAssetID">ID tài sản muốn lấy</param>
        /// <returns>Thông tin 1 nhân viên theo ID</returns>
        /// Create by: NVThinh (16/11/2022)
        public FixedAsset GetFixedAssetByID(Guid fixedAssetID)
        {
            return _fixedAssetDL.GetFixedAssetByID(fixedAssetID);
        }

        /// <summary>
        /// API lấy mã tài sản cố định mới
        /// </summary>
        /// <returns>Mã tài sản cố định mới</returns>
        /// created by: NVThinh 16/11/2022
        public string GetMaxFixedAssetCode()
        {
            return _fixedAssetDL.GetMaxFixedAssetCode();
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
            return _fixedAssetDL.InsertFixedAsset(fixedAsset);
        }

        /// <summary>
        /// Xóa nhiều bản ghi
        /// </summary>
        /// <param name="listFixedAssetID">Danh sách ID các tài sản cần xóa</param>
        /// <returns>Số lượng tài sản được xóa</returns>
        /// Created by: NVThinh (11/11/2022)
        public bool DeleteMultipleFixedAsset(ListFixedAssetID fixedAssetIDs)
        {
            return _fixedAssetDL.DeleteMultipleFixedAsset(fixedAssetIDs);
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
            return _fixedAssetDL.UpdateFixedAsset(fixedAssetID, fixedAsset);
        }

        #endregion

        #region DELETE
        /// <summary>
        /// API Xóa 01 tài sản
        /// </summary>
        /// <param name="fixedAssetID">ID tài sản cần xóa</param>
        /// <returns>ID tài sản được xóa</returns>
        /// Created by: NVThinh (11/11/2022)
        public int DeleteFixedAsset(Guid fixedAssetID)
        {
            return _fixedAssetDL.DeleteFixedAsset(fixedAssetID);
        }
        #endregion
    }
}
