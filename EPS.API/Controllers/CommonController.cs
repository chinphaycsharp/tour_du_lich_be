using EPS.API.Commons;
using EPS.API.Helpers;
using EPS.API.Models.Tour;
using EPS.Data.Entities;
using EPS.Service;
using EPS.Service.Dtos.Blog;
using EPS.Service.Dtos.Category;
using EPS.Service.Dtos.Common.EvaluateTour;
using EPS.Service.Dtos.Common.RegisterTour;
using EPS.Service.Dtos.Contact;
using EPS.Service.Dtos.Email;
using EPS.Service.Dtos.Hotel;
using EPS.Service.Dtos.ImageBlog;
using EPS.Service.Dtos.Tour;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        private ContactService _contactService;
        private BlogService _blogService;
        private ImageService _imageService;
        private CategoryService _categoryService;

        public CommonController(RegisterTourService registerTourService, EvaluateTourService evaluateTourService, 
            TourService tourService, HotelService hotelService, EmailService emailService, ContactService contactService, 
            BlogService blogService,ImageService imageService, CategoryService categoryService, LookupService lookupService)
        {
            _registerTourService = registerTourService;
            _evaluateTourService = evaluateTourService;
            _tourService = tourService;
            _hotelService = hotelService;
            _emailService = emailService;
            _contactService = contactService;
            _blogService = blogService;
            _imageService = imageService;
            _categoryService = categoryService;
        }

        #region tours
        [HttpGet("tours")]
        public async Task<IActionResult> GetListTours([FromQuery] TourGridPagingDto pagingModel)
        {
            return Ok(await _tourService.GetTours(pagingModel));
        }

        [HttpGet("gettourbyid/{id}")]
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
                        tourDetail.price, tourDetail.infor, tourDetail.intro, tourDetail.schedule, tourDetail.policy, tourDetail.note,tourDetail.tour_guide,tourDetail.isurance);
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

        [HttpGet("blogs")]
        public async Task<IActionResult> GetBlogs([FromQuery] BlogPagingGridDto pagingModel)
        {
            return Ok(await _blogService.GetBlog(pagingModel));
        }

        [HttpGet("getblogbyid/{id}")]
        public async Task<ApiResult<BlogDetailDto>> GetBlogById(int id)
        {
            ApiResult<BlogDetailDto> result = new ApiResult<BlogDetailDto>();
            var dto = await _blogService.GetBlogById(id);
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

        [HttpGet("hotels")]
        public async Task<IActionResult> GetHotels([FromQuery] HotelPagingGridDto pagingModel)
        {
            return Ok(await _hotelService.GetHotels(pagingModel));
        }

        [HttpGet("gethotelbyid/{id}")]
        public async Task<ApiResult<HotelDetailDto>> GetHotelById(int id)
        {
            ApiResult<HotelDetailDto> result = new ApiResult<HotelDetailDto>();
            var dto = await _hotelService.GetHotelById(id);
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

        [HttpGet("categories")]
        public async Task<IActionResult> GetListCategories([FromQuery] CategoryGridPagingDto pagingModel)
        {
            return Ok(await _categoryService.GetCategories(pagingModel));
        }
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
                MatchCollection matches = Regex.Matches(detailTour.infor, @"(?<=.>).*?(?=</.+>)");

                // Use foreach-loop.
                foreach (Match match in matches)
                {
                    foreach (Capture capture in match.Captures)
                    {
                        inforTour.Add(capture.Value);
                    }
                }
                var x = inforTour.Count > 2 ? inforTour[1] : "" ;
                string body = $"<p>Xin chào:{dto.name_register}</p>\n<p>Sau đây là thông tin của tour:</p>\n<p>Tên:{tour.name}</p>\n <p>Giá: {detailTour.price}VND</p>\n<p>{(inforTour.Count > 1 ? inforTour[0] : "")} {(inforTour.Count > 2 ? inforTour[1] : "")}</p>\n<p>{(inforTour.Count > 3 ? inforTour[2] : "")} {(inforTour.Count > 4 ? inforTour[3] : "")}</p>\n<p>{(inforTour.Count > 5 ? inforTour[4] : "")} {(inforTour.Count > 6 ? inforTour[5] : "")}</p>\n<p>{(inforTour.Count > 7 ? inforTour[6] : "")}</p>";
                UserEmailOptions options = new UserEmailOptions()
                {
                    ToEmails = dto.email_register,
                    Subject = $"Thông tin Tour: " + tour.name
                };

                if (hotels.Count > 0)
                {
                    body = body + $"<p>Khách sạn: {(hotels.Count > 1 ? hotels[0].name : " ")}, {(hotels.Count > 2 ? hotels[1].name : " ")}, {(hotels.Count > 3 ? hotels[2].name : " ")}</p>";
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

        #region about
        [HttpPost("contact")]
        public async Task<ApiResult<int>> Contact(int id, [FromForm] ContactCreateDto dto)
        {
            ApiResult<int> result = new ApiResult<int>();

            var check = await _contactService.CreateContact(dto);
            if (check == 0)
            {
                UserEmailOptions options = new UserEmailOptions()
                {
                    ToEmails = dto.email_register,
                    Subject = $"Thông tin liên hệ: ",
                    Body = @"<p>Xin chào: " + dto.name_register + "</p>" +
                                @"<p>Cảm ơn bạn đã tham khảo dịch vụ của chúng tôi. Chúng tôi sẽ liên hệ theo số điện thoại mà bạn cung cấp!!!</p>" +
                                @"<p>Thân ái!!!</p>"
                };

                await _emailService.SendEmailForEmailConfirmation(options);
                result.ResultObj = id;
                result.Message = "Xác nhận thông tin liên hệ thành công !";
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

        [HttpGet("images")]
        public async Task<IActionResult> GetListImageBlogs([FromQuery] ImageGridPagingDto pagingModel)
        {
            return Ok(await _imageService.GetImageBlogs(pagingModel));
        }

        [HttpGet("gethotelbycategoryid/{id}")]
        public async Task<ApiResult<List<HotelGridDto>>> GetHotelByCategoryId(int id)
        {
            ApiResult<List<HotelGridDto>> result = new ApiResult<List<HotelGridDto>>();
            var dto = await _hotelService.GetHotelByCategoryId(id);
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

        [HttpGet("gettourbycategoryid/{id}")]
        public async Task<ApiResult<List<TourGridDto>>> GetTourByCategoryId(int id)
        {
            ApiResult<List<TourGridDto>> result = new ApiResult<List<TourGridDto>>();
            var dto = await _tourService.GetTourBycategoryId(id);
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

        
        [HttpGet("getcommenttour/{id}")]
        public async Task<ApiResult<List<EvaluateTourGridDto>>> GetCommentTour(int id)
        {
            ApiResult<List<EvaluateTourGridDto>> result = new ApiResult<List<EvaluateTourGridDto>>();
            var dto = await _evaluateTourService.GetEvaluateTours(id);
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
