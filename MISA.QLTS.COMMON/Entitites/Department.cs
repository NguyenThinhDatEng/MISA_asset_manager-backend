namespace MISA.QLTS.Common.Entitites
{
    /// <summary>
    /// Thông tin bộ phận sử dụng
    /// </summary>
    public class Department
    {
        #region Property
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

        /// <summary>
        /// Ngay tao thong tin
        /// </summary>
        public DateTime created_date { get; set; }

        /// <summary>
        /// Nguoi tao thong tin
        /// </summary>
        public string created_by { get; set; }

        /// <summary>
        /// Ngay chinh sua thong tin
        /// </summary>
        public DateTime modified_date { get; set; }

        /// <summary>
        /// Nguoi chinh sua thong tin
        /// </summary>
        public string modified_by { get; set; }
        #endregion
    }
}
