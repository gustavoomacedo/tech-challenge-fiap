using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechChallengeFiap.Infrastructure.DTOs;
using TechChallengeFiap.Models;

namespace TechChallengeFiap.Tests
{
    public class ContactFixture
    {
        private readonly Faker _faker;
        public ContactFixture()
        {
            _faker = new Faker();
        }

        public ContactRequestDTO NewContact()
        {
            var name = _faker.Name.FullName();
            int ddd = 11;
            int telefone = _faker.Random.Number(10000000,999999999);
            string email = _faker.Internet.Email();
            return new ContactRequestDTO { Name = name, DDD = ddd, Telefone = telefone, Email = email };
        }
    }
}
