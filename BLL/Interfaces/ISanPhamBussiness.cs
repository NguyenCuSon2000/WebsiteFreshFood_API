using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    public partial interface ISanPhamBussiness
    {
        List<SanPhamModel> GetDataAll();
        bool Create(SanPhamModel model);
        bool Update(SanPhamModel model);
        bool Delete(int masp);
        List<SanPhamModel> Search(int pageIndex, int pageSize, out long total, string tensp, string maloaisp);
    }
}
