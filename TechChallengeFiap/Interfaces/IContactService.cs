using TechChallengeFiap.Infrastructure.DTOs;
using TechChallengeFiap.Models;

namespace TechChallengeFiap.Interfaces
{
    public interface IContactService
    {
        Task<ContactResponseDTO> AddContactAsync(ContactRequestDTO contact);

        Task<ICollection<ContactResponseDTO>> GetAllAsync();

        Task<ContactResponseDTO> GetByIdAsync(int id);

        Task updateContactAsync(ContactUpdateRequestDTO contact);

        Task deleteContactAsync(int id);

        Task<ICollection<ContactResponseDTO>> GetAllContactsByDDDAsync(int ddd);

        ICollection<int> GetAllDDDs();
    }
}
