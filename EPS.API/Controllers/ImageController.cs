using EPS.API.Helpers;
using EPS.Data.Entities;
using EPS.Service;
using EPS.Service.Dtos.Image;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EPS.API.Controllers
{
    [Produces("application/json")]
    [Route("api/image")]
    [Authorize]
    public class ImageController : BaseController
    {
        private ImageService _imageService;

        private IWebHostEnvironment _webHostEnvironment;

        public ImageController(ImageService imageService, IWebHostEnvironment webHostEnvironment)
        {
            _imageService = imageService;
            _webHostEnvironment = webHostEnvironment;
        }

        [CustomAuthorize(PrivilegeList.ViewImage)]
        [HttpGet("images")]
        public async Task<IActionResult> GetImagesByTreeId([FromQuery] ImageGridPagingDto pagingModel)
        {
            return Ok(await _imageService.getImagesByType(pagingModel));
        }
    }
}
