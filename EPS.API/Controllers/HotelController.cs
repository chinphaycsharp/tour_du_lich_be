using EPS.API.Commons;
using EPS.API.Helpers;
using EPS.API.Models.Image;
using EPS.Data.Entities;
using EPS.Service;
using EPS.Service.Dtos.Category;
using EPS.Service.Dtos.Hotel;
using EPS.Service.Dtos.ImageBlog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace EPS.API.Controllers
{
    [Produces("application/json")]
    [Route("api/hotel")]
    [Authorize]
    public class HotelController : BaseController
    {
        private HotelService _hotelService;
        private ImageService _imageTourService;
        private IWebHostEnvironment _webHostEnvironment;

        public HotelController(HotelService hotelService, ImageService imageTourService, IWebHostEnvironment webHostEnvironment)
        {
            _hotelService = hotelService;
            _imageTourService = imageTourService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> GetHotels([FromQuery] HotelPagingGridDto pagingModel)
        {
            return Ok(await _hotelService.GetHotels(pagingModel));
        }

        [CustomAuthorize(PrivilegeList.ManageHotel)]
        [HttpPost]
        public async Task<ApiResult<int>> CreateHotels([FromForm] HotelCreateDto dto)
        {
            ApiResult<int> result = new ApiResult<int>();
            if (Request.Form.Files.Count < 0)
            {
                result.ResultObj = default;
                result.Message = "Ảnh không được để trống !";
                result.statusCode = 201;
                return result;
            }
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "common", Request.Form.Files[0].FileName);
            using (var fileSteam = new FileStream(path, FileMode.Create))
            {
                await Request.Form.Files[0].CopyToAsync(fileSteam);
            }
            dto.background_image = Request.Form.Files[0].FileName;
            dto.created_time = DateTime.Now;
            dto.status = 1;

            var id = await _hotelService.CreateHotel(dto);
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

        [CustomAuthorize(PrivilegeList.ManageHotel)]
        [HttpDelete("{id}")]
        public async Task<ApiResult<int>> DeleteHotel(int id)
        {
            ApiResult<int> result = new ApiResult<int>();
            var checkToursDelete = await _hotelService.DeleteHotel(id);
            if (checkToursDelete == 1)
            {
                result.ResultObj = checkToursDelete;
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

        [CustomAuthorize(PrivilegeList.ManageHotel)]
        [HttpPut("{id}")]
        public async Task<ApiResult<int>> UpdateHotel(int id, [FromForm] HotelUpdateDto dto)
        {
            ApiResult<int> result = new ApiResult<int>();
            if (Request.Form.Files.Count < 0)
            {
                result.ResultObj = default;
                result.Message = "Ảnh không được để trống !";
                result.statusCode = 201;
                return result;
            }
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "common", Request.Form.Files[0].FileName);
            using (var fileSteam = new FileStream(path, FileMode.Create))
            {
                await Request.Form.Files[0].CopyToAsync(fileSteam);
            }
            dto.background_image = Request.Form.Files[0].FileName;
            dto.updated_time = DateTime.Now;
            dto.status = 1;
            var check = await _hotelService.UpdateHotel(id, dto);
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

        [CustomAuthorize(PrivilegeList.ManageHotel)]
        [HttpGet("getbyid/{id}")]
        public async Task<ApiResult<HotelDetailDto>> GetHotelById(int id)
        {
            ApiResult<HotelDetailDto> result = new ApiResult<HotelDetailDto>();
            var dto = await _hotelService.GetHotelById(id);
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

        [CustomAuthorize(PrivilegeList.ManageHotel)]
        [HttpGet("getbycategoryid/{id}")]
        public async Task<ApiResult<List<HotelGridDto>>> GetHotelByCategoryId(int id)
        {
            ApiResult<List<HotelGridDto>> result = new ApiResult<List<HotelGridDto>>();
            var dto = await _hotelService.GetHotelByCategoryId(id);
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
        [HttpGet("imagehotels")]
        public async Task<IActionResult> GetListImageTours([FromQuery] ImageGridPagingDto pagingModel)
        {
            return Ok(await _imageTourService.GetImageTours(pagingModel));
        }

        [CustomAuthorize(PrivilegeList.ManageTour)]
        [HttpPost("imagehotels")]
        public async Task<ApiResult<bool>> CreateImageTour([FromForm] ImageViewModel dto)
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
                        var path = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", item.FileName);
                        if (dto.type_id == 0)
                        {
                            int lastId = await _hotelService.GetLastHotelRecord();
                            dto.type_id = lastId;
                        }
                        ImageCreateDto imagetour = new ImageCreateDto(dto.type_id, item.FileName, dto.type);
                        var id = await _imageTourService.CreateImageTours(imagetour);
                        using (var fileSteam = new FileStream(path, FileMode.Create))
                        {
                            await item.CopyToAsync(fileSteam);
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
