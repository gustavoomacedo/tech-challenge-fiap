using TechChallengeFiap.Models;

namespace TechChallengeFiap.Interfaces
{
    public interface IContactService
    {
        Task<Contact> AddContactAsync(Contact contact);

        Task<ICollection<Contact>> GetAllAsync();

        Task<Contact> GetByIdAsync(int id);

        Task updateContactAsync(Contact contact);

        Task deleteContactAsync(int id);

        Task<ICollection<Contact>> GetAllContactsByDDDAsync(int ddd);

        ICollection<int> GetAllDDDs();
    }
}
