using MISA.QLTS.Common.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MISA.QLTS.Common.Entitites
{
    public class Budget : BaseEntity
    {
        #region Property

        /// <summary>
        /// ID ngân sách
        /// </summary>
        [Key]
        public Guid? budget_id { get; set; }

        /// <summary>
        /// Tên ngân sách
        /// </summary>
        [RequiredField(field: "Tên ngân sách")]
        public string? budget_name { get; set; }

        #endregion
    }
}
