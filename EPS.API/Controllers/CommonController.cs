using EPS.API.Commons;
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
        public CommonController(RegisterTourService registerTourService, EvaluateTourService evaluateTourService)
        {
            _registerTourService = registerTourService;
            _evaluateTourService = evaluateTourService;
        }

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
