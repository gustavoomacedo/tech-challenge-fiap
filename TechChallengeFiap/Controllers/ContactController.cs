using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using TechChallengeFiap.Interfaces;
using TechChallengeFiap.Models;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.IdentityModel.Tokens;

namespace TechChallengeFiap.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContactController : ControllerBase
    {
       
        private readonly ILogger<ContactController> _logger;
        private IValidator<Contact> _validator;
        private readonly IContactService _contactService;

        public ContactController(ILogger<ContactController> logger, IValidator<Contact> validator, IContactService contactService)
        {
            _logger = logger;
            _contactService = contactService;
            _validator = validator;
        }


        /// <summary>
        /// M�todo para a adi��o de um contato
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        /// <response code="200">Contato adicionado com sucesso!</response>
        /// <response code="400">Erro na valida��o do contato</response>
        /// <response code="500">N�o foi poss�vel adicionar esse contato</response>
        [HttpPost("AddContact")]
        [AllowAnonymous]
        public IActionResult AddContact([FromBody] Contact contact)
        {
            try
            {
                ValidationResult validationResult = _validator.Validate(contact);
                if (!ModelState.IsValid || !validationResult.IsValid)
                {
                    return BadRequest(new { model = ModelState, errors = validationResult.Errors });
                }

                var returnContact = _contactService.AddContact(contact);
                return Created("", returnContact);
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, $"N�o foi poss�vel adicionar esse contato: {ex.Message}");
            }
        }

        /// <summary>
        /// M�todo para a listar todos os contatos
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Contato adicionado com sucesso!</response>
        /// <response code="400">Erro na valida��o do contato</response>
        /// <response code="500">N�o foi poss�vel adicionar esse contato</response>
        [HttpGet("GetAll")]
        [AllowAnonymous]
        public IActionResult Contacts()
        {
            try
            {
                var contacts = _contactService.GetAll();
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
        public IActionResult GetContactsByDdd(int ddd)
        {
            try
            {
                var contacts = _contactService.GetAllContactsByDDD(ddd);

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
        /// <response code="200">Contato adicionado com sucesso!</response>
        /// <response code="400">Erro na valida��o do contato</response>
        /// <response code="500">N�o foi poss�vel adicionar esse contato</response>
        [HttpPut("update")]
        [AllowAnonymous]
        public IActionResult UpdateContacts([FromBody] Contact contact)
        {
            try
            {
                _contactService.updateContact(contact);
                return Ok();
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"N�o foi poss�vel processar a requisi��o: {ex.Message}");
            }
        }

        /// <summary>
        /// M�todo para deletar
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Contato adicionado com sucesso!</response>
        /// <response code="400">Erro na valida��o do contato</response>
        /// <response code="500">N�o foi poss�vel adicionar esse contato</response>
        [HttpDelete("delete/{id}")]
        [AllowAnonymous]
        public IActionResult DeleteContacts(int id)
        {
            try
            {
                _contactService.deleteContact(id);
                return Ok();
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"N�o foi poss�vel processar a requisi��o: {ex.Message}");
            }
        }


    }
}
