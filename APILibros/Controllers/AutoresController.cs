using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APILibros.Modelos;
using APILibros.Key;

namespace APILibros.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiKey]
    public class AutoresController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AutoresController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("ObtenerAutores")]
        public async Task<IActionResult> ObtenerAutores()
        {
            var autores = await _context.Autor
                .AsNoTracking()
                .ToListAsync();

            return Ok(autores);
        }

        [HttpGet("ObtenerAutorPorId/{id:int}")]
        public async Task<IActionResult> ObtenerAutorPorId(int id)
        {
            var autor = await _context.Autor
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.IdAutor == id);

            if (autor == null)
            {
                return NotFound(new
                {
                    msg = $"No se encontró el autor con Id {id}."
                });
            }

            return Ok(autor);
        }

        [HttpGet("{id:int}/libros")]
        public async Task<ActionResult<IEnumerable<LibrosModel>>> ObtenerLibrosDeAutor(int id)
        {
            var autorExiste = await _context.Autor
                .AnyAsync(a => a.IdAutor == id);

            if (!autorExiste)
            {
                return NotFound(new
                {
                    msg = $"No se encontró el autor con Id {id}."
                });
            }

            var libros = await _context.Libros
                .AsNoTracking()
                .Where(l => l.IdAutor == id)
                .ToListAsync();

            if (libros == null || libros.Count == 0)
            {
                return Ok(new
                {
                    msg = "El autor no tiene libros registrados.",
                    libros = new List<LibrosModel>()
                });
            }

            return Ok(libros);
        }

        [HttpPost("AgregarAutor")]
        public async Task<IActionResult> AgregarAutor([FromBody] AutorModel autor)
        {
            if (autor == null)
            {
                return BadRequest(new
                {
                    msg = "Debe enviar los datos del autor."
                });
            }

            _context.Autor.Add(autor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(ObtenerAutorPorId),
                new { id = autor.IdAutor },
                autor
            );
        }

        [HttpPut("ActualizarAutor/{id:int}")]
        public async Task<IActionResult> ActualizarAutor(
            int id,
            [FromBody] AutorModel autorActualizado)
        {
            if (autorActualizado == null)
            {
                return BadRequest(new
                {
                    msg = "Debe enviar los datos actualizados del autor."
                });
            }

            var autorExistente = await _context.Autor
                .FirstOrDefaultAsync(a => a.IdAutor == id);

            if (autorExistente == null)
            {
                return NotFound(new
                {
                    msg = $"No se encontró el autor con Id {id}."
                });
            }

            autorExistente.Nombre = autorActualizado.Nombre;
            autorExistente.Nacionalidad = autorActualizado.Nacionalidad;
            autorExistente.AnioNacimiento = autorActualizado.AnioNacimiento;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("EliminarAutor/{id:int}")]
        public async Task<IActionResult> EliminarAutor(int id)
        {
            var autorExistente = await _context.Autor
                .FirstOrDefaultAsync(a => a.IdAutor == id);

            if (autorExistente == null)
            {
                return NotFound(new
                {
                    msg = $"No se encontró el autor con Id {id}."
                });
            }

            _context.Autor.Remove(autorExistente);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}