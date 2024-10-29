namespace TechChallengeFiap.Infrastructure.DTOs
{
    public class ContactRequestDTO
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required int DDD { get; set; }
        public required int Telefone { get; set; }
    }
}
