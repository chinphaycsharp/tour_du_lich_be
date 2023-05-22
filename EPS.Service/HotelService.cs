using AutoMapper;
using EPS.Data;
using EPS.Data.Entities;
using EPS.Service.Dtos.Category;
using EPS.Service.Dtos.Hotel;
using EPS.Service.Dtos.Tour;
using EPS.Service.Helpers;
using EPS.Utils.Service;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPS.Service
{
    public class HotelService
    {
        private EPSBaseService _baseService;
        private EPSRepository _repository;
        private IMapper _mapper;

        public HotelService(EPSRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _baseService = new EPSBaseService(repository, mapper);
        }

        public async Task<PagingResult<HotelGridDto>> GetHotels(HotelPagingGridDto dto)
        {
            return await _baseService.FilterPagedAsync<hotel, HotelGridDto>(dto);
        }

        public async Task<List<HotelGridDto>> GetHotelByCategoryId(int category_id)
        {
            var result = await _repository.Filter<hotel>(x => x.category_id == category_id).ToListAsync();
            List<HotelGridDto> hotels = new List<HotelGridDto>();
            foreach (var item in result)
            {
                HotelGridDto hotel = new HotelGridDto()
                {
                    id = item.id,
                    category_id = item.category_id,
                    name = item.name,
                    status = item.status,
                };
                hotels.Add(hotel);
            }
            return hotels;
        }

        //public async Task<PagingResult<HotelGridDto>> GetHotels(HotelPagingGridDto dto)
        //{
        //    return await _baseService.FilterPagedAsync<hotel, HotelGridDto>(dto);
        //}

        public async Task<int> CreateHotel(HotelCreateDto dto, bool isExploiting = false)
        {
            await _baseService.CreateAsync<hotel, HotelCreateDto>(dto);
            return dto.id;
        }

        public async Task<int> DeleteHotel(int id)
        {
            return await _baseService.DeleteAsync<hotel, int>(id);
        }

        public async Task<int> UpdateHotel(int id, HotelUpdateDto dto)
        {
            return await _baseService.UpdateAsync<hotel, HotelUpdateDto>(id, dto);
        }

        public async Task<HotelDetailDto> GetHotelById(int id)
        {
            return await _baseService.FindAsync<hotel, HotelDetailDto>(id);
        }
    }
}
