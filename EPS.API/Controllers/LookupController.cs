using EPS.API.Commons;
using EPS.API.Helpers;
using EPS.Data.Entities;
using EPS.Service;
using EPS.Service.Dtos.Privilege;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EPS.API.Controllers
{
    [Produces("application/json")]
    [Route("api/lookup")]
    [Authorize]
    public class LookupController : BaseController
    {
        private LookupService _lookupService;
        private AuthorizationService _authorizationService;

        public LookupController(LookupService lookupService, AuthorizationService authorizationService)
        {
            _lookupService = lookupService;
            _authorizationService = authorizationService;
        }

        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles()
        {
            return Ok(await _lookupService.GetRoles());
        }

        [HttpGet("privileges")]
        public async Task<IActionResult> GetPrivileges([FromQuery] PrivilegeGridPagingDto dto)
        {
            return Ok(await _lookupService.GetPrivileges(dto));
        }

        [CustomAuthorize(PrivilegeList.ManageUser)]
        [HttpPost]
        public async Task<ApiResult<string>> CreatePrivilege([FromForm] PrivilegeCreateDto model)
        {
            ApiResult<string> result = new ApiResult<string>();
            string[] names = model.Id.Split(" ");
            model.Id = "";
            for (int i = 0; i < names.Length; i++)
            {
                model.Id += names[i];
            }
            model.Status = true;
            var id = await _lookupService.CreatePrivilege(model);
            if (id != "")
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

        [CustomAuthorize(PrivilegeList.ManageUser)]
        [HttpDelete("{id}")]
        public async Task<ApiResult<string>> DetelePrivilege(string id)
        {
            ApiResult<string> result = new ApiResult<string>();
            PrivilegeUpdateDto model = new PrivilegeUpdateDto()
            {
                Id = id,
                Status = false
            };
            var check = await _lookupService.UpdatePrivilege(id, model);
            if (id != "")
            {
                string[] arr = new string[] { id };
                int roleId = await _authorizationService.GetRoleByPrivilegeId(id);
                int userId = await _authorizationService.GetUserByPrivilegeId(id);
                await _authorizationService.DeleteRolePrivilege(roleId, arr);
                await _authorizationService.DeleteUserPrivileges(userId, arr);
                result.ResultObj = id;
                result.Message = "Xóa thành công !";
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

        [CustomAuthorize(PrivilegeList.ManageUser)]
        [HttpPut("{id}")]
        public async Task<ApiResult<int>> UpdatePrivilege(string id, [FromForm] PrivilegeUpdateDto model)
        {
            ApiResult<int> result = new ApiResult<int>();
            var check = await _lookupService.UpdatePrivilege(id, model);
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

        [HttpGet("nameprivilege")]
        [AllowAnonymous]
        public async Task<IActionResult> GetNamePrivilege()
        {
            return Ok(await _lookupService.GetNamePrivilege());
        }

        [HttpGet("getcontainprivilegename/{name}/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetContainPrivilegeName(string name, int id)
        {
            return Ok(await _lookupService.GetContainPrivilegeName(name, id));
        }
    }
}
