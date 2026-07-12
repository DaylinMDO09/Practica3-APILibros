using Microsoft.AspNetCore.Mvc;
using APILibros.Modelos;
using APILibros.Key;
using Microsoft.EntityFrameworkCore;

namespace APILibros.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiKey]
    public class AutoresController : Controller
    {
        private readonly AppDbContext _context;

        public AutoresController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("ObtenerAutores")]
        public async Task<IActionResult> ObtenerAutores()
        {
            var autores = await _context.Autor.ToListAsync();
            return Ok(autores);
        }

        [HttpGet("ObtenerAutorPorId/{id}")]
        public async Task<IActionResult> ObtenerAutorPorId(int id)
        {
            var autor = await _context.Autor.FirstOrDefaultAsync(a => a.IdAutor == id);
            if (autor == null)
            {
                return NotFound();
            }
            return Ok(autor);
        }

        [HttpPost("AgregarAutor")]
        public async Task<IActionResult> AgregarAutor([FromBody] AutorModel autor)
        {
            if (autor == null)
            {
                return BadRequest();
            }
            _context.Autor.Add(autor);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(ObtenerAutorPorId), new { id = autor.IdAutor }, autor);
        }

        [HttpPut("ActualizarAutor/{id}")]
        public async Task<IActionResult> ActualizarAutor(int id, [FromBody] AutorModel autorActualizado)
        {
            var autorExistente = await _context.Autor.FirstOrDefaultAsync(a => a.IdAutor == id);
            if (autorExistente == null)
            {
                return NotFound();
            }
            autorExistente.Nombre = autorActualizado.Nombre;
            autorExistente.Nacionalidad = autorActualizado.Nacionalidad;
            autorExistente.AnioNacimiento = autorActualizado.AnioNacimiento;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("EliminarAutor/{id}")]
        public async Task<IActionResult> EliminarAutor(int id)
        {
            var autorExistente = await _context.Autor.FirstOrDefaultAsync(a => a.IdAutor == id);
            if (autorExistente == null)
            {
                return NotFound();
            }
            _context.Autor.Remove(autorExistente);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
