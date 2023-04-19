using Dapper;
using MISA.QLTS.Common.Constants;
using MISA.QLTS.Common.Entitites.DTO;
using MISA.QLTS.DL;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.BL
{
    public class BaseBL<T> : IBaseBL<T>
    {
        #region Field

        private IBaseDL<T> _baseDL;

        #endregion

        #region Constructor

        public BaseBL(IBaseDL<T> baseDL)
        {
            _baseDL = baseDL;
        }

        #endregion

        #region Method
        /// <summary>
        /// Lấy thông tin toàn bộ bản ghi
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        /// Author: NVThinh (16/11/2022)
        public IEnumerable<T> GetAllRecords()
        {
            return _baseDL.GetAllRecords();
        }

        /// <summary>
        /// Lấy thông tin bản ghi theo ID
        /// </summary>
        /// <param name="recordID">ID bản ghi muốn lấy</param>
        /// <returns>Thông tin bản ghi theo ID</returns>
        /// Author: NVThinh (16/11/2022)
        public T GetByID(Guid recordID)
        {
            return _baseDL.GetByID(recordID);
        }

        /// <summary>
        /// API lấy mã record mới
        /// </summary>
        /// <returns>Mã record mới</returns>
        /// Author: NVThinh 9/1/2023
        public string GetNextCode()
        {
            return _baseDL.GetNextCode();
        }

        /// <summary>
        /// Xóa nhiều bản ghi
        /// </summary>
        /// <param name="recordIDs">Danh sách ID các bản ghi cần xóa</param>
        /// <returns>Object chứa các thông tin trả về client</returns>
        /// <author>NVThinh 10/1/2023</author>
        public ServiceResponse DeleteMultipleFixedAsset(List<Guid> recordIDs)
        {
            return _baseDL.DeleteMultipleFixedAsset(recordIDs);
        }

        /// <summary>
        /// API Xóa 1 bản ghi theo ID
        /// </summary>
        /// <param name="recordID">ID bản ghi cần xóa</param>
        /// <returns>ID bản ghi được xóa</return
        /// <author>NVThinh 10/1/2023</author>
        public int DeleteByID(Guid recordID)
        {
            return _baseDL.DeleteByID(recordID);
        }

        /// <summary>
        /// Kiểm tra trùng mã
        /// </summary>
        /// <param name="recordCode">Mã bản ghi</param>
        /// <param name="recordID">ID bản ghi</param>
        /// <returns>Boolean</returns>
        /// Created by: NVThinh (21/11/2022)
        public bool CheckDuplicateCode(string recordCode, Guid recordID, string idType)
        {
           return _baseDL.CheckDuplicateCode(recordCode, recordID, idType);
        }



        #endregion
    }
}
