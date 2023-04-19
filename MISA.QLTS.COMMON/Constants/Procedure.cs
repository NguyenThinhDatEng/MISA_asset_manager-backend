using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Common.Constants
{
    public class Procedure
    {
        /// <summary>
        /// Format tên procedure lấy tất cả bản ghi
        /// </summary>
        public static string GET_ALL = "Proc_GetAll{0}";

        /// <summary>
        /// Format tên procedure lấy bản ghi theo ID
        /// </summary>
        public static string GET_BY_ID = "Proc_Get{0}ByID";

        /// <summary>
        /// Format tên procedure lấy bản ghi theo mã
        /// </summary>
        public static string GET_BY_CODE = "Proc_Get{0}ByCode";

        /// <summary>
        /// Format tên procedure Lấy mã mới nhất
        /// </summary>
        public static string GET_LAST_CODE = "Proc_Get{0}LastCode";

        /// <summary>
        /// Format tên procedure Xóa 1 bản ghi theo ID
        /// </summary>
        public static string DELETE_BY_ID = "Proc_Delete{0}ByID";

        /// <summary>
        /// Format tên procedure Xóa nhiều bản ghi
        /// </summary>
        public static string DELETE_MULTIPLE = "Proc_DeleteMultiple{0}";
    }
}
