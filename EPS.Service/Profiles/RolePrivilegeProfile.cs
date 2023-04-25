using AutoMapper;
using EPS.Data.Entities;
using EPS.Service.Dtos.RolePrivilege;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPS.Service.Profiles
{
    public class RolePrivilegeProfileDtoToEntity : Profile
    {
    }

    public class RolePrivilegeProfileEntityToDto : Profile
    {

        public RolePrivilegeProfileEntityToDto()
        {
            CreateMap<RolePrivilege, RolePrivilegeGridDto>();
        }
    }
}
