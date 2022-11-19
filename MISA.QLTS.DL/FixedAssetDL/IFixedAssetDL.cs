using MISA.QLTS.Common.Entitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.DL
{
    public interface IFixedAssetDL
    {
        /// <summary>
        /// Lấy thông tin tất cả tài sản
        /// </summary>
        /// <returns>Danh sách tài sản</returns>
        /// Create by: NVThinh (16/11/2022)
        public IEnumerable<dynamic> GetAllFixedAsset();

        /// <summary>
        /// Lấy thông tin 1 tài sản theo ID
        /// </summary>
        /// <param name="fixedAssetID">ID tài sản muốn lấy</param>
        /// <returns>Thông tin 1 nhân viên theo ID</returns>
        /// Create by: NVThinh (16/11/2022)
        public FixedAsset GetFixedAssetByID(Guid fixedAssetID);
    }
}
