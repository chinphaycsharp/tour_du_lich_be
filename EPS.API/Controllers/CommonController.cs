using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Spreadsheet;
using EPS.API.Commons;
using EPS.API.Helpers;
using EPS.API.Models.Tour;
using EPS.Data.Entities;
using EPS.Service;
using EPS.Service.Dtos.Common.EvaluateTour;
using EPS.Service.Dtos.Common.RegisterTour;
using EPS.Service.Dtos.Email;
using EPS.Service.Dtos.Tour;
using EPS.Service.Dtos.TourDetail;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
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
        private HotelService _hotelService;
        private EmailService _emailService;
        public CommonController(RegisterTourService registerTourService, EvaluateTourService evaluateTourService, TourService tourService, HotelService hotelService, EmailService emailService)
        {
            _registerTourService = registerTourService;
            _evaluateTourService = evaluateTourService;
            _tourService = tourService;
            _hotelService = hotelService;
            _emailService = emailService;
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
                var tour = await _tourService.GetTourById(dto.id_tour);
                var detailTour = await _tourService.GetDetailTourById(dto.id_tour);
                var hotels = await _hotelService.GetHotelByCategoryId(tour.category_id);
                List<string> inforTour = new List<string>();
                MatchCollection matches = Regex.Matches(detailTour.infor, @"<strong>.*?</strong>");
                
                // Use foreach-loop.
                foreach (Match match in matches)
                {
                    foreach (Capture capture in match.Captures)
                    {
                        inforTour.Add(capture.Value);
                    }
                }
                var x = inforTour[0] != null ? inforTour[0] : "";
                string body = @"<p>Xin chào: " + dto.name_register + "</p>" +
                                @"<p>Sau đây là thông tin của tour:</p>"+
                                @"<p>Tên: " + tour.name + "</p>" +
                                @"<p>Giá: " + detailTour.price + "</p>" +
                                @"<p>" + inforTour[0] + " " + inforTour[1] + "</p>" +
                                @"<p>" + inforTour[2] + " " + inforTour[3] +"</p>" +
                                @"<p>" + inforTour[4] + " " + inforTour[5] + "</p>" +
                                @"<p>" + inforTour[6] + " " + inforTour[7] + "</p>";
                UserEmailOptions options = new UserEmailOptions()
                {
                    ToEmails = dto.email_register,
                    Subject = $"Thông tin Tour: " + tour.name
                };

                if (hotels.Count > 0)
                {
                    body = body + @"<p>Khách sạn: " + hotels[0].name + ", " + hotels[1].name + ", " + hotels[2].name + "</p>";
                }
                body = body + @"<p>Cảm ơn bạn đã tham khảo dịch vụ của chúng tôi. Chúng tôi sẽ liên hệ theo số điện thoại mà bạn cung cấp!!!</p>" +
                                @"<p>Thân ái!!!</p>";

                options.Body = body;

                await _emailService.SendEmailForEmailConfirmation(options);
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

        #region confirm_tour
        [HttpPut("confirmtour/{id}")]
        public async Task<ApiResult<int>> ConfirmTour(int id, [FromForm] RegisterTourUpdateDto dto)
        {
            ApiResult<int> result = new ApiResult<int>();

            var check = await _registerTourService.UpdateRegisterTour(id, dto);
            if (check == 1)
            {
                var tour = await _tourService.GetTourById(dto.id_tour);
                UserEmailOptions options = new UserEmailOptions()
                {
                    ToEmails = dto.email_register,
                    Subject = $"Thông tin Tour: " + tour.name,
                    Body = @"<p>Xin chào: " + dto.name_register + "</p>" +
                                @"<p>Bạn đã đăng ký tour:" + tour.name + " thành công!!!</p>" +
                                @"<p>Cảm ơn bạn đã tham khảo dịch vụ của chúng tôi. Chúng tôi sẽ liên hệ theo số điện thoại mà bạn cung cấp!!!</p>" +
                                @"<p>Thân ái!!!</p>"
                };

                await _emailService.SendEmailForEmailConfirmation(options);
                result.ResultObj = id;
                result.Message = "Xác nhận tour thành công !";
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
