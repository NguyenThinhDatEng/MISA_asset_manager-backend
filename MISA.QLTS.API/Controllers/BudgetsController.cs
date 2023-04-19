using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MISA.QLTS.API.Controllers;
using MISA.QLTS.BL;
using MISA.QLTS.Common.Entitites;

namespace MISA.QLTS.COMMON.Controllers
{
    [Authorize]
    public class BudgetsController : BasesController<Budget>
    {

        #region Constructor

        public BudgetsController(IBudgetBL budgetBL) : base(budgetBL) { }

        #endregion
    }
}
