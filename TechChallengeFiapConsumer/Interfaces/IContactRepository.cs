using TechChallengeFiapConsumerAdd.Models;

namespace TechChallengeFiapConsumerAdd.Interfaces
{
    public interface IContactRepository : IRepository<ContactDto>
    {
        Task<ICollection<ContactDto>> GetContactsByDDD(int DDD);
    }
}
