using AutoMapper;
using EPS.Data;
using EPS.Data.Entities;
using EPS.Service.Dtos.Common;
using EPS.Service.Dtos.Privilege;
using EPS.Service.Dtos.Tour;
using EPS.Service.Helpers;
using EPS.Utils.Service;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPS.Service
{
    public class TourService
    {
        private EPSBaseService _baseService;
        private EPSRepository _repository;
        private IMapper _mapper;

        public TourService(EPSRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _baseService = new EPSBaseService(repository, mapper);
        }

        public async Task<PagingResult<TourGridDto>> GetTours(TourGridPagingDto dto)
        {
            return await _baseService.FilterPagedAsync<tour, TourGridDto>(dto);
        }

        public async Task<int> CreateTours(TourCreateDto dto, bool isExploiting = false)
        {
            await _baseService.CreateAsync<tour, TourCreateDto>(dto);
            return dto.id;
        }

        public async Task<int> DeleteTours(int id)
        {
            return await _baseService.DeleteAsync<tour, int>(id);
        }

        public async Task<int> UpdateTours(int id, TourUpdateDto dto)
        {
            return await _baseService.UpdateAsync<tour, TourUpdateDto>(id, dto);
        }

        public async Task<TourDetailDto> GetTourById(int id)
        {
            return await _baseService.FindAsync<tour, TourDetailDto>(id);
        }
    }
}
