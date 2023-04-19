using MISA.QLTS.Common.Entitites;

namespace MISA.QLTS.BL
{
    public class UserBL : IUserBL
    {
        /// <summary>
        /// Xác thực danh tính
        /// Chỉ duy nhất 1 tài khoản có thể đăng nhập
        /// </summary>
        /// <param name="user">Đối tượng người dùng</param>
        /// <returns>User</returns>
        /// <author>NVThinh 29/12/2022</author>
        public async Task<User> AuthenticateUser(User user)
        {
            if (user.userName == "TheWill" && user.password == "123")
            {
                return user;
            }
            return null;
        }
    }
}

     
