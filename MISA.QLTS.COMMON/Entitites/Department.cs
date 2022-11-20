namespace MISA.QLTS.Common.Entitites
{
    /// <summary>
    /// Thông tin bộ phận sử dụng
    /// </summary>
    public class Department : BaseEntity
    {
        /// <summary>
        /// ID phong ban
        /// </summary>
        public Guid department_id { get; set; }

        /// <summary>
        /// Mã phòng ban
        /// </summary>
        public string department_code { get; set; }

        /// <summary>
        /// Ten phong ban
        /// </summary>
        public string department_name { get; set; }
    }
}
