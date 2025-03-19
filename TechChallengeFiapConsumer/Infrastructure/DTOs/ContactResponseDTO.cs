namespace TechChallengeFiapConsumerAdd.Infrastructure.DTOs
{
    public class ContactResponseDTO
    {
        public int Id { get; set; }
        public DateTime DataCriacao { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required int DDD { get; set; }
        public required int Telefone { get; set; }
    }
}
