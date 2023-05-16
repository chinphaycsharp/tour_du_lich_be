using EPS.API.Commons;
using EPS.API.Helpers;
using EPS.Data.Entities;
using EPS.Service;
using EPS.Service.Dtos.Common.RegisterTour;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPS.API.Controllers
{
    [Produces("application/json")]
    [Route("api/registertour")]
    [Authorize]
    public class RegisterTourController : BaseController
    {
        private RegisterTourService _registerTourService;

        public RegisterTourController(RegisterTourService registerTourService)
        {
            _registerTourService = registerTourService;
        }

        [CustomAuthorize(PrivilegeList.ManageTour)]
        [HttpGet]
        public async Task<IActionResult> GetListTours([FromQuery] RegisterTourGridPagingDto pagingModel)
        {
            return Ok(await _registerTourService.GetRegisterTours(pagingModel));
        }

        [CustomAuthorize(PrivilegeList.ManageTour)]
        [HttpPost]
        public async Task<ApiResult<int>> CreateTour([FromForm] RegisterTourCreateDto dto)
        {
            ApiResult<int> result = new ApiResult<int>();
            dto.created_time = DateTime.Now;
            var id = await _registerTourService.CreateRegisterTour(dto);
            if (id == 0)
            {
                result.ResultObj = id;
                result.Message = "Thêm mới thành công !";
                result.statusCode = 200;
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

        [CustomAuthorize(PrivilegeList.ManageTour)]
        [HttpDelete("{id}")]
        public async Task<ApiResult<int>> DeleteTour(int id)
        {
            ApiResult<int> result = new ApiResult<int>();

            var checkdetails = await _registerTourService.DeleteRegisterTour(id);
            if (checkdetails == 1)
            {
                result.ResultObj = checkdetails;
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

        [CustomAuthorize(PrivilegeList.ManageTour)]
        [HttpPut("{id}")]
        public async Task<ApiResult<int>> UpdateTour(int id, [FromForm] RegisterTourUpdateDto dto)
        {
            ApiResult<int> result = new ApiResult<int>();
            var check = await _registerTourService.UpdateRegisterTour(id, dto);
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

        [CustomAuthorize(PrivilegeList.ManageTour)]
        [HttpGet("getbyid/{id}")]
        public async Task<ApiResult<RegisterTourDetailDto>> GetTourById(int id)
        {
            ApiResult<RegisterTourDetailDto> result = new ApiResult<RegisterTourDetailDto>();
            var dto = await _registerTourService.GetRegisterTourById(id);
            if (dto != null)
            {
                result.ResultObj = dto;
                result.Message = "";
                result.statusCode = 200;
                return result;
            }
            else
            {
                result.ResultObj = new RegisterTourDetailDto();
                result.Message = "Đã có lỗi xẩy ra với hệ thống, vui lòng thử lại !";
                result.statusCode = 500;
                return result;
            }
        }
    }
}
