using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MISA.QLTS.Common.Functions
{
    public class ValidateData
    {
        #region Method

        /// <summary>
        /// Định dạng dữ liệu theo mẫu
        /// </summary>
        /// <param name="pattern">Định dạng</param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool Regex(string pattern, string value)
        {
            Regex regex = new Regex(pattern);
            return regex.IsMatch(value);
        }

        public static bool IsNumber(string value)
        {
            string pattern = @"^[0-9]+$";
            return Regex(pattern, value);
        }

        public static bool isFixedAssetCode(string value)
        {
            string pattern = @"(TS)(\d{5})";
            return Regex(pattern, value);
        }

        public static bool isDepartmentCode(string value)
        {
            string pattern = @"(D)(\d{3})";
            return Regex(pattern, value);
        }

        public static bool isFixedAssetCategoryCode(string value)
        {
            string pattern = @"(AC)(\d{3})";
            return Regex(pattern, value);
        }
        #endregion
    }
}
