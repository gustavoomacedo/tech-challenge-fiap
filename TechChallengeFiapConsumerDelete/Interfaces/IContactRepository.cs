using TechChallengeFiapConsumerDelete.Models;

namespace TechChallengeFiapConsumerDelete.Interfaces
{
    public interface IContactRepository : IRepository<ContactDto>
    {
        Task<ICollection<ContactDto>> GetContactsByDDD(int DDD);
    }
}
