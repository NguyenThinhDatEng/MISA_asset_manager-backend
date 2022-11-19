using MISA.QLTS.Common.Entitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.DL
{
    public interface IDepartmentDL
    {
        /// <summary>
        /// Lấy thông tin tất cả bộ phận sử dụng
        /// </summary>
        /// <returns>Danh sách bộ phận sử dụng</returns>
        /// Create by: NVThinh (16/11/2022)
        public IEnumerable<dynamic> GetAllDepartment();

    }
}
