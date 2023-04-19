using Microsoft.AspNetCore.Mvc;
using MISA.QLTS.Common.Entitites;

namespace MISA.QLTS.BL
{
    public interface IUserBL
    {
        /// <summary>
        /// Xác thực danh tính
        /// </summary>
        /// <param name="user">Đối tượng người dùng</param>
        /// <returns>User</returns>
        /// <author>NVThinh 29/12/2022</author>
        public Task<User> AuthenticateUser(User user);
    }
}
