using AutoMapper;
using EPS.Data.Entities;
using EPS.Data;
using EPS.Service.Dtos.Category;
using EPS.Service.Helpers;
using EPS.Utils.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EPS.Service.Dtos.Blog;
using System.Text.RegularExpressions;

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

        public async Task<BlogDetailRegexDto> GetBlogById(int id)
        {
            var blog = await _baseService.FindAsync<blog, BlogDetailDto>(id);
            BlogDetailRegexDto detailRegexDto = new BlogDetailRegexDto()
            {
                id = blog.id,
                title = blog.title,
                img_src = blog.img_src,
                created_timeStr = blog.created_timeStr,
                updated_timeStr = blog.updated_timeStr
            };
            RegexOptions options = RegexOptions.Multiline;
            var matches = Regex.Matches(blog.content, @"^[a-z,0-9]*.+\.$", options);
            foreach (Match m in matches)
            {
                detailRegexDto.contents.Add(m.Value);
            }
            return detailRegexDto;
        }
    }
}
