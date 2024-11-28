using CQRS_Un_Proyecto.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using MySQLService.Repositories;
using MySQLService.Services;

namespace MySQLService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistorialEstadoConsultaController : ControllerBase
    {
        private readonly HistorialEstadoConsultaRepository _repository;
        private readonly HistorialEstadoConsultaRabbitMqService _rabbitMqService;

        public HistorialEstadoConsultaController(HistorialEstadoConsultaRepository repository, HistorialEstadoConsultaRabbitMqService rabbitMqService)
        {
            _repository = repository;
            _rabbitMqService = rabbitMqService;
        }

        [HttpPost]
        public async Task<ActionResult<HistorialEstadoConsultaEntity>> Create([FromBody] HistorialEstadoConsultaEntity historial)
        {
            var createdHistorial = await _repository.AddAsync(historial);

            // Publicar mensaje a RabbitMQ
            _rabbitMqService.PublishMessage(createdHistorial);

            return CreatedAtAction(nameof(Create), new { id = createdHistorial.IdHistorialEstado }, createdHistorial);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] HistorialEstadoConsultaEntity historial)
        {
            if (id != historial.IdHistorialEstado) return BadRequest();

            var updatedHistorial = await _repository.UpdateAsync(historial);
            if (updatedHistorial == null) return NotFound();

            // Publicar mensaje a RabbitMQ
            _rabbitMqService.PublishMessage(updatedHistorial);

            return Ok(updatedHistorial);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _repository.DeleteAsync(id);
            if (!success) return NotFound();

            // Publicar mensaje a RabbitMQ
            _rabbitMqService.PublishMessage(new { IdHistorialEstado = id, Action = "Deleted" });

            return NoContent();
        }
    }
}
