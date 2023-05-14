using AutoMapper;
using EPS.Data.Entities;
using EPS.Service.Dtos.Blog;
using EPS.Service.Dtos.Category;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace EPS.Service.Profiles
{
    public class BlogProfileDtoToEntity : Profile
    {
        public BlogProfileDtoToEntity()
        {
            CreateMap<BlogCreateDto, blog>();
            CreateMap<BlogUpdateDto, blog>();
        }
    }

    public class BlogProfileEntityToDto : Profile
    {
        public BlogProfileEntityToDto()
        {
            CreateMap<blog, BlogGridDto>()
                          .ForMember(dest => dest.created_timeStr, mo => mo.MapFrom(src => src.created_time.ToString("dd/M/yyyy", CultureInfo.InvariantCulture)))
                           .ForMember(dest => dest.updated_timeStr, mo => mo.MapFrom(src => src.updated_time.ToString("dd/M/yyyy", CultureInfo.InvariantCulture)));

            CreateMap<blog, BlogDetailDto>()
                            .ForMember(dest => dest.created_timeStr, mo => mo.MapFrom(src => src.created_time.ToString("dd/M/yyyy", CultureInfo.InvariantCulture)))
                             .ForMember(dest => dest.updated_timeStr, mo => mo.MapFrom(src => src.updated_time.ToString("dd/M/yyyy", CultureInfo.InvariantCulture)));
        }
    }
}
