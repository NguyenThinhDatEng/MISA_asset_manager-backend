using MISA.QLTS.Common.Enums;
using MISA.QLTS.Common.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MISA.QLTS.Common.Attributes
{
    public class RequiredFieldAttribute : ValidationAttribute
    {
        #region Field

        public QLTSType dataType;

        private string field;   
        #endregion

        #region Constructor

        public RequiredFieldAttribute() { }

        public RequiredFieldAttribute(string field)
        {
            this.field = field;
            dataType = QLTSType.Text;
        }
        #endregion

        #region Method

        public override bool IsValid(object? value)
        {
            // Nếu dữ liệu là kiểu text
            if (String.IsNullOrEmpty(value?.ToString()))
            {
                ErrorMessage = field + " là thông tin bắt buộc";
                return false;
            }
            else
            {
                // Nếu dữ liệu là kiểu số
                if (dataType == QLTSType.Number && value?.ToString() == "0")
                {
                    ErrorMessage = field + " là thông tin bắt buộc";
                    return false;
                }
            }

            return true;
        }
        #endregion
    }
}
