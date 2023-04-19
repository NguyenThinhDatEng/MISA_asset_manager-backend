using MISA.QLTS.Common.Entitites.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.BL
{
    public interface IBaseBL<T>
    {
        /// <summary>
        /// Lấy thông tin toàn bộ bản ghi
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        /// Author: NVThinh (16/11/2022)
        public IEnumerable<T> GetAllRecords();

        /// <summary>
        /// Lấy thông tin bản ghi theo ID
        /// </summary>
        /// <param name="recordID">ID bản ghi muốn lấy</param>
        /// <returns>Thông tin bản ghi theo ID</returns>
        /// Author: NVThinh (16/11/2022)
        public T GetByID(Guid recordID);

        /// <summary>
        /// API lấy mã record mới
        /// </summary>
        /// <returns>Mã record mới</returns>
        /// Author: NVThinh 9/1/2023
        public string GetNextCode();

        /// <summary>
        /// Xóa nhiều bản ghi
        /// </summary>
        /// <param name="recordIDs">Danh sách ID các bản ghi cần xóa</param>
        /// <returns>Object chứa các thông tin trả về client</returns>
        /// <author>NVThinh 10/1/2023</author>
        public ServiceResponse DeleteMultipleFixedAsset(List<Guid> recordIDs);

        /// <summary>
        /// API Xóa 1 bản ghi theo ID
        /// </summary>
        /// <param name="recordID">ID bản ghi cần xóa</param>
        /// <returns>ID bản ghi được xóa</return
        /// <author>NVThinh 10/1/2023</author>
        public int DeleteByID(Guid recordID);

        /// <summary>
        /// Kiểm tra trùng mã
        /// </summary>
        /// <param name="recordCode">Mã bản ghi</param>
        /// <param name="recordID">ID bản ghi</param>
        /// <returns>Boolean</returns>
        /// Author: NVThinh (21/11/2022)
        public bool CheckDuplicateCode(string recordCode, Guid recordID, string idType);
    }
}
