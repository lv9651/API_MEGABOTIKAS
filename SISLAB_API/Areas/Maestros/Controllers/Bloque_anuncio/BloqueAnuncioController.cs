using Microsoft.AspNetCore.Mvc;
using SISLAB_API.Areas.Maestros.Models;

using SISLAB_API.Areas.Maestros.Services;


namespace SISLAB_API.Areas.Maestros.Controllers 
{
    [Route("api/[controller]")]
    [ApiController]
    public class BloqueAnuncioController : ControllerBase
    {
        private readonly BloqueAnuncioService _bloqueAnuncioService;

        // Inject the service
        public BloqueAnuncioController(BloqueAnuncioService bloqueAnuncioService)
        {
            _bloqueAnuncioService = bloqueAnuncioService;
        }

        // Example: Create a new block
        [HttpPost]
        public async Task<IActionResult> CreateBlock([FromBody] BloqueAnuncio bloqueAnuncio)
        {
            var rowsAffected = await _bloqueAnuncioService.CreateAsync(bloqueAnuncio);
            if (rowsAffected > 0)
            {
                return Ok("Bloque creado exitosamente.");
            }
            return BadRequest("Error al crear el bloque.");
        }

        // Example: Update an existing block
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBlock(int id, [FromBody] BloqueAnuncio bloqueAnuncio)
        {
            bloqueAnuncio.Id = id;
            var rowsAffected = await _bloqueAnuncioService.UpdateAsync(bloqueAnuncio);
            if (rowsAffected > 0)
            {
                return Ok("Bloque actualizado exitosamente.");
            }
            return BadRequest("Error al actualizar el bloque.");
        }

        // Example: Delete a block
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlock(int id)
        {
            var rowsAffected = await _bloqueAnuncioService.DeleteAsync(id);
            if (rowsAffected > 0)
            {
                return Ok("Bloque eliminado exitosamente.");
            }
            return BadRequest("Error al eliminar el bloque.");
        }

        // Example: Get a block by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBlock(int id)
        {
            var bloque = await _bloqueAnuncioService.GetByIdAsync(id);
            if (bloque == null)
            {
                return NotFound("Bloque no encontrado.");
            }
            return Ok(bloque);
        }

        // Example: Get all blocks
        [HttpGet]
        public async Task<IActionResult> GetAllBlocks()
        {
            var bloques = await _bloqueAnuncioService.GetAllAsync();
            return Ok(bloques);
        }
    }
}