using CQRS_Un_Proyecto.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using MySQLService.Repositories;
using MySQLService.Services;

namespace MySQLService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadoConsultaController : ControllerBase
    {
        private readonly EstadoConsultaRepository _repository;
        private readonly EstadoConsultaRabbitMqService _rabbitMqService;

        public EstadoConsultaController(EstadoConsultaRepository repository, EstadoConsultaRabbitMqService rabbitMqService)
        {
            _repository = repository;
            _rabbitMqService = rabbitMqService;
        }

        [HttpPost]
        public async Task<ActionResult<EstadoConsultaEntity>> Create([FromBody] EstadoConsultaEntity estadoConsulta)
        {
            var createdEstado = await _repository.AddAsync(estadoConsulta);

            // Publicar mensaje a RabbitMQ
            _rabbitMqService.PublishMessage(createdEstado);

            return CreatedAtAction(nameof(Create), new { id = createdEstado.IdEstadoConsulta }, createdEstado);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] EstadoConsultaEntity estadoConsulta)
        {
            if (id != estadoConsulta.IdEstadoConsulta) return BadRequest();

            var updatedEstado = await _repository.UpdateAsync(estadoConsulta);
            if (updatedEstado == null) return NotFound();

            // Publicar mensaje a RabbitMQ
            _rabbitMqService.PublishMessage(updatedEstado);

            return Ok(updatedEstado);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _repository.DeleteAsync(id);
            if (!success) return NotFound();

            // Publicar mensaje a RabbitMQ
            _rabbitMqService.PublishMessage(new { IdEstadoConsulta = id, Action = "Deleted" });

            return NoContent();
        }
    }
}
