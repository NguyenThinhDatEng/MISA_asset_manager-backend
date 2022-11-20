using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Common.Entitites
{
    public class BaseEntity
    {
        /// <summary>
        /// Ngay tao thong tin
        /// </summary>
        public DateTime created_date { get; set; }

        /// <summary>
        /// Nguoi tao thong tin
        /// </summary>
        public string created_by { get; set; }

        /// <summary>
        /// Ngay chinh sua thong tin gần nhất
        /// </summary>
        public DateTime modified_date { get; set; }

        /// <summary>
        /// Nguoi chinh sua thong tin gần nhất
        /// </summary>
        public string modified_by { get; set; }
    }
}
