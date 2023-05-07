using AutoMapper;
using EPS.Data.Entities;
using EPS.Service.Dtos.Category;
using EPS.Service.Dtos.Tour;
using System.Globalization;

namespace EPS.Service.Profiles
{
    public class CategoryProfileDtoToEntity : Profile
    {
        public CategoryProfileDtoToEntity()
        {
            CreateMap<CategoryCreateDto, category>();
            CreateMap<CategoryUpdateDto, category>();
        }
    }

    public class CategoryProfileEntityToDto : Profile
    {
        public CategoryProfileEntityToDto()
        {
            CreateMap<category, CategoryGridDto>()
                .ForMember(dest => dest.created_timeStr, mo => mo.MapFrom(src => src.created_time.ToString("dd/M/yyyy", CultureInfo.InvariantCulture)))
                 .ForMember(dest => dest.updated_timeStr, mo => mo.MapFrom(src => src.updated_time.ToString("dd/M/yyyy", CultureInfo.InvariantCulture)));

            CreateMap<category, CategoryDetailDto>()
                            .ForMember(dest => dest.created_timeStr, mo => mo.MapFrom(src => src.created_time.ToString("dd/M/yyyy", CultureInfo.InvariantCulture)))
                             .ForMember(dest => dest.updated_timeStr, mo => mo.MapFrom(src => src.updated_time.ToString("dd/M/yyyy", CultureInfo.InvariantCulture)));
        }
    }
}
