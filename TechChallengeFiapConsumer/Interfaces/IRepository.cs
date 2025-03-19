using TechChallengeFiapConsumerAdd.Models;

namespace TechChallengeFiapConsumerAdd.Interfaces
{
    public interface IRepository<T> where T : EntityBase
    {
        Task<IList<T>> GetAll();
        Task<T> GetById(int id);
        Task<int> Add(T entity);
        Task Update(T entity);
        Task Delete(int id);
    }
}