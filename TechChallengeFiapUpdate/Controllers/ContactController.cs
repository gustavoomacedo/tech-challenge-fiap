using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechChallengeFiapUpdate.Infrastructure.DTOs;
using TechChallengeFiapUpdate.RabbitMQ;

namespace TechChallengeFiapUpdate.Controllers
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

                await _messagePublisher.PublishMessageAsync(contact);
                return Ok();
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"N�o foi poss�vel processar a requisi��o: {ex.Message}");
            }
        }
    }
}
