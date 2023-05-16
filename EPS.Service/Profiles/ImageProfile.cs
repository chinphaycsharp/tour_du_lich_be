using AutoMapper;
using EPS.Data.Entities;
using EPS.Service.Dtos.Contact;
using EPS.Service.Dtos.ImageTour;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace EPS.Service.Profiles
{
    public class ImageProfileDtoToEntity : Profile
    {
        public ImageProfileDtoToEntity()
        {

        }
    }

    public class ImageProfileEntityToDto : Profile
    {
        public ImageProfileEntityToDto()
        {
            CreateMap<image_tour, ImageTourGridDto>()
               .ForMember(dest => dest.img_src, mo => mo.MapFrom(src => "http://192.168.1.5:5001/uploads/" + src.img_src));
        }
    }
}
