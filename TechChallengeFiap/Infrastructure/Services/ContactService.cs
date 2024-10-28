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
        public async Task<Contact> AddContactAsync(Contact contact)
        {
            try
            {
                if (contact == null)
                {
                    throw new ArgumentNullException(nameof(contact));
                }

                await _contactRepository.Add(contact);
                return contact;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao inserir o contato: " + ex);
            }

        }

        public async Task<ICollection<Contact>> GetAllAsync()
        {
            return await _contactRepository.GetAll();
        }

        public async Task<Contact> GetByIdAsync(int id)
        {
            return await _contactRepository.GetById(id);
        }

        public async Task deleteContactAsync(int id)
        {
            await _contactRepository.Delete(id);
        }

        public async Task updateContactAsync(Contact contact)
        {
            Contact contactEntity = await GetByIdAsync(contact.Id);
            if (contactEntity == null)
            {
                throw new Exception("Contato não encontrado.");
            }

            contactEntity.Name = contact.Name;
            contactEntity.Email = contact.Email;
            contactEntity.DDD = contact.DDD;
            contactEntity.Telefone = contact.Telefone;

            await _contactRepository.Update(contactEntity);
        }

        public ICollection<int> GetAllDDDs()
        {
            return new HashSet<int>() { 11, 12, 13, 14, 15, 16, 17, 18, 19, 21, 22, 24, 27, 28, 31, 32, 33, 34, 35, 37, 38, 41, 42, 43, 44, 45, 46, 47, 48, 49, 51, 53, 54, 55, 61, 62, 63, 64, 65, 66, 67, 68, 69, 71, 73, 74, 75, 77, 79, 81, 82, 83, 84, 85, 86, 87, 88, 89, 91, 92, 93, 94, 95, 96, 97, 98, 99 };
        }

        public async Task<ICollection<Contact>> GetAllContactsByDDDAsync(int ddd)
        {
            return await _contactRepository.GetContactsByDDD(ddd);
        }
    }
}
