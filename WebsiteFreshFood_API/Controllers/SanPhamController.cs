﻿using BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebsiteFreshFood_API.Controllers
{
    [Authorize]
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

        public string SaveFileFromBase64String(string RelativePathFileName, string dataFromBase64String)
        {
            if (dataFromBase64String.Contains("base64,"))
            {
                dataFromBase64String = dataFromBase64String.Substring(dataFromBase64String.IndexOf("base64,", 0) + 7);
            }
            return WriteFileToAuthAccessFolder(RelativePathFileName, dataFromBase64String);
        }
        public string WriteFileToAuthAccessFolder(string RelativePathFileName, string base64StringData)
        {
            try
            {
                string result = "";
                string serverRootPathFolder = _path;
                string fullPathFile = $@"{serverRootPathFolder}\{RelativePathFileName}";
                string fullPathFolder = System.IO.Path.GetDirectoryName(fullPathFile);
                if (!Directory.Exists(fullPathFolder))
                    Directory.CreateDirectory(fullPathFolder);
                System.IO.File.WriteAllBytes(fullPathFile, Convert.FromBase64String(base64StringData));
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [Route("upload")]
        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            try
            {
                if (file.Length > 0)
                {
                    string filePath = $"upload/{file.FileName}";
                    var fullPath = CreatePathFile(filePath);
                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    return Ok(new { filePath });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Không tìm thây");
            }
        }

        [NonAction]
        private string CreatePathFile(string RelativePathFileName)
        {
            try
            {
                string serverRootPathFolder = _path;
                string fullPathFile = $@"{serverRootPathFolder}\{RelativePathFileName}";
                string fullPathFolder = System.IO.Path.GetDirectoryName(fullPathFile);
                if (!Directory.Exists(fullPathFolder))
                    Directory.CreateDirectory(fullPathFolder);
                return fullPathFile;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        // GET: api/<SanPhamController>
        [AllowAnonymous]
        [Route("get-all")]
        [HttpGet]
        public IEnumerable<SanPhamModel> GetDataAll()
        {
            return _spBusiness.GetDataAll();
        }

        [AllowAnonymous]
        [Route("get-product-new")]
        [HttpGet]
        public IEnumerable<SanPhamModel> GetProductNew()
        {
            return _spBusiness.GetProductNew();
        }

        [AllowAnonymous]
        [Route("get-by-maloai/{maloai}")]
        [HttpGet]
        public IEnumerable<SanPhamModel> GetDatabyMaLoai(string maloai)
        {
            return _spBusiness.GetDatabyMaLoai(maloai);
        }

        [AllowAnonymous]
        [Route("get-by-id/{masp}")]
        [HttpGet]
        public SanPhamModel GetDatabyID(string masp)
        {
            return _spBusiness.GetDatabyID(masp);
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

        
        [Route("delete-product/{id}")]
        [HttpDelete]
        public IActionResult DeleteProduct(int id)
        {
            _spBusiness.Delete(id);
            return Ok();
        }

        [AllowAnonymous]
        [Route("search")]
        [HttpPost]
        public ResponseModel search([FromBody] Dictionary<string, object> formData)
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
