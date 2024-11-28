using CQRS_Un_Proyecto.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using MySQLService.Repositories;
using MySQLService.Services;

namespace MySQLService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PacienteController : ControllerBase
    {
        private readonly PacienteRepository _repository;
        private readonly PacienteRabbitMqService _rabbitMqService;

        public PacienteController(PacienteRepository repository, PacienteRabbitMqService rabbitMqService)
        {
            _repository = repository;
            _rabbitMqService = rabbitMqService;
        }
        [HttpPost]
        public async Task<ActionResult<PacienteEntity>> Create([FromBody] PacienteEntity paciente)
        {
            var createdPaciente = await _repository.AddAsync(paciente);

            // Publicar mensaje a RabbitMQ
            _rabbitMqService.PublishMessage(createdPaciente);

            return CreatedAtAction(nameof(Create), new { id = createdPaciente.IdPaciente }, createdPaciente);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PacienteEntity paciente)
        {
            if (id != paciente.IdPaciente) return BadRequest();

            var updatedPaciente = await _repository.UpdateAsync(paciente);
            if (updatedPaciente == null) return NotFound();

            // Publicar mensaje a RabbitMQ
            _rabbitMqService.PublishMessage(updatedPaciente);

            return Ok(updatedPaciente);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _repository.DeleteAsync(id);
            if (!success) return NotFound();

            // Publicar mensaje a RabbitMQ
            _rabbitMqService.PublishMessage(new { IdPaciente = id, Action = "Deleted" });

            return NoContent();
        }
    }
}
