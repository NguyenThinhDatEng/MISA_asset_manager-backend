using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Common.Enums
{
    /// <summary>
    /// Enum sử dụng để mô tả lỗi xảy ra khi gọi API
    /// </summary>
    public enum QLTSErrorCode
    {
        /// <summary>
        /// Lỗi gặp exception
        /// </summary>
        Exception = 1,

        /// <summary>
        /// Lỗi không tìm thấy dữ liệu trả về
        /// </summary>
        NotFound = 2,

        /// <summary>
        /// Lỗi request
        /// </summary>
        BadRequest = 3
    }
}
