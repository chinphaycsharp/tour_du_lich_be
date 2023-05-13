using AutoMapper;
using EPS.Data.Entities;
using EPS.Service.Dtos.Tour;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace EPS.Service.Profiles
{
    public class HotelProfileProfileDtoToEntity : Profile
    {
        public HotelProfileProfileDtoToEntity() 
        { 
        }
    }
    public class HotelProfileEntityToDto : Profile
    {
        public HotelProfileEntityToDto()
        {
            CreateMap<hotel, TourGridDto>()
               .ForMember(dest => dest.created_timeStr, mo => mo.MapFrom(src => src.created_time.ToString("dd/M/yyyy", CultureInfo.InvariantCulture)))
                .ForMember(dest => dest.updated_timeStr, mo => mo.MapFrom(src => src.updated_time.ToString("dd/M/yyyy", CultureInfo.InvariantCulture)));
        }
    }
}
