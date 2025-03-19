using TechChallengeFiapConsumerUpdate.Models;

namespace TechChallengeFiapConsumerUpdate.Interfaces
{
    public interface IContactRepository : IRepository<ContactDto>
    {
        Task<ICollection<ContactDto>> GetContactsByDDD(int DDD);
    }
}
