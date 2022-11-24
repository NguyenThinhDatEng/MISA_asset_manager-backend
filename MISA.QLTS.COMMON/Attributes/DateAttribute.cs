using MISA.QLTS.Common.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Common.Attributes
{
    public class DateAttribute : ValidationAttribute
    {
        #region Field

        /// <summary>
        ///  Trường thông tin
        /// </summary>
        public string Field;
        #endregion

        #region Constructor

        public DateAttribute() { }

        public DateAttribute(string field)
        {
            Field = field;
        }
        #endregion

        #region Method

        /// <summary>
        /// Kiểm tra ngày được truyền vào có lớn hơn ngày hiện tại không
        /// </summary>
        /// <param name="value">ngày truyền vào</param>
        /// <returns>true nếu ngày truyền vào nhỏ hơn hoặc bằng ngày hiện tại</returns>
        public override bool IsValid(object? value)
        {
            if (value != null && DateTime.Compare(Convert.ToDateTime(value), DateTime.Now) > 0)
            {
                ErrorMessage = Field + " không được lớn hơn ngày hiện tại";
                return false;
            }

            return true;
        } 
        #endregion
    }
}
