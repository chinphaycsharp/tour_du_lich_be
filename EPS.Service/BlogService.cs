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
using EPS.Service.Dtos.Blog.BlogContent;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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

        public async Task<int> CreateContentBlog(string content, int idBLog)
        {
            string[] contents = content.Split("\n");
            int count = 1;
            foreach (var item in contents)
            {
                BlogContentCreateDto dto = new BlogContentCreateDto()
                {
                    id_blog= idBLog,
                    content = item.ToString(),
                    status = 1,
                    order = count
                };
                await _baseService.CreateAsync<content_blog, BlogContentCreateDto>(dto);
                count++;
            }
            return 0;
        }

        public async Task<int> DeleteBlog(int id)
        {
            return await _baseService.DeleteAsync<blog, int>(id);
        }

        public async Task<int> UpdateBlog(int id, BlogUpdateDto dto)
        {
            return await _baseService.UpdateAsync<blog, BlogUpdateDto>(id, dto);
        }

        //public async Task<int> UpdateContentBlog(int idBlog, BlogUpdateDto dto)
        //{
        //    var blogContents = await _repository.Filter<content_blog>(x => x.id_blog == idBlog).ToListAsync();
        //    foreach (var item in blogContents)
        //    {

        //    }
        //    return await _baseService.UpdateAsync<content_blog, BlogContentUpdateDto>(id, dto);
        //}

        public async Task<BlogDetailRegexDto> GetBlogById(int id)
        {
            var blog = await _baseService.FindAsync<blog, BlogDetailDto>(id);
            if(blog == null)
            {
                return new BlogDetailRegexDto();
            }
            var blogContents = await _repository.Filter<content_blog>(x => x.id_blog == blog.id).ToListAsync();
            if(blogContents == null)
            {
                return new BlogDetailRegexDto();
            }
            var contents = blogContents.OrderBy(x => x.order).ToList();
            BlogDetailRegexDto detailRegexDto = new BlogDetailRegexDto()
            {
                id = blog.id,
                title = blog.title,
                img_src = blog.img_src,
                created_timeStr = blog.created_timeStr,
                updated_timeStr = blog.updated_timeStr,
                contents = (from x in contents select new ContentBlogItem() { id = x.id, content = x.content,order = x.order }).ToList()
            };
            
            return detailRegexDto;
        }
    }
}
