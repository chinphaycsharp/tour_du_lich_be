using AutoMapper;
using EPS.Data.Entities;
using EPS.Service.Dtos.Contact;
using System.Globalization;

namespace EPS.Service.Profiles
{
    public class ContactProfileDtoToEntity : Profile
    {
        public ContactProfileDtoToEntity()
        {
            CreateMap<ContactCreateDto, contact>();
            CreateMap<ContactUpdateDto, contact>();
        }
    }

    public class ContactProfileEntityToDto : Profile
    {
        public ContactProfileEntityToDto()
        {
            CreateMap<contact, ContactGridDto>()
                .ForMember(dest => dest.created_timeStr, mo => mo.MapFrom(src => src.created_time.ToString("dd/M/yyyy", CultureInfo.InvariantCulture)))
                 .ForMember(dest => dest.updated_timeStr, mo => mo.MapFrom(src => src.updated_time.ToString("dd/M/yyyy", CultureInfo.InvariantCulture)));

            CreateMap<contact, ContactDetailDto>()
                            .ForMember(dest => dest.created_timeStr, mo => mo.MapFrom(src => src.created_time.ToString("dd/M/yyyy", CultureInfo.InvariantCulture)))
                             .ForMember(dest => dest.updated_timeStr, mo => mo.MapFrom(src => src.updated_time.ToString("dd/M/yyyy", CultureInfo.InvariantCulture)));
        }
    }
}
