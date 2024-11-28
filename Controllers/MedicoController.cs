using CQRS_Un_Proyecto.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using MySQLService.Repositories;
using MySQLService.Services;

namespace MySQLService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicoController : ControllerBase
    {
        private readonly MedicoRepository _repository;
        private readonly MedicoRabbitMqService _rabbitMqService;

        public MedicoController(MedicoRepository repository, MedicoRabbitMqService rabbitMqService)
        {
            _repository = repository;
            _rabbitMqService = rabbitMqService;
        }

        [HttpPost]
        public async Task<ActionResult<MedicoEntity>> Create([FromBody] MedicoEntity medico)
        {
            var createdMedico = await _repository.AddAsync(medico);

            _rabbitMqService.PublishMessage(createdMedico);

            return CreatedAtAction(nameof(Create), new { id = createdMedico.IdMedico }, createdMedico);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MedicoEntity medico)
        {
            if (id != medico.IdMedico) return BadRequest();

            var updatedMedico = await _repository.UpdateAsync(medico);
            if (updatedMedico == null) return NotFound();

            _rabbitMqService.PublishMessage(updatedMedico);

            return Ok(updatedMedico);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _repository.DeleteAsync(id);
            if (!success) return NotFound();

            _rabbitMqService.PublishMessage(new { IdMedico = id, Action = "Deleted" });

            return NoContent();
        }
    }
}
