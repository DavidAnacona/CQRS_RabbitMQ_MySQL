using CQRS_Rabbit_MySQL.Services;
using Microsoft.AspNetCore.Mvc;
using MySQLService.Repositories;

namespace MySQLService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultaController : ControllerBase
    {
        private readonly ConsultaRepository _repository;
        private readonly RabbitMqService _rabbitMqService;

        public ConsultaController(ConsultaRepository repository, RabbitMqService rabbitMqService)
        {
            _repository = repository;
            _rabbitMqService = rabbitMqService;
        }

        [HttpPost]
        public async Task<ActionResult<ConsultaEntity>> Create([FromBody] ConsultaEntity consulta)
        {
            var createdConsulta = await _repository.AddAsync(consulta);

            // Publicar mensaje a RabbitMQ
            _rabbitMqService.PublishMessage(createdConsulta);

            return CreatedAtAction(nameof(Create), new { id = createdConsulta.IdConsulta }, createdConsulta);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ConsultaEntity consulta)
        {
            if (id != consulta.IdConsulta) return BadRequest();

            var updatedConsulta = await _repository.UpdateAsync(consulta);
            if (updatedConsulta == null) return NotFound();

            // Publicar mensaje a RabbitMQ
            _rabbitMqService.PublishMessage(updatedConsulta);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _repository.DeleteAsync(id);
            if (!success) return NotFound();

            // Publicar mensaje a RabbitMQ
            _rabbitMqService.PublishMessage(new { IdConsulta = id, Action = "Deleted" });

            return NoContent();
        }
    }
}
