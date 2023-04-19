using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MISA.QLTS.API.Controllers;
using MISA.QLTS.BL;
using MISA.QLTS.Common.Entitites;

namespace MISA.QLTS.COMMON.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class DepartmentsController : BasesController<Department>
    {

        #region Constructor

        public DepartmentsController(IDepartmentBL departmentBL) : base(departmentBL) { }

        #endregion
    }
}
