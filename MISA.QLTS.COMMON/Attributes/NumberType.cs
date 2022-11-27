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

        /// <summary>
        /// Kiểm tra dữ liệu kiểu số có chỉ gồm các ký tự số hay không
        /// </summary>
        /// <param name="value">là 1 object có 1 thuộc tính</param>
        /// <returns>true nếu giá trị toàn ký tự số</returns>
        /// <author>NVThinh 27/11/2022</author>
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
