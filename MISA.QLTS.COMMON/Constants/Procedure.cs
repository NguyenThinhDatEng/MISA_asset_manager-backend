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
    }
}
