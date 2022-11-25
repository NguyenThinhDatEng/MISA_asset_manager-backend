using MISA.QLTS.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Common.Entitites.DTO
{
    public class ServiceResponse
    {
        /// <summary>
        /// Kết quả thành công hay thất bại
        /// </summary>
        public Boolean Success { get; set; }

        /// <summary>
        /// Mã lỗi
        /// </summary>
        public QLTSErrorCode ErrorCode { get; set; }

        /// <summary>
        /// Dữ liệu phản hồi
        /// </summary>
        public List<string> Data { get; set; }
    }
}
