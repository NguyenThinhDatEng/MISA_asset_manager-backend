using MISA.QLTS.Common.Entitites;
using MISA.QLTS.DL;

namespace MISA.QLTS.BL
{
    public class BudgetBL : BaseBL<Budget>, IBudgetBL
    {
        #region Field

        private IBudgetDL _budgetDL;

        #endregion

        #region Constructor
        public BudgetBL(IBudgetDL budgetDL) : base(budgetDL)
        {
            _budgetDL = budgetDL;
        }

        #endregion
    }
}
