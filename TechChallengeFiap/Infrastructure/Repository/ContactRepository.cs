using Microsoft.EntityFrameworkCore;
using TechChallengeFiap.Interfaces;
using TechChallengeFiap.Models;

namespace TechChallengeFiap.Infrastructure.Repository
{
    public class ContactRepository : EFRepository<Contact>, IContactRepository
    {
        public ContactRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<ICollection<Contact>> GetContactsByDDD(int DDD)
        {
            return await _context.Contact.Where(x => x.DDD == DDD).ToListAsync();
        }
    }
}