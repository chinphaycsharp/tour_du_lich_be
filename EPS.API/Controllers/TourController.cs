using EPS.API.Commons;
using EPS.API.Helpers;
using EPS.API.Models.ImageTour;
using EPS.API.Models.Tour;
using EPS.Data.Entities;
using EPS.Service;
using EPS.Service.Dtos.Common.RegisterTour;
using EPS.Service.Dtos.ImageTour;
using EPS.Service.Dtos.Tour;
using EPS.Service.Dtos.TourDetail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
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
        private RegisterTourService _registerTourService;
        private IWebHostEnvironment _webHostEnvironment;

        public TourController(TourService tourService, ImageTourService imageTourService, RegisterTourService registerTourService, IWebHostEnvironment webHostEnvironment)
        {
            _tourService = tourService;
            _imageTourService = imageTourService;
            _registerTourService = registerTourService;
            _webHostEnvironment = webHostEnvironment;
        }

        #region tour
        [CustomAuthorize(PrivilegeList.ManageTour)]
        [HttpGet]
        public async Task<IActionResult> GetListTours([FromQuery] TourGridPagingDto pagingModel)
        {
            return Ok(await _tourService.GetTours(pagingModel));
        }

        [CustomAuthorize(PrivilegeList.ManageTour)]
        [HttpPost]
        public async Task<ApiResult<int>> CreateTour([FromForm] TourCreateViewModelDto dto)
        {
            ApiResult<int> result = new ApiResult<int>();
            TourCreateDto tourDto = new TourCreateDto(dto.category_id, dto.name, dto.url, Request.Form.Files[0].FileName);
            tourDto.created_time = DateTime.Now;
            if (Request.Form.Files.Count > 0)
            {
                var path = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", Request.Form.Files[0].FileName);
                using (var fileSteam = new FileStream(path, FileMode.Create))
                {
                    await Request.Form.Files[0].CopyToAsync(fileSteam);
                }
            }
            var id = await _tourService.CreateTours(tourDto);
            if (id == 0)
            {
                var lastId = await _tourService.GetLastTourRecord();
                DetailTourCreateDto detail = new DetailTourCreateDto(lastId, dto.price, dto.infor, dto.intro, dto.schedule, dto.policy, dto.note, Request.Form.Files[0].FileName);
                var checkDetail = await _tourService.CreateDetailTour(detail);
                if (checkDetail == 0)
                {
                    result.ResultObj = checkDetail;
                    result.Message = "Thêm mới thành công !";
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
            var detailTour = await _tourService.GetDetailTourById(id);
            if(detailTour.id_tour < 0)
            {
                result.ResultObj = default;
                result.Message = "Đã có lỗi xẩy ra với hệ thống, vui lòng thử lại !";
                result.statusCode = 500;
            }
            var checkdetails = await _tourService.DeleteDetailTours(id);
            if (checkdetails == 1)
            {
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
        public async Task<ApiResult<int>> UpdateTour(int id, [FromForm] TourUpdateViewModel dto)
        {
            ApiResult<int> result = new ApiResult<int>();
            TourUpdateDto tourDto = new TourUpdateDto(dto.category_id, dto.name, dto.url, Request.Form.Files[0].FileName);
            tourDto.updated_time = DateTime.Now;
            if(Request.Form.Files.Count > 0)
            {
                var path = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", Request.Form.Files[0].FileName);
                using (var fileSteam = new FileStream(path, FileMode.Create))
                {
                    await Request.Form.Files[0].CopyToAsync(fileSteam);
                }
            }
            var check = await _tourService.UpdateTours(id, tourDto);
            if (check == 1)
            {
                var detailTour = await _tourService.GetDetailTourById(id);
                DetailTourUpdateDto detail = new DetailTourUpdateDto(id, dto.price, dto.infor, dto.intro, dto.schedule, dto.policy, dto.note, Request.Form.Files[0].FileName);
                var checkDetail = await _tourService.UpdateDetailTourById(detailTour.id_tour, detail);
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
        public async Task<ApiResult<TourDetailViewModel>> GetTourById(int id)
        {
            ApiResult<TourDetailViewModel> result = new ApiResult<TourDetailViewModel>();
            var dto = await _tourService.GetTourById(id);
            if (dto != null)
            {
                var tourDetail = await _tourService.GetTourDetailById(id);
                if (tourDetail != null)
                {
                    var tourDetailViewModel = new TourDetailViewModel(dto.id, dto.category_id, dto.name, dto.url,
                        dto.created_time.ToString("dd/M/yyyy", CultureInfo.InvariantCulture), dto.status, dto.background_image,
                        tourDetail.price, tourDetail.infor, tourDetail.intro, tourDetail.schedule, tourDetail.policy, tourDetail.note);
                    result.ResultObj = tourDetailViewModel;
                    result.Message = "";
                    result.statusCode = 200;
                    return result;
                }
                else
                {
                    result.ResultObj = new TourDetailViewModel();
                    result.Message = "Đã có lỗi xẩy ra với hệ thống, vui lòng thử lại !";
                    result.statusCode = 500;
                    return result;
                }
            }
            else
            {
                result.ResultObj = new TourDetailViewModel();
                result.Message = "Đã có lỗi xẩy ra với hệ thống, vui lòng thử lại !";
                result.statusCode = 500;
                return result;
            }
        }
        #endregion

        #region image_tours
        [CustomAuthorize( PrivilegeList.ManageImage)]
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

        #region register_tours
        [CustomAuthorize(PrivilegeList.ManageTour)]
        [HttpGet("getregistertours")]
        public async Task<IActionResult> GetListRegisterTours([FromQuery] RegisterTourGridPagingDto pagingModel)
        {
            return Ok(await _registerTourService.GetRegisterTours(pagingModel));
        }

        [CustomAuthorize(PrivilegeList.ManageTour)]
        [HttpDelete("registertour/{id}")]
        public async Task<ApiResult<int>> DeleteRegisterTour(int id)
        {
            ApiResult<int> result = new ApiResult<int>();
            var check = await _registerTourService.DeleteRegisterTour(id);
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
        #endregion
    }
}
