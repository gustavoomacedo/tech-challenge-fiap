using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechChallengeFiap.Interfaces;
using TechChallengeFiap.Infrastructure.DTOs;
using TechChallengeFiap.RabbitMQ;

namespace TechChallengeFiap.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContactController : ControllerBase
    {
       
        private readonly ILogger<ContactController> _logger;
        private readonly IContactService _contactService;
        private readonly IMessagePublisher _messagePublisher;

        

        public ContactController(ILogger<ContactController> logger, IContactService contactService, IMessagePublisher messagePublisher)
        {
            _logger = logger;
            _messagePublisher = messagePublisher;
            _contactService = contactService;
        }


        /// <summary>
        /// Método para listar todos os DDDs
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Todos os DDDs foram buscados com sucesso</response>
        /// <response code="500">Não foi possível buscar todos os DDDs</response>
        [HttpGet("GetDDDs")]
        [AllowAnonymous]
        public IActionResult DDD()
        {
            try
            {
                var ddds = _contactService.GetAllDDDs();
                return Ok(ddds);
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Não foi possível processar a requisição: {ex.Message}");
            }
        }

        /// <summary>
        /// Método para listar todos os contatos
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Todos os Contatos foram buscados com sucesso</response>
        /// <response code="500">Não foi possível buscar todos os contatos</response>
        [HttpGet("GetAll")]
        [AllowAnonymous]
        public async Task<IActionResult> Contacts()
        {
            try
            {
                var contacts = await _contactService.GetAllAsync();
                return Ok(contacts);
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Não foi possível processar a requisição: {ex.Message}");
            }
        }

        /// <summary>
        /// Método para a listar todos os contatos por DDD
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Contatos por DDD buscados com sucesso!</response>
        /// <response code="204">Não foi encontrado nenhum contato com o DDD informado</response>
        /// <response code="500">Não foi possível buscar os os contatos com o DDD informado</response>
        [HttpGet("GetByDdd")]
        [AllowAnonymous]
        public async Task<IActionResult> GetContactsByDdd(int ddd)
        {
            try
            {
                ICollection<ContactResponseDTO> contacts = await _contactService.GetAllContactsByDDDAsync(ddd);

                if (contacts == null || !contacts.Any())
                    return NoContent(); 

                return Ok(contacts); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ops! Ocorreu um erro: {ex.Message}");
            }
        }

    }
}
