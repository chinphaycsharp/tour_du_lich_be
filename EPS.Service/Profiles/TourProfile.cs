using AutoMapper;
using EPS.Data.Entities;
using EPS.Service.Dtos.Tour;
using System.Globalization;

namespace EPS.Service.Profiles
{
    public class TourProfileDtoToEntity : Profile
    {
    }

    public class TourProfileEntityToDto : Profile
    {
        public TourProfileEntityToDto()
        {
            CreateMap<tour, TourGridDto>()
                .ForMember(dest => dest.created_timeStr, mo => mo.MapFrom(src => src.created_time.ToString("dd/M/yyyy", CultureInfo.InvariantCulture)))
                 .ForMember(dest => dest.updated_timeStr, mo => mo.MapFrom(src => src.updated_time.ToString("dd/M/yyyy", CultureInfo.InvariantCulture)));

            CreateMap<tour, TourDetailDto>()
                            .ForMember(dest => dest.created_timeStr, mo => mo.MapFrom(src => src.created_time.ToString("dd/M/yyyy", CultureInfo.InvariantCulture)))
                             .ForMember(dest => dest.updated_timeStr, mo => mo.MapFrom(src => src.updated_time.ToString("dd/M/yyyy", CultureInfo.InvariantCulture)));
        }
    }
}
