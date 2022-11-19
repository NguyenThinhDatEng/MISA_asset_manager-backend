﻿using MISA.QLTS.Common.Entitites;
using MISA.QLTS.Common.Entitites.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.DL
{
    public interface IFixedAssetDL
    {
        /// <summary>
        /// Lấy thông tin tất cả tài sản
        /// </summary>
        /// <returns>Danh sách tài sản</returns>
        /// Create by: NVThinh (16/11/2022)
        public IEnumerable<dynamic> GetAllFixedAsset();

        /// <summary>
        /// Lấy thông tin 1 tài sản theo ID
        /// </summary>
        /// <param name="fixedAssetID">ID tài sản muốn lấy</param>
        /// <returns>Thông tin 1 nhân viên theo ID</returns>
        /// Create by: NVThinh (16/11/2022)
        public FixedAsset GetFixedAssetByID(Guid fixedAssetID);

        /// <summary>
        /// API lấy mã tài sản cố định mới
        /// </summary>
        /// <returns>Mã tài sản cố định mới</returns>
        /// created by: NVThinh 16/11/2022
        public string GetMaxFixedAssetCode();

        /// <summary>
        /// API Tạo mới tài sản cố định
        /// </summary>
        /// <param name="fixedAsset">Đối tượng tài sản cố định</param>
        /// <returns>ID tài sản cố định được thêm</returns>
        /// Created by: NVThinh (11/11/2022)
        public int InsertFixedAsset(FixedAsset fixedAsset);

        /// <summary>
        /// API cập nhật thông tin tài sản
        /// </summary>
        /// <param name="fixedAssetID">ID tài sản được cập nhật</param>
        /// <param name="fixedAsset">Đối tượng tài sản cố định</param>
        /// <returns>ID bản ghi được cập nhật</returns>
        /// Created by: NVThinh (11/11/2022)
        public int UpdateFixedAsset(Guid fixedAssetID, FixedAsset fixedAsset);

        /// <summary>
        /// API Xóa 01 tài sản
        /// </summary>
        /// <param name="fixedAssetID">ID tài sản cần xóa</param>
        /// <returns>ID tài sản được xóa</returns>
        /// Created by: NVThinh (11/11/2022)
        public int DeleteFixedAsset(Guid fixedAssetID);

        /// <summary>
        /// Xóa nhiều bản ghi
        /// </summary>
        /// <param name="listFixedAssetID">Danh sách ID các tài sản cần xóa</param>
        /// <returns>Số lượng tài sản được xóa</returns>
        /// Created by: NVThinh (11/11/2022)
        public bool DeleteMultipleFixedAsset(ListFixedAssetID fixedAssetIDs);
    }
}
