﻿using Microsoft.EntityFrameworkCore;
using TechChallengeFiapConsumerDelete.Interfaces;
using TechChallengeFiapConsumerDelete.Models;

namespace TechChallengeFiapConsumerDelete.Infrastructure.Repository
{
    public class ContactRepository : EFRepository<ContactDto>, IContactRepository
    {
        public ContactRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<ICollection<ContactDto>> GetContactsByDDD(int DDD)
        {
            return await _context.Contact.Where(x => x.DDD == DDD).ToListAsync();
        }
    }
}