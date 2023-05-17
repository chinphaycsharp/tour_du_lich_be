using AutoMapper;
using EPS.Data;
using EPS.Data.Entities;
using EPS.Service.Dtos.Blog;
using EPS.Service.Helpers;
using EPS.Utils.Service;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace EPS.Service
{
    public class BlogService
    {
        private EPSBaseService _baseService;
        private EPSRepository _repository;
        private IMapper _mapper;

        public BlogService(EPSRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _baseService = new EPSBaseService(repository, mapper);
        }

        public async Task<PagingResult<BlogGridDto>> GetBlog(BlogPagingGridDto dto)
        {
            return await _baseService.FilterPagedAsync<blog, BlogGridDto>(dto);
        }

        public async Task<int> GetLastBlogRecord()
        {
            var id = await _repository.Filter<blog>(x => x.id > 0).Select(x => x.id).MaxAsync();
            return id;
        }

        public async Task<int> CreateBlog(BlogCreateDto dto, bool isExploiting = false)
        {
            await _baseService.CreateAsync<blog, BlogCreateDto>(dto);
            return dto.id;
        }

        public async Task<int> DeleteBlog(int id)
        {
            return await _baseService.DeleteAsync<blog, int>(id);
        }

        public async Task<int> UpdateBlog(int id, BlogUpdateDto dto)
        {
            return await _baseService.UpdateAsync<blog, BlogUpdateDto>(id, dto);
        }

        public async Task<BlogDetailDto> GetBlogById(int id)
        {
            var blog = await _baseService.FindAsync<blog, BlogDetailDto>(id);
            return blog;
        }
    }
}
