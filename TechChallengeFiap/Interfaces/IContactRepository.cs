using TechChallengeFiap.Models;

namespace TechChallengeFiap.Interfaces
{
    public interface IContactRepository : IRepository<Contact>
    {
        ICollection<Contact> GetContactsByDDD(int DDD);
    }
}
