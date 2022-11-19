using MISA.QLTS.Common.Entitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.DL
{
    public interface IFixedAssetCategoryBL
    {
        /// <summary>
        /// Lấy thông tin tất cả loại tài sản
        /// </summary>
        /// <returns>Danh sách loại tài sản</returns>
        /// Create by: NVThinh (16/11/2022)
        public IEnumerable<dynamic> GetAllFixedAssetCategory();

    }
}
