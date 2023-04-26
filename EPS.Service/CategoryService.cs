using AutoMapper;
using EPS.Data;
using EPS.Data.Entities;
using EPS.Service.Dtos.Category;
using EPS.Service.Dtos.Category;
using EPS.Service.Helpers;
using EPS.Utils.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EPS.Service
{
    public class CategoryService
    {
        private EPSBaseService _baseService;
        private EPSRepository _repository;
        private IMapper _mapper;

        public CategoryService (EPSRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _baseService = new EPSBaseService(repository, mapper);
        }

        public async Task<PagingResult<CategoryGridDto>> GetCategories(CategoryGridPagingDto dto)
        {
            return await _baseService.FilterPagedAsync<category, CategoryGridDto>(dto);
        }

        public async Task<int> CreateCategory(CategoryCreateDto dto, bool isExploiting = false)
        {
            await _baseService.CreateAsync<category, CategoryCreateDto>(dto);
            return dto.id;
        }

        public async Task<int> DeleteCategory(int id)
        {
            return await _baseService.DeleteAsync<category, int>(id);
        }

        public async Task<int> UpdateCategory(int id, CategoryUpdateDto dto)
        {
            return await _baseService.UpdateAsync<category, CategoryUpdateDto>(id, dto);
        }

        public async Task<CategoryDetailDto> GetCategoryById(int id)
        {
            return await _baseService.FindAsync<category, CategoryDetailDto>(id);
        }
    }
}
