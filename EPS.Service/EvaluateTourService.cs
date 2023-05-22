using AutoMapper;
using EPS.Data;
using EPS.Data.Entities;
using EPS.Service.Dtos.Common.Evaluate_tour;
using EPS.Service.Dtos.Common.EvaluateTour;
using EPS.Service.Helpers;
using EPS.Utils.Service;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace EPS.Service
{
    public class EvaluateTourService
    {
        private EPSBaseService _baseService;
        private EPSRepository _repository;
        private IMapper _mapper;

        public EvaluateTourService(EPSRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _baseService = new EPSBaseService(repository, mapper);
        }

        public async Task<List<EvaluateTourGridDto>> GetEvaluateTours(int tour_id)
        {
            var result = await _repository.Filter<evaluate_tour>(x => x.id_tour == tour_id).ToListAsync();
            List<EvaluateTourGridDto> evaluateTours = new List<EvaluateTourGridDto>();
            foreach (var item in result)
            {
                EvaluateTourGridDto evaluateTour = new EvaluateTourGridDto()
                {
                    id = item.id,
                    id_tour = item.id_tour,
                    content = item.content,
                    star_count = item.star_count,
                    created_timeStr = item.created_time.ToString("dd/M/yyyy", CultureInfo.InvariantCulture),
                    updated_timeStr = item.updated_time.ToString("dd/M/yyyy", CultureInfo.InvariantCulture),
                    status = item.status,
                };
                evaluateTours.Add(evaluateTour);
            }
            return evaluateTours;
        }

        public async Task<int> EvaluateTours(EvaluateTourCreateDto dto, bool isExploiting = false)
        {
            await _baseService.CreateAsync<evaluate_tour, EvaluateTourCreateDto>(dto);
            return dto.id;
        }

        public async Task<int> DeleteCategory(int id)
        {
            return await _baseService.DeleteAsync<evaluate_tour, int>(id);
        }

        public async Task<int> UpdateCategory(int id, EvaluateTourUpdateDto dto)
        {
            return await _baseService.UpdateAsync<evaluate_tour, EvaluateTourUpdateDto>(id, dto);
        }

        public async Task<EvaluateTourDetailDto> GetEvaluateTourById(int id)
        {
            return await _baseService.FindAsync<evaluate_tour, EvaluateTourDetailDto>(id);
        }
    }
}
