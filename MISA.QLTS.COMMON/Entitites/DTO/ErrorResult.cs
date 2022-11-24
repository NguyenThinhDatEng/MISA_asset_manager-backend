using MISA.QLTS.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Common.Entitites.DTO
{
    public class ErrorResult
    {
        /// <summary>
        /// Mã lỗi
        /// </summary>
        public QLTSErrorCode ErrorCode { get; set; }

        /// <summary>
        /// Message trả về cho developer
        /// </summary>
        public string DevMsg { get; set; }

        /// <summary>
        /// Message trả về cho người dùng
        /// </summary>
        public string UserMsg { get; set; }

        /// <summary>
        /// Thông tin thêm
        /// </summary>
        public List<string> MoreInfo { get; set; }

        /// <summary>
        /// ID dùng để truy vết lỗi khi log lại
        /// </summary>
        public string TraceID { get; set; }
    }
}
