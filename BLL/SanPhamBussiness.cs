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
    public class SanPhamBussiness : ISanPhamBussiness
    {
        private ISanPhamRepository _res;
        private string Secret;
        public SanPhamBussiness(ISanPhamRepository res, IConfiguration configuration)
        {
            Secret = configuration["AppSettings:Secret"];
            _res = res;
        }
        public List<SanPhamModel> GetDataAll()
        {
            return _res.GetDataAll();
        }
      

        public bool Create(SanPhamModel model)
        {
            return _res.Create(model);
        }
        public bool Update(SanPhamModel model)
        {
            return _res.Update(model);
        }
        public bool Delete(int masp)
        {
            return _res.Delete(masp);
        }
        public List<SanPhamModel> Search(int pageIndex, int pageSize, out long total, string tensp, string maloaisp)
        {
            return _res.Search(pageIndex, pageSize, out total, tensp, maloaisp);
        }
    }
}
