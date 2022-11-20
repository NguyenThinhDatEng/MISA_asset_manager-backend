using Dapper;
using MISA.QLTS.BL;
using MISA.QLTS.Common.Entitites;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.DL
{
    public class FixedAssetCategoryBL : BaseBL<FixedAssetCategory>, IFixedAssetCategoryBL
    {
        #region Field
        private IFixedAssetCategoryDL _fixedAssetCategoryDL;
        #endregion

        #region Constructor
        public FixedAssetCategoryBL(IFixedAssetCategoryDL fixedAssetCategoryDL) : base(fixedAssetCategoryDL)
        {
            _fixedAssetCategoryDL = fixedAssetCategoryDL;
        }
        #endregion
    }
}
