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
        public Contact AddContact(Contact contact)
        {
            try
            {
                if (contact == null)
                {
                    throw new ArgumentNullException(nameof(contact));
                }

                _contactRepository.Add(contact);
                return contact;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao inserir o contato: " + ex);
            }

        }

        public ICollection<Contact> GetAll()
        {
            return _contactRepository.GetAll();
        }

        public Contact GetById(int id)
        {
            return _contactRepository.GetById(id);
        }

        public void deleteContact(int id)
        {
            _contactRepository.Delete(id);
        }

        public void updateContact(Contact contact)
        {
            _contactRepository.Update(contact);
        }

        public ICollection<int> GetAllDDDs()
        {
            return new HashSet<int>() { 11, 12, 13, 14, 15, 16, 17, 18, 19, 21, 22, 24, 27, 28, 31, 32, 33, 34, 35, 37, 38, 41, 42, 43, 44, 45, 46, 47, 48, 49, 51, 53, 54, 55, 61, 62, 63, 64, 65, 66, 67, 68, 69, 71, 73, 74, 75, 77, 79, 81, 82, 83, 84, 85, 86, 87, 88, 89, 91, 92, 93, 94, 95, 96, 97, 98, 99 };
        }

        public ICollection<Contact> GetAllContactsByDDD(int ddd)
        {
            return _contactRepository.GetContactsByDDD(ddd);
        }
    }
}
