using TechChallengeFiap.Models;

namespace TechChallengeFiap.Interfaces
{
    public interface IContactService
    {

        Task<Contact> AddContact(Contact contact);
    }
}
