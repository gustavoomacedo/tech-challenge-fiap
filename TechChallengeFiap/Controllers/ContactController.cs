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
        /// M�todo para a adi��o de um contato
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        /// <response code="201">Contato adicionado com sucesso!</response>
        /// <response code="400">Erro na valida��o do contato</response>
        /// <response code="500">N�o foi poss�vel adicionar esse contato</response>
        [HttpPost("AddContact")]
        [AllowAnonymous]
        public async Task<IActionResult> AddContact([FromBody] ContactRequestDTO contact)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { model = ModelState });
                }

                await _messagePublisher.PublishMessageAsync(contact);
                return Created("", contact);
            }
            catch (Exception ex)
            {                
                return StatusCode(500, $"N�o foi poss�vel adicionar esse contato: {ex.Message}");
            }
        }

        /// <summary>
        /// M�todo para listar todos os DDDs
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Todos os DDDs foram buscados com sucesso</response>
        /// <response code="500">N�o foi poss�vel buscar todos os DDDs</response>
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

                return StatusCode(500, $"N�o foi poss�vel processar a requisi��o: {ex.Message}");
            }
        }

        /// <summary>
        /// M�todo para listar todos os contatos
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Todos os Contatos foram buscados com sucesso</response>
        /// <response code="500">N�o foi poss�vel buscar todos os contatos</response>
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

                return StatusCode(500, $"N�o foi poss�vel processar a requisi��o: {ex.Message}");
            }
        }

        /// <summary>
        /// M�todo para a listar todos os contatos por DDD
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Contatos por DDD buscados com sucesso!</response>
        /// <response code="204">N�o foi encontrado nenhum contato com o DDD informado</response>
        /// <response code="500">N�o foi poss�vel buscar os os contatos com o DDD informado</response>
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

        /// <summary>
        /// M�todo atualizar contato
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Contato editado com sucesso!</response>
        /// <response code="400">Erro na valida��o do contato</response>
        /// <response code="500">N�o foi poss�vel editar esse contato</response>
        [HttpPut("update")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateContacts([FromBody] ContactUpdateRequestDTO contact)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { model = ModelState });
                }

                await _contactService.updateContactAsync(contact);
                return Ok();
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"N�o foi poss�vel processar a requisi��o: {ex.Message}");
            }
        }

        /// <summary>
        /// M�todo para deletar um contato
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Contato deletado com sucesso!</response>
        /// <response code="500">N�o foi poss�vel deletar esse contato</response>
        [HttpDelete("delete/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteContacts(int id)
        {
            try
            {
                await _contactService.deleteContactAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"N�o foi poss�vel processar a requisi��o: {ex.Message}");
            }
        }


    }
}
