using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    public partial interface IHoaDonBussiness
    {
        List<HoaDonModel> GetDataAll();
        bool Create(HoaDonModel model);
        bool Update(HoaDonModel model);
        HoaDonModel GetDatabyID(string id);
        bool Delete(string id);
        List<HoaDonModel> Search(int pageIndex, int pageSize, out long total, string hoten, string diachi);

    }
}
