using AutoMapper;
using EPS.Data;
using EPS.Data.Entities;
using EPS.Service.Dtos.Common;
using EPS.Service.Dtos.Privilege;
using EPS.Service.Dtos.Tour;
using EPS.Service.Dtos.TourDetail;
using EPS.Service.Helpers;
using EPS.Utils.Service;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<int> GetLastTourRecord()
        {
            var id = await _repository.Filter<tour>(x => x.id > 0).Select(x => x.id).MaxAsync();
            return id;
        }

        public async Task<detail_tour> GetDetailTourById(int Tourid)
        {
            var id = await _repository.Filter<detail_tour>(x => x.id_tour == Tourid).FirstOrDefaultAsync();
            return id;
        }

        public async Task<int> CreateTours(TourCreateDto dto, bool isExploiting = false)
        {
            await _baseService.CreateAsync<tour, TourCreateDto>(dto);
            return dto.id;
        }
        
        public async Task<int> CreateDetailTour(DetailTourCreateDto dto, bool isExploiting = false)
        {
            await _baseService.CreateAsync<detail_tour, DetailTourCreateDto>(dto);
            return dto.id;
        }

        public async Task<int> DeleteTours(int id)
        {
            return await _baseService.DeleteAsync<tour, int>(id);
        }

        public async Task<int> DeleteTourAndHotelByCategoryId(int id)
        {
            int result = 0;
            var tours = await _repository.Filter<tour>(x => x.category_id == id).ToListAsync();
            foreach (var item in tours)
            {
                result = 0;
                result = await _baseService.DeleteAsync<tour, int>(item.id);
            }
            var hotels = await _repository.Filter<hotel>(x => x.category_id == id).ToListAsync();
            foreach (var item in hotels)
            {
                result = 0;
                result = await _baseService.DeleteAsync<hotel, int>(item.id);
            }
            return result;
        }

        public async Task<int> DeleteDetailTours(int id)
        {
            return await _baseService.DeleteAsync<detail_tour, int>(id);
        }

        public async Task<int> UpdateTours(int id, TourUpdateDto dto)
        {
            return await _baseService.UpdateAsync<tour, TourUpdateDto>(id, dto);
        }

        public async Task<int> UpdateDetailTourById(int id, DetailTourUpdateDto dto)
        {
            return await _baseService.UpdateAsync<detail_tour, DetailTourUpdateDto>(id, dto);
        }

        public async Task<TourDetailDto> GetTourById(int id)
        {
            return await _baseService.FindAsync<tour, TourDetailDto>(id);
        }

        public async Task<DetailTourDetailDto> GetTourDetailById(int tourId)
        {
            var detailtour = await _repository.Filter<detail_tour>(x=>x.id_tour == tourId).FirstOrDefaultAsync();
            if(detailtour != null)
            {
                DetailTourDetailDto dto = new DetailTourDetailDto();
                detailtour.id = dto.id;
                detailtour.id_tour = tourId;
                detailtour.price = dto.price;
                detailtour.infor = dto.infor;
                detailtour.schedule = dto.schedule;
                detailtour.policy = dto.policy;
                detailtour.note = dto.note;
                return dto;
            }
            return new DetailTourDetailDto();
        }
    }
}
