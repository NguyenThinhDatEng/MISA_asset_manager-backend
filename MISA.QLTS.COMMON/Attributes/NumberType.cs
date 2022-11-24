using MISA.QLTS.Common.Enums;
using MISA.QLTS.Common.Functions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Common.Attributes
{
    public class NumberType : ValidationAttribute
    {
        #region Method

        public override bool IsValid(object? value)
        {
            // Nếu dữ liệu là kiểu text
            var isNumber = ValidateData.IsNumber(value.ToString());
            if (!isNumber)
            {
                ErrorMessage = "Một số thuộc tính chỉ bao gồm các chữ số";
                return false;
            }
            return true;
        }

        #endregion
    }
}
