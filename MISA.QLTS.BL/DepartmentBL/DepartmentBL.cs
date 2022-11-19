using MISA.QLTS.DL;

namespace MISA.QLTS.BL
{
    public class DepartmentBL : IDepartmentBL
    {
        #region Field
        private IDepartmentDL _departmentDL;
        #endregion

        #region Constructor
        public DepartmentBL(IDepartmentDL departmentDL)
        {
            _departmentDL = departmentDL;
        }
        #endregion

        /// <summary>
        /// Lấy thông tin tất cả bộ phận sử dụng
        /// </summary>
        /// <returns>Danh sách bộ phận sử dụng</returns>
        /// Create by: NVThinh (16/11/2022)
        public IEnumerable<dynamic> GetAllDepartment()
        {
            return _departmentDL.GetAllDepartment();
        }
    }
}
