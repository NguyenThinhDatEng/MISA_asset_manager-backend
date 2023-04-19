using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Common.Entitites
{
    public class User
    {
        /// <summary>
        /// Tên đăng nhập
        /// </summary>
        public string userName { get; set; }

        /// <summary>
        /// Mật khẩu đăng nhập
        /// </summary>
        public string password { get; set; }
    }
}
