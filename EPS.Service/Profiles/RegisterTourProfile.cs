using AutoMapper;
using EPS.Data.Entities;
using EPS.Service.Dtos.Common.RegisterTour;
using EPS.Service.Dtos.TourDetail;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPS.Service.Profiles
{
    public class RegisterTourProfileDtoToEntity : Profile
    {
        public RegisterTourProfileDtoToEntity()
        {

        }
    }

    public class RegisterTourProfileEntityToDto : Profile
    {
        public RegisterTourProfileEntityToDto()
        {
            CreateMap<register_tour, RegisterTourGridDto>();
            CreateMap<v_detail_tour_register, DetailTourGridDto>();
        }
    }
}
