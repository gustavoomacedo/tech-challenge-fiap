using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechChallengeFiapAdd.Infrastructure.DTOs;
using TechChallengeFiapAdd.RabbitMQ;

namespace TechChallengeFiapAdd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContactController : ControllerBase
    {
       
        private readonly IMessagePublisher _messagePublisher;

        

        public ContactController(ILogger<ContactController> logger, IMessagePublisher messagePublisher)
        {
            _messagePublisher = messagePublisher;
        }


        /// <summary>
        /// Método para a adição de um contato
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        /// <response code="201">Contato adicionado com sucesso!</response>
        /// <response code="400">Erro na validação do contato</response>
        /// <response code="500">Não foi possível adicionar esse contato</response>
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
                return StatusCode(500, $"Não foi possível adicionar esse contato na fila: {ex.Message}");
            }
        }

    }
}
