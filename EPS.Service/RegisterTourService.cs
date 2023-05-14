using AutoMapper;
using EPS.Data;
using EPS.Data.Entities;
using EPS.Service.Dtos.Common.RegisterTour;
using EPS.Service.Dtos.Tour;
using EPS.Service.Dtos.TourDetail;
using EPS.Service.Helpers;
using EPS.Utils.Service;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace EPS.Service
{
    public class RegisterTourService
    {
        private EPSBaseService _baseService;
        private EPSRepository _repository;
        private IMapper _mapper;

        public RegisterTourService(EPSRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _baseService = new EPSBaseService(repository, mapper);
        }

        public async Task<PagingResult<RegisterTourGridDto>> GetRegisterTours(RegisterTourGridPagingDto dto)
        {
            return await _baseService.FilterPagedAsync<register_tour, RegisterTourGridDto>(dto);
        }

        public async Task<RegisterTourDetailDto> GetRegisterTourById(int id)
        {
            var dto = await  _baseService.FindAsync<blog, RegisterTourDetailDto>(id);
            return dto;
        }

        public async Task<int> CreateRegisterTour(RegisterTourCreateDto dto, bool isExploiting = false)
        {
            await _baseService.CreateAsync<register_tour, RegisterTourCreateDto>(dto);
            return dto.id;
        }

        public async Task<int> DeleteRegisterTour(int id)
        {
            return await _baseService.DeleteAsync<register_tour, int>(id);
        }

        public async Task<int> UpdateRegisterTour(int id, RegisterTourUpdateDto dto)
        {
            return await _baseService.UpdateAsync<register_tour, RegisterTourUpdateDto>(id, dto);
        }
    }
}
