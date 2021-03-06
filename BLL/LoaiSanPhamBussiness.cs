using DAL;
using Microsoft.IdentityModel.Tokens;
using Model;
using System;
using Helper;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;


namespace BLL
{
    public class LoaiSanPhamBussiness : ILoaiSanPhamBussiness
    {
        private ILoaiSanPhamRepository _res;
        private string Secret;
        public LoaiSanPhamBussiness(ILoaiSanPhamRepository res, IConfiguration configuration)
        {
            Secret = configuration["AppSettings:Secret"];
            _res = res;
        }
        public List<LoaiSanPhamModel> GetDataAll()
        {
            return _res.GetDataAll();
        }

        public LoaiSanPhamModel GetDatabyID(string maloaisp)
        {
            return _res.GetDatabyID(maloaisp);
        }
        public bool Create(LoaiSanPhamModel model)
        {
            return _res.Create(model);
        }
        public bool Update(LoaiSanPhamModel model)
        {
            return _res.Update(model);
        }
        public bool Delete(string maloaisp)
        {
            return _res.Delete(maloaisp);
        }
        public List<LoaiSanPhamModel> Search(int pageIndex, int pageSize, out long total, string maloaisp, string tenloai)
        {
            return _res.Search(pageIndex, pageSize, out total, maloaisp, tenloai);
        }
    }
}
