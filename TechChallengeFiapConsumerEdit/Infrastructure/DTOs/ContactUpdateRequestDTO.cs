using System.ComponentModel.DataAnnotations;

namespace TechChallengeFiapConsumerUpdate.Infrastructure.DTOs
{
    public class ContactUpdateRequestDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome não pode ter mais que 100 caracteres.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "O e-mail fornecido não é válido.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "O DDD é obrigatório.")]
        [RegularExpression(@"^\d{2}$", ErrorMessage = "O DDD deve conter exatamente 2 dígitos.")]
        public int DDD { get; set; }

        [Required(ErrorMessage = "O telefone é obrigatório.")]
        [RegularExpression(@"^\d{8,9}$", ErrorMessage = "O telefone deve conter entre 8 ou 9 dígitos.")]
        public int Telefone { get; set; }
    }
}
