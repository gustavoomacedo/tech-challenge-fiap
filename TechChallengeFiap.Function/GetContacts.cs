using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace TechChallengeFiap.Function
{
    public class GetContacts
    {
        private readonly ILogger<GetContacts> _logger;
        private readonly string _connectionString;

        public GetContacts(ILogger<GetContacts> logger)
        {
            _logger = logger;
            _connectionString = Environment.GetEnvironmentVariable("SqlConnectionString", EnvironmentVariableTarget.Process) ?? "";
        }

        [Function("GetContacts")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "contacts")] HttpRequest req)
        {
            _logger.LogInformation("Recebida solicitação para obter contatos.");

            var contacts = new List<ContactDTO>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    string query = "SELECT Name, Email, DDD, Telefone FROM Contact";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            contacts.Add(new ContactDTO
                            {
                                Name = reader.GetString(0),
                                Email = reader.GetString(1),
                                DDD = reader.GetInt32(2),
                                Telefone = reader.GetInt32(3)
                            });
                        }
                    }
                }

                return new OkObjectResult(contacts);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao acessar o banco de dados: {ex.Message}");
                return new StatusCodeResult(500);
            }
        }
    }
    public class ContactDTO
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public int DDD { get; set; }
        public int Telefone { get; set; }
    }
}
