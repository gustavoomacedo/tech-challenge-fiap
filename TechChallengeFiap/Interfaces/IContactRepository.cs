using TechChallengeFiap.Models;

namespace TechChallengeFiap.Interfaces
{
    public interface IContactRepository
    {
        Task<Contact> AddAsync(Contact contact);
    }
}
