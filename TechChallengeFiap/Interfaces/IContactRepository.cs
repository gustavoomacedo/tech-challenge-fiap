using TechChallengeFiap.Models;

namespace TechChallengeFiap.Interfaces
{
    public interface IContactRepository : IRepository<ContactDto>
    {
        Task<ICollection<ContactDto>> GetContactsByDDD(int DDD);
    }
}
