using EPS.API.Commons;
using EPS.API.Helpers;
using EPS.Data.Entities;
using EPS.Service.Dtos.Tour;
using EPS.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using EPS.Service.Dtos.Category;

namespace EPS.API.Controllers
{
    [Produces("application/json")]
    [Route("api/category")]
    [Authorize]
    public class CategoryController : Controller
    {
        private CategoryService _categoryService;

        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [CustomAuthorize(PrivilegeList.ViewCategory, PrivilegeList.ManageCategory)]
        [HttpGet]
        public async Task<IActionResult> GetListCategories([FromQuery] CategoryGridPagingDto pagingModel)
        {
            return Ok(await _categoryService.GetCategories(pagingModel));
        }

        [CustomAuthorize(PrivilegeList.ManageCategory)]
        [HttpPost]
        public async Task<ApiResult<int>> CreateCategory([FromForm] CategoryCreateDto dto)
        {
            dto.created_time = DateTime.Now;
            ApiResult<int> result = new ApiResult<int>();

            var id = await _categoryService.CreateCategory(dto);
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

        [CustomAuthorize(PrivilegeList.ManageCategory)]
        [HttpDelete("{id}")]
        public async Task<ApiResult<int>> DeleteCategory(int id)
        {
            ApiResult<int> result = new ApiResult<int>();
            var check = await _categoryService.DeleteCategory(id);
            if (check == 1)
            {
                result.ResultObj = check;
                result.Message = "Xóa bản ghi thành công !";
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

        [CustomAuthorize(PrivilegeList.ManageCategory)]
        [HttpPut("{id}")]
        public async Task<ApiResult<int>> UpdateCategory(int id, [FromForm] CategoryUpdateDto dto)
        {
            ApiResult<int> result = new ApiResult<int>();
            dto.updated_time = DateTime.Now;
            var check = await _categoryService.UpdateCategory(id, dto);
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

        [CustomAuthorize(PrivilegeList.ManageCategory)]
        [HttpGet("getbyid/{id}")]
        public async Task<ApiResult<CategoryDetailDto>> GetCategoryById(int id)
        {
            ApiResult<CategoryDetailDto> result = new ApiResult<CategoryDetailDto>();
            var dto = await _categoryService.GetCategoryById(id);
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
