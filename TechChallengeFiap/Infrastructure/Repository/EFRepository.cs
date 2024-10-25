using Microsoft.EntityFrameworkCore;
using TechChallengeFiap.Interfaces;
using TechChallengeFiap.Models;

namespace TechChallengeFiap.Infrastructure.Repository
{
    public class EFRepository<T> : IRepository<T> where T : EntityBase
    {
        protected ApplicationDbContext _context;
        protected DbSet<T> _dbSet;

        public EFRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public int Add(T entity)
        {
            entity.DataCriacao = DateTime.Now;
            _context.Set<T>().Add(entity);
            return _context.SaveChanges();
        }

        public void Delete(int id)
        {
            _context.Set<T>().Remove(GetById(id));
            _context.SaveChanges();
        }

        public IList<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public T GetById(int id)
        {
            var entity = _context.Set<T>().FirstOrDefault(x => x.Id == id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Entidade com ID {id} não foi encontrada.");
            }
            return entity;
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
            _context.SaveChanges();
        }
    }
}