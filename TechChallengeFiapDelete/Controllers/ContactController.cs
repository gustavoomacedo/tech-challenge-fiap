using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechChallengeFiapDelete.Infrastructure.DTOs;
using TechChallengeFiapDelete.RabbitMQ;

namespace TechChallengeFiapDelete.Controllers
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
        /// Método para deletar um contato
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Contato deletado com sucesso!</response>
        /// <response code="500">Não foi possível deletar esse contato</response>
        [HttpDelete("delete/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteContacts(int id)
        {
            try
            {
                await _messagePublisher.PublishMessageAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Não foi possível processar a requisição na fila: {ex.Message}");
            }
        }


    }
}
