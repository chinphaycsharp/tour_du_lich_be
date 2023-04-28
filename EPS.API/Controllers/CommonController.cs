﻿using EPS.API.Commons;
using EPS.API.Helpers;
using EPS.API.Models.Tour;
using EPS.Data.Entities;
using EPS.Service;
using EPS.Service.Dtos.Common.EvaluateTour;
using EPS.Service.Dtos.Common.RegisterTour;
using EPS.Service.Dtos.Tour;
using EPS.Service.Dtos.TourDetail;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace EPS.API.Controllers
{
    [Produces("application/json")]
    [Route("api/commmon")]
    public class CommonController : BaseController
    {
        private RegisterTourService _registerTourService;
        private EvaluateTourService _evaluateTourService;
        private TourService _tourService;
        public CommonController(RegisterTourService registerTourService, EvaluateTourService evaluateTourService, TourService tourService)
        {
            _registerTourService = registerTourService;
            _evaluateTourService = evaluateTourService;
            _tourService = tourService;
        }

        #region tours
        [HttpGet("tours")]
        public async Task<IActionResult> GetListTours([FromQuery] TourGridPagingDto pagingModel)
        {
            return Ok(await _tourService.GetTours(pagingModel));
        }

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
                    var tourDetailViewModel = new TourDetailViewModel(dto.id,dto.category_id,dto.name,dto.url,
                        dto.created_time.ToString("dd/M/yyyy", CultureInfo.InvariantCulture),dto.status,dto.background_image,
                        tourDetail.price,tourDetail.infor,tourDetail.intro,tourDetail.schedule,tourDetail.policy,tourDetail.note);
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

        //[CustomAuthorize(PrivilegeList.ManageTour)]
        //[HttpGet("getdetailtourbyid/{id}")]
        //public async Task<ApiResult<DetailTourDetailDto>> GetTourDetailById(int id)
        //{
        //    ApiResult<DetailTourDetailDto> result = new ApiResult<DetailTourDetailDto>();
        //    var dto = await _tourService.GetTourDetailById(id);
        //    if (dto != null)
        //    {
        //        result.ResultObj = dto;
        //        result.Message = "";
        //        result.statusCode = 200;
        //        return result;
        //    }
        //    else
        //    {
        //        result.ResultObj = dto;
        //        result.Message = "Đã có lỗi xẩy ra với hệ thống, vui lòng thử lại !";
        //        result.statusCode = 500;
        //        return result;
        //    }
        //}
        #endregion

        #region register_tours
        [HttpPost("resgistertour")]
        public async Task<ApiResult<int>> CreateRegisterTour([FromForm] RegisterTourCreateDto dto)
        {
            dto.created_time = DateTime.Now;
            ApiResult<int> result = new ApiResult<int>();

            var id = await _registerTourService.CreateRegisterTour(dto);
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
        #endregion

        #region evaluate_tour
        [HttpPost("evaluatetour")]
        public async Task<ApiResult<int>> EvaluateTour([FromForm] EvaluateTourCreateDto dto)
        {
            dto.created_time = DateTime.Now;
            ApiResult<int> result = new ApiResult<int>();

            var id = await _evaluateTourService.EvaluateTours(dto);
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
        #endregion
    }
}
