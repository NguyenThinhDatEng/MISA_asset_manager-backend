using Dapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.QLTS.API.Controllers;
using MISA.QLTS.Common.Entitites;
using MISA.QLTS.Common.Enums;
using MISA.QLTS.DL;
using MySqlConnector;

namespace MISA.QLTS.COMMON.Controllers
{
    [Authorize]
    public class FixedAssetCategoriesController : BasesController<FixedAssetCategory>
    {
        #region Field

        private IFixedAssetCategoryBL _fixedAssetCategoryBL;

        #endregion

        #region Constructor

        public FixedAssetCategoriesController(IFixedAssetCategoryBL fixedAssetCategoryBL) : base(fixedAssetCategoryBL)
        {
            _fixedAssetCategoryBL = fixedAssetCategoryBL;
        }

        #endregion
    }
}
