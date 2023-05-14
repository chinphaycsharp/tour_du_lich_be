﻿using EPS.API.Commons;
using EPS.API.Helpers;
using EPS.Data.Entities;
using EPS.Service;
using EPS.Service.Dtos.Blog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EPS.API.Controllers
{
    [Produces("application/json")]
    [Route("api/blog")]
    [Authorize]
    public class BlogController : BaseController
    {
        private BlogService _blogService;

        public BlogController(BlogService blogService)
        {
            _blogService = blogService;
        }

        [HttpGet]
        public async Task<IActionResult> GetListCategories([FromQuery] BlogPagingGridDto pagingModel)
        {
            return Ok(await _blogService.GetBlog(pagingModel));
        }

        [CustomAuthorize(PrivilegeList.ManageBlog)]
        [HttpPost]
        public async Task<ApiResult<int>> CreateBlog([FromForm] BlogCreateDto dto)
        {
            dto.created_time = DateTime.Now;
            ApiResult<int> result = new ApiResult<int>();
            var id = await _blogService.CreateBlog(dto);
            if (id == 0)
            {
                result.ResultObj = id;
                result.Message = "Tạo mới thành công !";
                result.statusCode = 201;
                return result;
            }
            else
            {
                result.ResultObj = id;
                result.Message = "Đã có lỗi xẩy ra với hệ thống, vui lòng thử lại !";
                result.statusCode = 500;
                return result;
            }
        }

        [CustomAuthorize(PrivilegeList.ManageBlog)]
        [HttpDelete("{id}")]
        public async Task<ApiResult<int>> DeleteBlog(int id)
        {
            ApiResult<int> result = new ApiResult<int>();
            var checkDelete = await _blogService.DeleteBlog(id);
            if (checkDelete == 1)
            {
                result.ResultObj = checkDelete;
                result.Message = "Xóa bản ghi thành công !";
                result.statusCode = 200;
                return result;
            }
            else
            {
                result.ResultObj = default;
                result.Message = "Đã có lỗi xẩy ra với hệ thống, vui lòng thử lại !";
                result.statusCode = 500;
                return result;
            }
        }

        [CustomAuthorize(PrivilegeList.ManageBlog)]
        [HttpPut("{id}")]
        public async Task<ApiResult<int>> UpdateBlog(int id, [FromForm] BlogUpdateDto dto)
        {
            ApiResult<int> result = new ApiResult<int>();
            dto.updated_time = DateTime.Now;
            var check = await _blogService.UpdateBlog(id, dto);
            if (check == 1)
            {
                result.ResultObj = check;
                result.Message = "Cập nhập thành công !";
                result.statusCode = 200;
                return result;
            }
            else
            {
                result.ResultObj = check;
                result.Message = "Đã có lỗi xẩy ra với hệ thống, vui lòng thử lại !";
                result.statusCode = 500;
                return result;
            }
        }

        [CustomAuthorize(PrivilegeList.ManageBlog)]
        [HttpGet("getbyid/{id}")]
        public async Task<ApiResult<BlogDetailRegexDto>> GetBlogById(int id)
        {
            ApiResult<BlogDetailRegexDto> result = new ApiResult<BlogDetailRegexDto>();
            var dto = await _blogService.GetBlogById(id);
            if (dto != null)
            {
                result.ResultObj = dto;
                result.Message = "";
                result.statusCode = 200;
                return result;
            }
            else
            {
                result.ResultObj = dto;
                result.Message = "Đã có lỗi xẩy ra với hệ thống, vui lòng thử lại !";
                result.statusCode = 500;
                return result;
            }
        }
    }
}
