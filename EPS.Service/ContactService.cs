using AutoMapper;
using EPS.Data;
using EPS.Data.Entities;
using EPS.Service.Dtos.Contact;
using EPS.Service.Helpers;
using EPS.Utils.Service;
using System.Threading.Tasks;

namespace EPS.Service
{
    public class ContactService
    {
        private EPSBaseService _baseService;
        private EPSRepository _repository;
        private IMapper _mapper;

        public ContactService(EPSRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _baseService = new EPSBaseService(repository, mapper);
        }

        public async Task<PagingResult<ContactGridDto>> GetContacts(ContactPagingGridDto dto)
        {
            return await _baseService.FilterPagedAsync<contact, ContactGridDto>(dto);
        }

        public async Task<int> CreateContact(ContactCreateDto dto, bool isExploiting = false)
        {
            await _baseService.CreateAsync<contact, ContactCreateDto>(dto);
            return dto.id;
        }

        public async Task<int> DeleteContact(int id)
        {
            return await _baseService.DeleteAsync<contact, int>(id);
        }

        public async Task<int> UpdateContact(int id, ContactUpdateDto dto)
        {
            return await _baseService.UpdateAsync<contact, ContactUpdateDto>(id, dto);
        }

        public async Task<ContactDetailDto> GetContactById(int id)
        {
            return await _baseService.FindAsync<contact, ContactDetailDto>(id);
        }
    }
}
