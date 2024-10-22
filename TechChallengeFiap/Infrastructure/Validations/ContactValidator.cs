using FluentValidation;
using TechChallengeFiap.Interfaces;
using TechChallengeFiap.Models;

namespace TechChallengeFiap.Infrastructure.Validations
{
    public class ContactValidator : AbstractValidator<Contact>
    {
        private readonly IContactService _contactService;
        public ContactValidator(IContactService contactService)
        {
            _contactService = contactService;

            RuleFor(x => x.Name).NotEmpty().WithMessage("Nome não pode ser vazio");
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.DDD).Custom((number, context) =>
            {
                var ddds = _contactService.GetAllDDDs();
                if (!ddds.Contains(number))
                    context.AddFailure("DDD inválido.");
            });
        }
    }
}
