using MISA.QLTS.Common.Entitites;
using MISA.QLTS.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
