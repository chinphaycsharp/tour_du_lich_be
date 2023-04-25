using AutoMapper;
using EPS.Data.Entities;
using EPS.Service.Dtos.Common;
using EPS.Service.Dtos.User;
using EPS.Service.Helpers;
using System.Linq;

namespace EPS.Service.Profiles
{
    public class UserDtoToEntity : Profile
    {
        public UserDtoToEntity()
        {
            CreateMap<UserCreateDto, User>();
            CreateMap<UserUpdateDto, User>();
            CreateMap<ImageUpdateDto, User>();
        }
    }

    public class UserEntityToDto : Profile
    {
        public UserEntityToDto()
        {
            CreateMap<User, UserGridDto>().IgnoreAllNonExisting()
                 .ForMember(dest => dest.Id, mo => mo.MapFrom(src => src.Id))
                 .ForMember(dest => dest.backgroundImage, mo => mo.MapFrom(src => ImageToBase64.HandleImage(src.backgroundImage, "images")));  // Get newly created identity Id back to Dto
            CreateMap<User, UserGridDto>()
                .ForMember(dest => dest.Roles, mo => mo.MapFrom(src => src.UserRoles.Select(x => x.Role.Name)))
                .ForMember(dest => dest.backgroundImage, mo => mo.MapFrom(src => ImageToBase64.HandleImage(src.backgroundImage, "images")));
            CreateMap<User, UserDetailDto>()
                .ForMember(dest => dest.RoleIds, mo => mo.MapFrom(src => src.UserRoles.Select(x => x.RoleId)))
                .ForMember(dest => dest.RoleNames, mo => mo.MapFrom(src => src.UserRoles.Select(x => x.Role.Name)))
                .ForMember(dest => dest.backgroundImage, mo => mo.MapFrom(src => ImageToBase64.HandleImage(src.backgroundImage, "images"))); ;
            CreateMap<UserPrivilege, UserPrivilegeDto>();

        }
    }
}
