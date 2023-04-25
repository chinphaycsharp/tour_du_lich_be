using EPS.API.Commons;
using EPS.API.Helpers;
using EPS.API.Models;
using EPS.Data.Entities;
using EPS.Service;
using EPS.Service.Dtos.Role;
using EPS.Service.Dtos.RolePrivilege;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPS.API.Controllers
{
    [Produces("application/json")]
    [Route("api/roles")]
    [Authorize]
    public class RoleController : BaseController
    {
        private AuthorizationService _authorizationService;

        public RoleController(AuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        //list all
        [CustomAuthorize(PrivilegeList.ViewRole, PrivilegeList.ManageRole)]
        [HttpGet]
        public async Task<IActionResult> GetListRoles([FromQuery]RoleGridPagingDto pagingModel)
        {
            return Ok(await _authorizationService.GetRoles(pagingModel));
        }

        //get privileges by roleId  
        [CustomAuthorize(PrivilegeList.ViewRole, PrivilegeList.ManageRole)]
        [HttpGet("{id}/privileges")]
        public async Task<IActionResult> GetRolePrivieleges(int id)
        {
            return Ok(await _authorizationService.GetRolePrivileges(id));
        }

        [CustomAuthorize(PrivilegeList.ManageRole)]
        [HttpPut("{id}/saveroles")]
        public async Task<IActionResult> SaveRolePrivileges(int id, RolePrivilegeUpdateDto dto)
        {
            await _authorizationService.SaveRolePrivileges(id, dto.privileges);

            return Ok();
        }

        //detail
        [CustomAuthorize(PrivilegeList.ViewRole, PrivilegeList.ManageRole)]
        [HttpGet("{id}")]
        public async Task<ApiResult<RoleDetailDto>> GetRoleById(int id)
        {
            ApiResult<RoleDetailDto> result = new ApiResult<RoleDetailDto>();
            var dto = await _authorizationService.GetRoleById(id);
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
            //return Ok(await _authorizationService.GetRoleById(id));
        }

        //create
        [CustomAuthorize(PrivilegeList.ManageRole)]
        [HttpPost]
        public async Task<ApiResult<int>> Create(RoleCreateDto roleCreateDto)
        {
            ApiResult<int> result = new ApiResult<int>();
            var dto = await _authorizationService.CreateRole(roleCreateDto);
            if (dto > 0)
            {
                result.ResultObj = dto;
                result.Message = "Thêm mới thành công !";
                result.statusCode = 201;
                return result;
            }
            else
            {
                result.ResultObj = dto;
                result.Message = "Đã có lỗi xẩy ra với hệ thống, vui lòng thử lại !";
                result.statusCode = 500;
                return result;
            }
            //return Ok(await _authorizationService.CreateRole(roleCreateDto));
        }

        //update
        [CustomAuthorize(PrivilegeList.ManageRole)]
        [HttpPut("{id}")]
        public async Task<ApiResult<int>> UpdateRole(int id, RoleUpdateDto roleUpdateDto)
        {
            ApiResult<int> result = new ApiResult<int>();
            var dto = await _authorizationService.UpdateRole(id, roleUpdateDto);
            if (dto == 1)
            {
                result.ResultObj = dto;
                result.Message = "Cập nhập thành công !";
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
            //return Ok(await _authorizationService.UpdateRole(id, roleUpdateDto));
        }

        [CustomAuthorize(PrivilegeList.ManageRole)]
        [HttpDelete("{id}")]
        public async Task<ApiResult<int>> Delete(int id)
        {
            ApiResult<int> result = new ApiResult<int>();
            List<string> PrivilegeIds = await _authorizationService.GetRolePrivileges(id);
            string[] arr = PrivilegeIds.Select(i => i.ToString()).ToArray();
            await _authorizationService.DeleteRolePrivilege(id, arr);
            var check = await _authorizationService.DeleteRole(id);
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
           // return Ok(await _authorizationService.DeleteRole(id));
        }

        //multiple delete 
        [CustomAuthorize(PrivilegeList.ManageRole)]
        [HttpDelete]
        public async Task<IActionResult> DeleteRoles(string ids)
        {
            if (string.IsNullOrEmpty(ids))
            {
                return BadRequest();
            }
            try
            {
                var roleIds = ids.Split(',').Select(x => Convert.ToInt32(x)).ToArray();
                return Ok(await _authorizationService.DeleteRole(roleIds));
            }
            catch (FormatException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
