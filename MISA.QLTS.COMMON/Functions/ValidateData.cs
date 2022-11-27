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
        /// <param name="pattern">Mẫu regex</param>
        /// <param name="value">giá trị cần kiểm tra (kiểu string)</param>
        /// <returns>true nếu giá trị có định dạng phù hợp</returns>
        /// <author>NVThinh 27/11/2022</author>
        public static bool Regex(string pattern, string value)
        {
            Regex regex = new Regex(pattern);
            return regex.IsMatch(value);
        }

        /// <summary>
        /// Kiểm tra kiểu số
        /// </summary>
        /// <param name="value">giá trị cần kiểm tra (kiểu string)</param>
        /// <returns>true nếu giá trị có định dạng phù hợp</returns>
        /// <author>NVThinh 27/11/2022</author>
        public static bool IsNumber(string value)
        {
            string pattern = @"^[0-9]+$";
            return Regex(pattern, value);
        }

        /// <summary>
        /// Kiểm tra định dạng mã tài sản cố định
        /// </summary>
        /// <param name="value">giá trị cần kiểm tra (kiểu string)</param>
        /// <returns>true nếu giá trị có định dạng phù hợp</returns>
        /// <author>NVThinh 27/11/2022</author>
        public static bool IsFixedAssetCode(string value)
        {
            // Nếu mã có dạng TS00000 trả về false
            if (int.Parse(value[2..]) == 0) return false;
            // Kiểm tra pattern
            string pattern = @"(TS)(\d{5})";
            return Regex(pattern, value);
        }

        /// <summary>
        /// Kiểm tra định dạng mã bộ phận sử dụng
        /// </summary>
        /// <param name="value">giá trị cần kiểm tra (kiểu string)</param>
        /// <returns>true nếu giá trị có định dạng phù hợp</returns>
        /// <author>NVThinh 27/11/2022</author>
        public static bool IsDepartmentCode(string value)
        {
            string pattern = @"(D)(\d{3})";
            return Regex(pattern, value);
        }

        /// <summary>
        /// Kiểm tra định dạng mã loại tài sản cố định
        /// </summary>
        /// <param name="value">giá trị cần kiểm tra (kiểu string)</param>
        /// <returns>true nếu giá trị có định dạng phù hợp</returns>
        /// <author>NVThinh 27/11/2022</author>
        public static bool IsFixedAssetCategoryCode(string value)
        {
            string pattern = @"(AC)(\d{3})";
            return Regex(pattern, value);
        }
        #endregion
    }
}
