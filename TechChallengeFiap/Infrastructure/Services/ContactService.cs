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
    }
}
