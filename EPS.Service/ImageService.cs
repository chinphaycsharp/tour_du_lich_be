using AutoMapper;
using EPS.Data;
using EPS.Data.Entities;
using EPS.Service.Dtos.ImageBlog;
using EPS.Service.Helpers;
using EPS.Utils.Service;
using System.Threading.Tasks;

namespace EPS.Service
{
    public class ImageService
    {
        private EPSBaseService _baseService;
        private EPSRepository _repository;
        private IMapper _mapper;

        public ImageService(EPSRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _baseService = new EPSBaseService(repository, mapper);
        }

        public async Task<PagingResult<ImageGridDto>> GetImageTours(ImageGridPagingDto dto)
        {
            return await _baseService.FilterPagedAsync<image, ImageGridDto>(dto);
        }

        public async Task<int> CreateImageTours(ImageCreateDto dto, bool isExploiting = false)
        {
            await _baseService.CreateAsync<image, ImageCreateDto>(dto);
            return dto.id;
        }

        public async Task<PagingResult<ImageGridDto>> GetImageBlogs(ImageGridPagingDto dto)
        {
            return await _baseService.FilterPagedAsync<image, ImageGridDto>(dto);
        }

        public async Task<int> CreateImageBlogs(ImageCreateDto dto, bool isExploiting = false)
        {
            await _baseService.CreateAsync<image, ImageCreateDto >(dto);
            return dto.id;
        }
    }
}
