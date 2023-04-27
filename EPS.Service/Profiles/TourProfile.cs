﻿using AutoMapper;
using EPS.Data.Entities;
using EPS.Service.Dtos.ImageTour;
using EPS.Service.Dtos.Tour;
using EPS.Service.Dtos.TourDetail;
using System.Globalization;

namespace EPS.Service.Profiles
{
    public class TourProfileDtoToEntity : Profile
    {
        public TourProfileDtoToEntity()
        {
            CreateMap<TourCreateDto, tour>();
            CreateMap<TourUpdateDto, tour>();
            CreateMap<DetailTourCreateDto, detail_tour>();
            CreateMap<DetailTourUpdateDto, detail_tour>();
            CreateMap<ImageTourCreateDto, image_tour>();
        }
    }

    public class TourProfileEntityToDto : Profile
    {
        public TourProfileEntityToDto()
        {
            CreateMap<tour, TourGridDto>()
                .ForMember(dest => dest.created_timeStr, mo => mo.MapFrom(src => src.created_time.ToString("dd/M/yyyy", CultureInfo.InvariantCulture)))
                 .ForMember(dest => dest.updated_timeStr, mo => mo.MapFrom(src => src.updated_time.ToString("dd/M/yyyy", CultureInfo.InvariantCulture)));

            CreateMap<detail_tour, TourDetailDto>();

            CreateMap<tour, TourDetailDto>()
                           .ForMember(dest => dest.created_timeStr, mo => mo.MapFrom(src => src.created_time.ToString("dd/M/yyyy", CultureInfo.InvariantCulture)))
                            .ForMember(dest => dest.updated_timeStr, mo => mo.MapFrom(src => src.updated_time.ToString("dd/M/yyyy", CultureInfo.InvariantCulture)));

            CreateMap<image_tour, ImageTourGridDto>()
               .ForMember(dest => dest.img_src, mo => mo.MapFrom(src => "https://3k2nnjkh-5001.asse.devtunnels.ms/uploads/" + src.img_src));
        }
    }
}
