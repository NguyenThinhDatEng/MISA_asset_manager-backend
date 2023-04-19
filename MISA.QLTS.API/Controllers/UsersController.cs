using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MISA.QLTS.BL;
using MISA.QLTS.Common.Entitites;
using MISA.QLTS.Common.Entitites.DTO;
using MISA.QLTS.Common.Enums;
using MISA.QLTS.Common.Resources;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace MISA.QLTS.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        #region Field

        private IUserBL _userBL;

        #endregion

        #region Constructor

        public UsersController(IUserBL userBL)
        {
            _userBL = userBL;
        }

        #endregion

        #region Method

        /// <summary>
        /// API đăng nhập
        /// </summary>
        /// <param name="user">Đối tượng người dùng</param>
        /// <returns>User</returns>
        /// <author>NVThinh 29/12/2022</author>
        [HttpPost("login")]
        public async Task<IActionResult> Login(User userInfo)
        {
            try
            {
                // Xác thực dữ liệu
                var user = await _userBL.AuthenticateUser(userInfo);

                // Không tìm thấy user
                if (user == null)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, new ErrorResult
                    {
                        ErrorCode = QLTSErrorCode.Unauthorized,
                        DevMsg = Errors.DevMsg_Unauthorized,
                        UserMsg = Errors.UserMsg_Unauthorized,
                    });
                }
                //Created a claim
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.userName),
                    new Claim(ClaimTypes.Role, "Administrator"),
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // Đăng nhập
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    new AuthenticationProperties
                    {
                        ExpiresUtc = DateTime.UtcNow.AddDays(1),
                        IsPersistent = true
                    });

                // Trả về
                return StatusCode(StatusCodes.Status200OK, user);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult
                {
                    ErrorCode = QLTSErrorCode.Exception,
                    DevMsg = Errors.DevMsg_Exception,
                    UserMsg = Errors.UserMsg_Exception,
                    MoreInfo = new List<string> { ex.Message },
                });
            }
        }

        [Authorize]
        [HttpGet("logout")]
        public async Task logout()
        {
            // Clear the existing external cookie
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
        }
        #endregion
    }
}