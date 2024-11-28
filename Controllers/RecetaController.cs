using CQRS_Un_Proyecto.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using MySQLService.Repositories;
using MySQLService.Services;

namespace MySQLService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecetaController : ControllerBase
    {
        private readonly RecetaRepository _repository;
        private readonly RecetaRabbitMqService _rabbitMqService;

        public RecetaController(RecetaRepository repository, RecetaRabbitMqService rabbitMqService)
        {
            _repository = repository;
            _rabbitMqService = rabbitMqService;
        }

        [HttpPost]
        public async Task<ActionResult<RecetaEntity>> Create([FromBody] RecetaEntity receta)
        {
            var createdReceta = await _repository.AddAsync(receta);

            // Publicar mensaje a RabbitMQ
            _rabbitMqService.PublishMessage(createdReceta);

            return CreatedAtAction(nameof(Create), new { id = createdReceta.IdReceta }, createdReceta);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] RecetaEntity receta)
        {
            if (id != receta.IdReceta) return BadRequest();

            var updatedReceta = await _repository.UpdateAsync(receta);
            if (updatedReceta == null) return NotFound();

            // Publicar mensaje a RabbitMQ
            _rabbitMqService.PublishMessage(updatedReceta);

            return Ok(updatedReceta);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _repository.DeleteAsync(id);
            if (!success) return NotFound();

            // Publicar mensaje a RabbitMQ
            _rabbitMqService.PublishMessage(new { IdReceta = id, Action = "Deleted" });

            return NoContent();
        }
    }
}
