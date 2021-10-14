using BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WebsiteFreshFood_API.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class HoaDonController : ControllerBase
    {
        private IHoaDonBussiness _HoaDonBussiness;
        public HoaDonController(IHoaDonBussiness HoaDonBussiness)
        {
            _HoaDonBussiness = HoaDonBussiness;
        }

        // GET: api/<HoaDonController>
        [Route("get-all")]
        [HttpGet]
        public IEnumerable<HoaDonModel> GetDataAll()
        {
            return _HoaDonBussiness.GetDataAll();
        }

        [Route("create-hoadon")]
        [HttpPost]
        public HoaDonModel CreateItem([FromBody] HoaDonModel model)
        {
            model.ma_hoa_don = Guid.NewGuid().ToString();
            if (model.listjson_chitiet != null)
            {
                foreach (var item in model.listjson_chitiet)
                {
                    item.ma_hoa_don = model.ma_hoa_don;
                    item.ma_chi_tiet = Guid.NewGuid().ToString();
                }

            }
            _HoaDonBussiness.Create(model);
            return model;
        }

        [Route("update-hoadon")]
        [HttpPost]
        public HoaDonModel UpdateHoaDon([FromBody] HoaDonModel model)
        {
            if (model.listjson_chitiet != null)
            {
                foreach (var item in model.listjson_chitiet)
                    if (item.status == 1)
                    {
                        item.ma_chi_tiet = Guid.NewGuid().ToString();
                    }
            }
            _HoaDonBussiness.Update(model);
            return model;
        }

        [Route("get-by-id/{id}")]
        [HttpGet]
        public HoaDonModel GetDatabyID(string id)
        {
            return _HoaDonBussiness.GetDatabyID(id);
        }

        [Route("delete-hoadon/{id}")]
        [HttpDelete]
        public IActionResult DeleteHoaDon(string id)
        {
            _HoaDonBussiness.Delete(id);
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
                string hoten = "";
                if (formData.Keys.Contains("hoten") && !string.IsNullOrEmpty(Convert.ToString(formData["hoten"]))) { hoten = Convert.ToString(formData["hoten"]); }
                string diachi = "";
                if (formData.Keys.Contains("diachi") && !string.IsNullOrEmpty(Convert.ToString(formData["diachi"]))) { diachi = Convert.ToString(formData["diachi"]); }
                long total = 0;
                var data = _HoaDonBussiness.Search(page, pageSize, out total, hoten, diachi);
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
