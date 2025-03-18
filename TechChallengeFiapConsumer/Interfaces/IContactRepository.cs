using TechChallengeFiapConsumer.Models;

namespace TechChallengeFiapConsumer.Interfaces
{
    public interface IContactRepository : IRepository<ContactDto>
    {
        Task<ICollection<ContactDto>> GetContactsByDDD(int DDD);
    }
}
