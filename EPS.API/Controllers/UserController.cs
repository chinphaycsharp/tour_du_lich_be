using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using EPS.API.Helpers;
using EPS.Data.Entities;
using EPS.Service.Dtos.User;
using EPS.Service;
using System.Collections.Generic;
using EPS.API.Commons;
using ClosedXML.Excel;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Data;
using System.Reflection;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using OfficeOpenXml.Style;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace EPS.API.Controllers
{
    [Produces("application/json")]
    [Route("api/users")]
    [Authorize]
    public class UserController : BaseController
    {
        private AuthorizationService _authorizationService;
        private IWebHostEnvironment _webHostEnvironment;

        public UserController(AuthorizationService authorizationService, IWebHostEnvironment webHostEnvironment)
        {
            _authorizationService = authorizationService;
            _webHostEnvironment = webHostEnvironment;
        }

        [CustomAuthorize(PrivilegeList.ViewUser)]
        [HttpGet("list")]
        public async Task<IActionResult> GetUsers([FromQuery]UserGridPagingDto pagingModel)
        {
            return Ok(await _authorizationService.GetUsers(pagingModel));
        }

        public static ExcelPackage getApplicantsStatistics(List<UserGridDto> data)
        {
            ExcelPackage p = ExcelHelper.CreateDoc("Applicant Statistics", "Applicant Statistics", "Applicant Statistics");
            var worksheet = p.Workbook.Worksheets.Add("Applicant Statistics");

            //Add Report Header
            worksheet.Cells[1, 1].RichText.Add("Danh sách tài khoản").Bold = true;
            worksheet.Cells[1, 1].Style.Font.Size = 20;
            worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[1, 1, 1, 3].Merge = true;

            //First add the headers
            worksheet.Cells[2, 1].Value = "STT";
            worksheet.Cells[2, 2].Value = "Tên";
            worksheet.Cells[2, 3].Value = "Họ và tên";
            worksheet.Cells[2, 4].Value = "Email";
            worksheet.Cells[2, 5].Value = "SDT";
            //Add values
            var numberformat = "#,##0";
            var dataCellStyleName = "TableNumber";
            var numStyle = p.Workbook.Styles.CreateNamedStyle(dataCellStyleName);
            numStyle.Style.Numberformat.Format = numberformat;

            for (int i = 0; i < data.Count; i++)
            {
                worksheet.Cells[i + 3, 1].Value = i + 1;
                worksheet.Cells[i + 3, 2].Value = data[i].FullName;
                worksheet.Cells[i + 3, 3].Value = data[i].Username;
                worksheet.Cells[i + 3, 4].Value = data[i].Email;
                worksheet.Cells[i + 3, 5].Value = data[i].PhoneNumber;
            }
            // Add to table / Add summary row
            var rowEnd = data.Count + 2;
            var tbl = worksheet.Tables.Add(new ExcelAddressBase(fromRow: 2, fromCol: 1, toRow: rowEnd, toColumn: 5), "Applicant");
            tbl.ShowHeader = true;
            tbl.TableStyle = TableStyles.Dark9; 
            tbl.ShowTotal = true;
            worksheet.Cells[rowEnd, 3].Style.Numberformat.Format = numberformat;

            // AutoFitColumns
            worksheet.Cells[2, 1, rowEnd, 5].AutoFitColumns();
            return p;
        }

        [CustomAuthorize(PrivilegeList.ViewUser)]
        [HttpGet("{id}")]
        public async Task<ApiResult<UserDetailDto>> GetUserById(int id)
        {
            ApiResult<UserDetailDto> result = new ApiResult<UserDetailDto>();
            var dto = await _authorizationService.GetUserById(id);
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
            //return Ok(await _authorizationService.GetUserById(id));
        }

        [CustomAuthorize(PrivilegeList.ManageUser)]
        [HttpPost]
        public async Task<ApiResult<int>> CreateUser([FromForm] UserCreateDto newUser)
        {
            ApiResult<int> result = new ApiResult<int>();

            if(Request.Form.Files.Count > 0)
            {
                var file = Request.Form.Files[0];
                newUser.backgroundImage = "avartar//" + file.FileName;
                var path = Path.Combine(_webHostEnvironment.WebRootPath, "images\\avartar", file.FileName);
                using (var fileSteam = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(fileSteam);
                }
            }
            var isAdmin = newUser.RoleIds.Any(x => x == 1);
            if (isAdmin)
            {
                newUser.IsAdministrator = true;
            }
            var id = await _authorizationService.CreateUser(newUser);
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
            //return Ok(await _authorizationService.CreateUser(newUser));
        }

        [CustomAuthorize(PrivilegeList.ManageUser)]
        [HttpPut("{id}")]
        public async Task<ApiResult<bool>> UpdateUser(int id, [FromForm] UserUpdateDto editedUser)
        {
            ApiResult<bool> result = new ApiResult<bool>();
            var roleIds = JsonConvert.DeserializeObject<List<int>>(editedUser.RoleIds);
            var isAdmin = roleIds.Any(x => x == 1);
            if (isAdmin)
            {
                editedUser.IsAdministrator = true;
            }
            if (Request.Form.Files.Count > 0)
            {
                var file = Request.Form.Files[0];
                editedUser.backgroundImage = file.FileName;
                var path = Path.Combine(_webHostEnvironment.WebRootPath, "images", file.FileName);
                using (var fileSteam = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(fileSteam);
                }
            }
            else
            {
                editedUser.backgroundImage = editedUser.imageName;
            }
            editedUser.CreateDate = DateTime.Now;
            var check = await _authorizationService.UpdateUser(id, editedUser,roleIds);
            if (check)
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
            //return Ok(await _authorizationService.UpdateUser(id, editedUser));
        }

        [CustomAuthorize(PrivilegeList.ManageUser)]
        [HttpPut("upload/{id}")]
        public async Task<ApiResult<bool>> UpLoadImage(int id)
        {
            ApiResult<bool> result = new ApiResult<bool>();
            ImageUpdateDto editedUser = new ImageUpdateDto();
            editedUser.Id = id;
            var file = Request.Form.Files;
            if (file != null)
            {
                editedUser.backgroundImage = "avartar//" + file[0].FileName;
                var path = Path.Combine(_webHostEnvironment.WebRootPath, "images\\avartar", file[0].FileName);
                using (var fileSteam = new FileStream(path, FileMode.Create))
                {
                    await file[0].CopyToAsync(fileSteam);
                }
                var dto = await _authorizationService.GetUserById(id);

            }
            var check = await _authorizationService.UpdateImage(id, editedUser);
            if (check)
            {
                result.ResultObj = check;
                result.Message = "Tải ảnh thành công !";
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
            //return Ok(await _authorizationService.UpdateUser(id, editedUser));
        }

        [CustomAuthorize(PrivilegeList.ManageUser)]
        [HttpDelete("{id}")]
        public async Task<ApiResult<int>> DeleteUser(int id)
        {
            ApiResult<int> result = new ApiResult<int>();
            var check = await _authorizationService.DeleteUser(id);
            if (check == 0)
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
            //return Ok(await _authorizationService.DeleteUser(id));
        }

        [CustomAuthorize(PrivilegeList.ManageUser)]
        [HttpGet("{id}/privileges")]
        public async Task<ApiResult<List<string>>> GetUserPrivileges(int id)
        {
            ApiResult<List<string>> result = new ApiResult<List<string>>();
            var dto = await _authorizationService.GetUserPrivileges(id);
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
            //return Ok(await _authorizationService.GetUserPrivileges(id));
        }

        [CustomAuthorize(PrivilegeList.ManageUser)]
        [HttpPut("{id}/privileges")]
        public async Task<IActionResult> SaveUserPrivileges(int id, string[] privileges)
        {
            await _authorizationService.SaveUserPrivileges(id, privileges);
            return Ok();
        }

        //multiple delete 
        [CustomAuthorize(PrivilegeList.ManageUser)]
        [HttpDelete]
        public async Task<IActionResult> DeleteUsers(string ids)
        {
            if (string.IsNullOrEmpty(ids))
            {
                return BadRequest();
            }
            try
            {
                var userIds = ids.Split(',').Select(x => Convert.ToInt32(x)).ToArray();
                return Ok(await _authorizationService.DeleteUser(userIds));
            }
            catch (FormatException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [CustomAuthorize(PrivilegeList.ManageUser)]
        [HttpPut]
        public async Task<ApiResult<bool>> ChangePassword([FromForm] ChangePasswordDto model)
        {
            {
                ApiResult<bool> result = new ApiResult<bool>();
                var check = await _authorizationService.ChangePassword( UserIdentity.Username, model);
                if (check)
                {
                    result.ResultObj = check;
                    result.Message = "Đổi mật khẩu thành công !";
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
                //return Ok(await _authorizationService.UpdateUser(id, editedUser));
            }
        }
    }
}
