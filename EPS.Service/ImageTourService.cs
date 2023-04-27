using AutoMapper;
using EPS.Data;
using EPS.Data.Entities;
using EPS.Service.Dtos.ImageTour;
using EPS.Service.Dtos.Tour;
using EPS.Service.Helpers;
using EPS.Utils.Service;
using System.Threading.Tasks;

namespace EPS.Service
{
    public class ImageTourService
    {
        private EPSBaseService _baseService;
        private EPSRepository _repository;
        private IMapper _mapper;

        public ImageTourService(EPSRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _baseService = new EPSBaseService(repository, mapper);
        }

        public async Task<PagingResult<ImageTourGridDto>> GetImageTours(ImageTourGridPagingDto dto)
        {
            return await _baseService.FilterPagedAsync<image_tour, ImageTourGridDto>(dto);
        }

        public async Task<int> CreateImageTours(ImageTourCreateDto dto, bool isExploiting = false)
        {
            await _baseService.CreateAsync<image_tour, ImageTourCreateDto>(dto);
            return dto.id;
        }
    }
}
