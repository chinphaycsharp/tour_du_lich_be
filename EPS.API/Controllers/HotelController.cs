using EPS.API.Commons;
using EPS.API.Helpers;
using EPS.Data.Entities;
using EPS.Service;
using EPS.Service.Dtos.Category;
using EPS.Service.Dtos.Hotel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EPS.API.Controllers
{
    [Produces("application/json")]
    [Route("api/hotel")]
    [Authorize]
    public class HotelController : BaseController
    {
        private HotelService _hotelService;

        public HotelController(HotelService hotelService)
        {
            _hotelService = hotelService;
        }

        [CustomAuthorize( PrivilegeList.ManageHotel)]
        [HttpGet]
        public async Task<IActionResult> GetListCategories([FromQuery] HotelPagingGridDto pagingModel)
        {
            return Ok(await _hotelService.GetCategories(pagingModel));
        }

        [CustomAuthorize(PrivilegeList.ManageHotel)]
        [HttpPost]
        public async Task<ApiResult<int>> CreateCategory([FromForm] HotelCreateDto dto)
        {
            dto.created_time = DateTime.Now;
            dto.status = 1;
            ApiResult<int> result = new ApiResult<int>();
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
        public async Task<ApiResult<int>> DeleteCategory(int id)
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
        public async Task<ApiResult<int>> UpdateCategory(int id, [FromForm] HotelUpdateDto dto)
        {
            ApiResult<int> result = new ApiResult<int>();
            dto.updated_time = DateTime.Now;
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
        public async Task<ApiResult<HotelDetailDto>> GetCategoryById(int id)
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
    }
}
