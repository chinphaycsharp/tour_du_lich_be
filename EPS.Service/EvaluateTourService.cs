using AutoMapper;
using EPS.Data;
using EPS.Data.Entities;
using EPS.Service.Dtos.Common.Evaluate_tour;
using EPS.Service.Dtos.Common.EvaluateTour;
using EPS.Service.Helpers;
using EPS.Utils.Service;
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

        public async Task<PagingResult<EvaluateTourGridDto>> GetEvaluateTours(EvaluateTourGridPagingDto dto)
        {
            return await _baseService.FilterPagedAsync<evaluate_tour, EvaluateTourGridDto>(dto);
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

        public async Task<EvaluateTourDetailDto> GetCategoryById(int id)
        {
            return await _baseService.FindAsync<evaluate_tour, EvaluateTourDetailDto>(id);
        }
    }
}
