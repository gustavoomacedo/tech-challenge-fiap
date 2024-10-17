using TechChallengeFiap.Models;

namespace TechChallengeFiap.Interfaces
{
    public interface IRepository<T> where T : EntityBase
    {
        IList<T> GetAll();
        T GetById(int id);
        int Add(T entity);
        void Update(T entity);
        void Delete(int id);
    }
}