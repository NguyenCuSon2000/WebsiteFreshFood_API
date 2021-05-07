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
    public class SanPhamController : ControllerBase
    {
        private ISanPhamBussiness _spBusiness;
        private string _path;
        public SanPhamController(ISanPhamBussiness newBusiness, IConfiguration configuration)
        {
            _spBusiness = newBusiness;
            _path = configuration["AppSettings:PATH"];
        }

        // GET: api/<SanPhamController>
        [Route("get-all")]
        [HttpGet]
        public IEnumerable<SanPhamModel> GetDataAll()
        {
            return _spBusiness.GetDataAll();
        }

        [Route("create-product")]
        [HttpPost]
        public SanPhamModel CreateProduct([FromBody] SanPhamModel model)
        {
            _spBusiness.Create(model);
            return model;
        }

        [Route("update-product")]
        [HttpPost]
        public SanPhamModel UpdateProduct([FromBody] SanPhamModel model)
        {
            _spBusiness.Update(model);
            return model;
        }



        [Route("delete-product")]
        [HttpPost]
        public IActionResult DeleteProduct([FromBody] Dictionary<string, object> formData)
        {
            int masp = 0;
            if (formData.Keys.Contains("masp") && !string.IsNullOrEmpty(Convert.ToString(formData["masp"])))
            { masp = int.Parse(formData["masp"].ToString()); }
            _spBusiness.Delete(masp);
            return Ok();
        }


        [Route("search-product")]
        [HttpPost]
        public ResponseModel Search([FromBody] Dictionary<string, object> formData)
        {
            var response = new ResponseModel();
            try
            {
                var page = int.Parse(formData["page"].ToString());
                var pageSize = int.Parse(formData["pageSize"].ToString());
                string tensp = "";
                if (formData.Keys.Contains("tensp") && !string.IsNullOrEmpty(Convert.ToString(formData["tensp"]))) { tensp = Convert.ToString(formData["tensp"]); }
                string maloaisp = "";
                if (formData.Keys.Contains("maloaisp") && !string.IsNullOrEmpty(Convert.ToString(formData["maloaisp"]))) { maloaisp = Convert.ToString(formData["maloaisp"]); }
                long total = 0;
                var data = _spBusiness.Search(page, pageSize, out total, tensp, maloaisp);
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
