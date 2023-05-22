using EPS.API.Commons;
using EPS.API.Helpers;
using EPS.Data.Entities;
using EPS.Service;
using EPS.Service.Dtos.Contact;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EPS.API.Controllers
{
    [Produces("application/json")]
    [Route("api/contact")]
    [Authorize]
    public class ContactController : BaseController
    {
        private ContactService _contactService;

        public ContactController(ContactService contactService)
        {
            _contactService = contactService;
        }

        [CustomAuthorize(PrivilegeList.ManageContact)]
        [HttpGet]
        public async Task<IActionResult> GetListContacts([FromQuery] ContactPagingGridDto pagingModel)
        {
            return Ok(await _contactService.GetContacts(pagingModel));
        }


        [CustomAuthorize(PrivilegeList.ManageContact)]
        [HttpPost]
        public async Task<ApiResult<int>> CreateContact([FromForm] ContactCreateDto dto)
        {
            dto.created_time = DateTime.Now;
            dto.status = 1;
            ApiResult<int> result = new ApiResult<int>();
            var id = await _contactService.CreateContact(dto);
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
            
        [CustomAuthorize(PrivilegeList.ManageContact)]
        [HttpDelete("{id}")]
        public async Task<ApiResult<int>> DeleteContact(int id)
        {
            ApiResult<int> result = new ApiResult<int>();
            var checkToursDelete = await _contactService.DeleteContact(id);
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

        [CustomAuthorize(PrivilegeList.ManageContact)]
        [HttpPut("{id}")]
        public async Task<ApiResult<int>> UpdateContact(int id, [FromForm] ContactUpdateDto dto)
        {
            ApiResult<int> result = new ApiResult<int>();
            dto.updated_time = DateTime.Now;
            var check = await _contactService.UpdateContact(id, dto);
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

        [CustomAuthorize(PrivilegeList.ManageContact)]
        [HttpGet("getbyid/{id}")]
        public async Task<ApiResult<ContactDetailDto>> GetContactById(int id)
        {
            ApiResult<ContactDetailDto> result = new ApiResult<ContactDetailDto>();
            var dto = await _contactService.GetContactById(id);
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
