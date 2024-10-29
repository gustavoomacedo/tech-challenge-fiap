using TechChallengeFiap.Infrastructure.DTOs;
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
        public async Task<ContactResponseDTO> AddContactAsync(ContactRequestDTO contactDTO)
        {
            try
            {
                if (contactDTO == null)
                {
                    throw new ArgumentNullException(nameof(contactDTO));
                }

                if (!this.GetAllDDDs().Contains(contactDTO.DDD))
                {
                    throw new ArgumentNullException("DDD inválido.");
                }

                var contact = new Contact()
                {
                    DDD = contactDTO.DDD,
                    Email = contactDTO.Email,
                    Name = contactDTO.Name,
                    Telefone = contactDTO.Telefone
                };

                var id = await _contactRepository.Add(contact);

                var contactResponse = new ContactResponseDTO()
                {
                    Id = id,
                    DataCriacao = contact.DataCriacao,
                    DDD = contactDTO.DDD,
                    Email = contactDTO.Email,
                    Name = contactDTO.Name,
                    Telefone = contactDTO.Telefone
                };

                return contactResponse;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao inserir o contato: " + ex);
            }
        }

        public async Task<ICollection<ContactResponseDTO>> GetAllAsync()
        {
            var contacts = await _contactRepository.GetAll();
            var contactDTOs = new List<ContactResponseDTO>();
            foreach (var contact in contacts)
            {
                contactDTOs.Add(new ContactResponseDTO()
                {
                    DDD = contact.DDD,
                    DataCriacao = contact.DataCriacao,
                    Email = contact.Email,
                    Name = contact.Name,
                    Telefone = contact.Telefone,
                    Id = contact.Id
                });
            }

            return contactDTOs;
        }

        public async Task<ContactResponseDTO> GetByIdAsync(int id)
        {
            var contact = await _contactRepository.GetById(id);

            if (contact != null)
            {
                return new ContactResponseDTO()
                {
                    DDD = contact.DDD,
                    DataCriacao = contact.DataCriacao,
                    Email = contact.Email,
                    Name = contact.Name,
                    Telefone = contact.Telefone,
                    Id = contact.Id
                };
            }
            else
            {
                throw new Exception("Contato não encontrado.");
            }

        }

        public async Task deleteContactAsync(int id)
        {
            await _contactRepository.Delete(id);
        }

        public async Task updateContactAsync(ContactUpdateRequestDTO contact)
        {
            Contact contactEntity = await _contactRepository.GetById(contact.Id);

            if (contactEntity == null)
            {
                throw new Exception("Contato não encontrado.");
            }

            if (!this.GetAllDDDs().Contains(contact.DDD))
            {
                throw new ArgumentNullException("DDD inválido.");
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

        public async Task<ICollection<ContactResponseDTO>> GetAllContactsByDDDAsync(int ddd)
        {
            var contacts = await _contactRepository.GetContactsByDDD(ddd);

            var contactDTOs = new List<ContactResponseDTO>();
            foreach (var contact in contacts) 
            {
                contactDTOs.Add(new ContactResponseDTO() 
                { 
                    DDD = contact.DDD,
                    DataCriacao = contact.DataCriacao,
                    Email = contact.Email,
                    Name = contact.Name,
                    Telefone = contact.Telefone,
                    Id = contact.Id
                });
            }

            return contactDTOs;
        }
    }
}
