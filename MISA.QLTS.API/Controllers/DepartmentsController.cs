using Microsoft.AspNetCore.Mvc;
using MISA.QLTS.API.Controllers;
using MISA.QLTS.BL;
using MISA.QLTS.Common.Entitites;

namespace MISA.QLTS.COMMON.Controllers
{
    [ApiController]
    public class DepartmentsController : BasesController<Department>
    {

        #region Constructor

        public DepartmentsController(BaseBL<Department> departmentBL) : base(departmentBL) { }

        #endregion
    }
}
