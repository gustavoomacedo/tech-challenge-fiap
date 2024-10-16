using TechChallengeFiap.Interfaces;
using TechChallengeFiap.Models;

namespace TechChallengeFiap.Infrastructure.Services
{
    public class ContactService : IContactService
    {

        public readonly IContactRepository _contactRepository;

        public ContactService(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        /// <summary>
        /// M
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task<Contact> AddContact(Contact contact)
        {
            try
            {
                if (contact == null)
                {
                    throw new ArgumentNullException(nameof(contact));
                }

                return await _contactRepository.AddAsync(contact);

            }
            catch (Exception ex) 
            {
                throw new Exception("Ocorreu um erro ao inserir o contato: " + ex);
            }

        }
    }
}
