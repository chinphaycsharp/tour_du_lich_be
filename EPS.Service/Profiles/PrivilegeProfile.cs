using AutoMapper;
using EPS.Data.Entities;
using EPS.Service.Dtos.Privilege;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPS.Service.Profiles
{
    public class PrivilegeProfileDtoToEntity : Profile
    {
        public PrivilegeProfileDtoToEntity()
        {
            CreateMap<PrivilegeCreateDto, Privilege>();
            CreateMap<PrivilegeUpdateDto, Privilege>();
        }
    }

    public class PrivilegeProfileEntityToDto : Profile
    {
        public PrivilegeProfileEntityToDto() {
            CreateMap<Privilege, PrivilegeGridDto>();
        }
    }
}
