using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    public partial interface ILoaiSanPhamBussiness
    {
        List<LoaiSanPhamModel> GetDataAll();
        LoaiSanPhamModel GetDatabyID(string maloaisp);
        bool Create(LoaiSanPhamModel model);
        bool Update(LoaiSanPhamModel model);
        bool Delete(string maloaisp);
        List<LoaiSanPhamModel> Search(int pageIndex, int pageSize, out long total,string maloaisp, string tenloai);
    }
}
