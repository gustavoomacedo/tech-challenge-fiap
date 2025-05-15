using TechChallengeFiap.Models;

namespace TechChallengeFiap.Interfaces
{
    public interface IContactRepository : IRepository<Contact>
    {
        Task<ICollection<Contact>> GetContactsByDDD(int DDD);
    }
}
