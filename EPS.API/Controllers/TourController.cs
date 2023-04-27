using EPS.API.Commons;
using EPS.API.Helpers;
using EPS.API.Models.ImageTour;
using EPS.API.Models.Tour;
using EPS.Data.Entities;
using EPS.Service;
using EPS.Service.Dtos.ImageTour;
using EPS.Service.Dtos.Tour;
using EPS.Service.Dtos.TourDetail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace EPS.API.Controllers
{
    [Produces("application/json")]
    [Route("api/tours")]
    [Authorize]
    public class TourController : BaseController
    {
        private TourService _tourService;
        private ImageTourService _imageTourService;
        private IWebHostEnvironment _webHostEnvironment;

        public TourController(TourService tourService, ImageTourService imageTourService, IWebHostEnvironment webHostEnvironment)
        {
            _tourService = tourService;
            _imageTourService = imageTourService;
            _webHostEnvironment = webHostEnvironment;
        }

        #region tour
        [CustomAuthorize(PrivilegeList.ViewTour, PrivilegeList.ManageTour)]
        [HttpGet]
        public async Task<IActionResult> GetListTours([FromQuery] TourGridPagingDto pagingModel)
        {
            return Ok(await _tourService.GetTours(pagingModel));
        }

        [CustomAuthorize(PrivilegeList.ManageTour)]
        [HttpPost]
        public async Task<ApiResult<int>> CreateTour([FromForm] TourCreateViewModel dto)
        {
            ApiResult<int> result = new ApiResult<int>();
            int files = Request.Form.Files.Count;
            TourCreateDto tourDto = new TourCreateDto(dto.category_id, dto.name, dto.url);
            tourDto.created_time = DateTime.Now;
            var id = await _tourService.CreateTours(tourDto);
            if (id == 0)
            {
                var lastId = await _tourService.GetLastTourRecord();
                DetailTourCreateDto detail = new DetailTourCreateDto(lastId, dto.price, dto.infor, dto.intro, Request.Form.Files[0].FileName, dto.schedule, dto.policy, dto.note);
                var checkDetail = await _tourService.CreateDetailTour(detail);
                if (checkDetail == 0)
                {
                    result.ResultObj = checkDetail;
                    result.Message = "Cập nhập thành công !";
                    result.statusCode = 200;
                    return result;
                }
                else
                {
                    result.ResultObj = checkDetail;
                    result.Message = "Đã có lỗi xẩy ra với hệ thống, vui lòng thử lại !";
                    result.statusCode = 500;
                    return result;
                }
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
            var check = await _tourService.DeleteTours(id);
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

        [CustomAuthorize(PrivilegeList.ManageTour)]
        [HttpPut("{id}")]
        public async Task<ApiResult<int>> UpdateTour(int id, [FromForm] TourUpdateViewModel dto)
        {
            ApiResult<int> result = new ApiResult<int>();
            int files = Request.Form.Files.Count;
            TourUpdateDto tourDto = new TourUpdateDto(dto.category_id, dto.name, dto.url);
            tourDto.updated_time = DateTime.Now;
            var check = await _tourService.UpdateTours(id, tourDto);
            if (check == 1)
            {
                var lastId = await _tourService.GetDetailTourById(id);
                DetailTourUpdateDto detail = new DetailTourUpdateDto(id, dto.price, dto.infor, dto.intro, Request.Form.Files[0].FileName, dto.schedule, dto.policy, dto.note);
                var checkDetail = await _tourService.UpdateDetailTourById(lastId, detail);
                if(checkDetail == 1)
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
        public async Task<ApiResult<TourDetailDto>> GetTourById(int id)
        {
            ApiResult<TourDetailDto> result = new ApiResult<TourDetailDto>();
            var dto = await _tourService.GetTourById(id);
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

        [CustomAuthorize(PrivilegeList.ManageTour)]
        [HttpGet("getdetailtourbyid/{id}")]
        public async Task<ApiResult<DetailTourDetailDto>> GetTourDetailById(int id)
        {
            ApiResult<DetailTourDetailDto> result = new ApiResult<DetailTourDetailDto>();
            var dto = await _tourService.GetTourDetailById(id);
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
        #endregion

        #region image_tours
        [CustomAuthorize(PrivilegeList.ViewImage, PrivilegeList.ManageImage)]
        [HttpGet("imagetours")]
        public async Task<IActionResult> GetListImageTours([FromQuery] ImageTourGridPagingDto pagingModel)
        {
            return Ok(await _imageTourService.GetImageTours(pagingModel));
        }

        [CustomAuthorize(PrivilegeList.ManageTour)]
        [HttpPost("imagetours")]
        public async Task<ApiResult<bool>> CreateImageTour([FromForm] ImageTourViewModel dto)
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
                        ImageTourCreateDto imagetour = new ImageTourCreateDto(dto.id_tour, item.FileName);
                        var id = await _imageTourService.CreateImageTours(imagetour);
                        using (var fileSteam = new FileStream(path, FileMode.Create))
                        {
                            await item.CopyToAsync(fileSteam);
                        }
                    }
                    catch(Exception ex)
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
