using TechChallengeFiap.Models;

namespace TechChallengeFiap.Interfaces
{
    public interface IContactService
    {
        Contact AddContact(Contact contact);
        ICollection<Contact> GetAll();
        Contact GetById(int id);
    }
}
