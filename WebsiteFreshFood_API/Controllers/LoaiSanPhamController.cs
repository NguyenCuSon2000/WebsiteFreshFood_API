using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Model;
using BLL;
using Microsoft.Extensions.Configuration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebsiteFreshFood_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoaiSanPhamController : ControllerBase
    {
        private ILoaiSanPhamBussiness _loaispBusiness;
        private string _path;
        public LoaiSanPhamController(ILoaiSanPhamBussiness newBusiness, IConfiguration configuration)
        {
            _loaispBusiness = newBusiness;
            _path = configuration["AppSettings:PATH"];
        }

        // GET: api/<LoaiSanPhamController>
        [Route("get-all")]
        [HttpGet]
        public IEnumerable<LoaiSanPhamModel> GetDataAll()
        {
            return _loaispBusiness.GetDataAll();
        }

        [Route("create-category")]
        [HttpPost]
        public LoaiSanPhamModel CreateLoaiSP([FromBody] LoaiSanPhamModel model)
        {
            _loaispBusiness.Create(model);
            return model;
        }

        [Route("update-category")]
        [HttpPost]
        public LoaiSanPhamModel UpdateLoai([FromBody] LoaiSanPhamModel model)
        {
            _loaispBusiness.Update(model);
            return model;
        }



        [Route("delete-category")]
        [HttpPost]
        public IActionResult DeleteCategory([FromBody] Dictionary<string, object> formData)
        {
            string maloaisp = "";
            if (formData.Keys.Contains("maloaisp") && !string.IsNullOrEmpty(Convert.ToString(formData["maloaisp"]))) { maloaisp = Convert.ToString(formData["maloaisp"]); }
            _loaispBusiness.Delete(maloaisp);
            return Ok();
        }


        [Route("search")]
        [HttpPost]
        public ResponseModel Search([FromBody] Dictionary<string, object> formData)
        {
            var response = new ResponseModel();
            try
            {
                var page = int.Parse(formData["page"].ToString());
                var pageSize = int.Parse(formData["pageSize"].ToString());
                string tenloai = "";
                if (formData.Keys.Contains("tenloai") && !string.IsNullOrEmpty(Convert.ToString(formData["tenloai"]))) { tenloai = Convert.ToString(formData["tenloai"]); }
                long total = 0;
                var data = _loaispBusiness.Search(page, pageSize, out total, tenloai);
                response.TotalItems = total;
                response.Data = data;
                response.Page = page;
                response.PageSize = pageSize;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return response;
        }
    }
}
