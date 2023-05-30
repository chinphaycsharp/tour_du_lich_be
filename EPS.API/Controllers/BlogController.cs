using EPS.API.Commons;
using EPS.API.Helpers;
using EPS.API.Models.Image;
using EPS.Data.Entities;
using EPS.Service;
using EPS.Service.Dtos.Blog;
using EPS.Service.Dtos.ImageBlog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace EPS.API.Controllers
{
    [Produces("application/json")]
    [Route("api/blogs")]
    [Authorize]
    public class BlogController : BaseController
    {
        private BlogService _blogService;
        private ImageService _imageService;
        private IWebHostEnvironment _webHostEnvironment;
        public BlogController(BlogService blogService, IWebHostEnvironment webHostEnvironment, ImageService imageService)
        {
            _blogService = blogService;
            _webHostEnvironment = webHostEnvironment;
            _imageService = imageService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBlogs([FromQuery] BlogPagingGridDto pagingModel)
        {
            return Ok(await _blogService.GetBlog(pagingModel));
        }

        [CustomAuthorize(PrivilegeList.ManageBlog)]
        [HttpPost]
        public async Task<ApiResult<int>> CreateBlog([FromForm] BlogCreateDto dto)
        {
            dto.created_time = DateTime.Now;
            ApiResult<int> result = new ApiResult<int>();
            if (Request.Form.Files.Count < 0)
            {
                result.ResultObj = default;
                result.Message = "Ảnh không được để trống !";
                result.statusCode = 201;
                return result;
            }
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "common/blog", Request.Form.Files[0].FileName);
            using (var fileSteam = new FileStream(path, FileMode.Create))
            {
                await Request.Form.Files[0].CopyToAsync(fileSteam);
            }
            dto.img_src = Request.Form.Files[0].FileName;
            dto.created_time = DateTime.Now;
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
            if (Request.Form.Files.Count < 0)
            {
                result.ResultObj = default;
                result.Message = "Ảnh không được để trống !";
                result.statusCode = 201;
                return result;
            }
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "common/blog", Request.Form.Files[0].FileName);
            using (var fileSteam = new FileStream(path, FileMode.Create))
            {
                await Request.Form.Files[0].CopyToAsync(fileSteam);
            }
            dto.img_src = Request.Form.Files[0].FileName;
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
        public async Task<ApiResult<BlogDetailDto>> GetBlogById(int id)
        {
            ApiResult<BlogDetailDto> result = new ApiResult<BlogDetailDto>();
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

        #region image_tours
        [CustomAuthorize(PrivilegeList.ManageImage)]
        [HttpGet("imageblogs")]
        public async Task<IActionResult> GetListImageBlogs([FromQuery] ImageGridPagingDto pagingModel)
        {
            return Ok(await _imageService.GetImageBlogs(pagingModel));
        }

        [CustomAuthorize(PrivilegeList.ManageTour)]
        [HttpPost("imageblogs")]
        public async Task<ApiResult<bool>> CreateImageBlog([FromForm] ImageViewModel dto)
        {
            ApiResult<bool> result = new ApiResult<bool>();
            bool isSuccess = true;
            if (Request.Form.Files.Count > 0)
            {
                foreach (var item in Request.Form.Files)
                {
                    dto.img_src = item.FileName;
                    try
                    {
                        var path = Path.Combine(_webHostEnvironment.WebRootPath, "common/blog", item.FileName);
                        if (dto.type_id == 0)
                        {
                            int lastId = await _blogService.GetLastBlogRecord();
                            dto.type_id = lastId;
                        }
                        ImageCreateDto imagetour = new ImageCreateDto(dto.type_id, item.FileName, dto.type);
                        int resultCreate = 0;

                        resultCreate = await _imageService.CreateImageBlogs(imagetour);
                        if (resultCreate == 0)
                        {
                            using (var fileSteam = new FileStream(path, FileMode.Create))
                            {
                                await item.CopyToAsync(fileSteam);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        isSuccess = false;
                    }
                }
            }

            if (isSuccess == true)
            {
                result.ResultObj = isSuccess;
                result.Message = "Tạo mới thành công !";
                result.statusCode = 201;
                return result;
            }
            else
            {
                result.ResultObj = isSuccess;
                result.Message = "Đã có lỗi xẩy ra với hệ thống, vui lòng thử lại !";
                result.statusCode = 500;
                return result;
            }
        }
        #endregion
    }
}
