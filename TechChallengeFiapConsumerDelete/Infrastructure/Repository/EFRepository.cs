using Microsoft.EntityFrameworkCore;
using TechChallengeFiapConsumerDelete.Interfaces;
using TechChallengeFiapConsumerDelete.Models;

namespace TechChallengeFiapConsumerDelete.Infrastructure.Repository
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

        public async Task<int> Add(T entity)
        {
            entity.DataCriacao = DateTime.Now;
            await _context.Set<T>().AddAsync(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            _context.Set<T>().Remove(await GetById(id));
            _context.SaveChanges();
        }

        public async Task<IList<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetById(int id)
        {
            var entity = await _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null)
            {
                throw new KeyNotFoundException($"Entidade com ID {id} não foi encontrada.");
            }

            return entity;
        }

        public async Task Update(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}