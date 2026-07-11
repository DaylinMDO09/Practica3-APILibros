using Microsoft.AspNetCore.Mvc;
using APILibros.Modelos;
using APILibros.Key;
using Microsoft.EntityFrameworkCore;

namespace APILibros.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiKey]
    public class LibrosController : Controller
    {
        private readonly AppDbContext _context;

        public LibrosController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet("ObtenerLibros")]
        public async Task<IActionResult> ObtenerLibros()
        {
            var libros = await _context.Libros.ToListAsync();
            return Ok(libros);
        }
        [HttpGet("ObtenerLibroPorId/{id}")]
        public async Task<IActionResult> ObtenerLibroPorId(int id)
        {
            var libro = await _context.Libros.FirstOrDefaultAsync(l => l.IdLibro == id);
            if (libro == null)
            {
                return NotFound();
            }
            return Ok(libro);
        }
        [HttpGet("ObtenerLibrosPorPaginas")]
        public async Task<IActionResult> ObtenerLibrosPorPaginas(int pagina, int tamanioPagina)
        {
            var libros = await _context.Libros
                .Skip((pagina - 1) * tamanioPagina)
                .Take(tamanioPagina)
                .ToListAsync();
            return Ok(libros);
        }
        [HttpPost("AgregarLibro")]
        public async Task<IActionResult> AgregarLibro([FromBody] LibrosModel libro)
        {
            if (libro == null)
            {
                return BadRequest();
            }
            _context.Libros.Add(libro);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(ObtenerLibroPorId), new { id = libro.IdLibro }, libro);
        }
        [HttpPut("ActualizarLibro/{id}")]
        public async Task<IActionResult> ActualizarLibro(int id, [FromBody] LibrosModel libroActualizado)
        {
            var libroExistente = await _context.Libros.FirstOrDefaultAsync(l => l.IdLibro == id);
            if (libroExistente == null)
            {
                return NotFound();
            }
            libroExistente.Titulo = libroActualizado.Titulo;
            libroExistente.Autor = libroActualizado.Autor;
            libroExistente.AnioPublicacion = libroActualizado.AnioPublicacion;
            libroExistente.Genero = libroActualizado.Genero;
            libroExistente.NumeroPaginas = libroActualizado.NumeroPaginas;
            libroExistente.Precio = libroActualizado.Precio;
            libroExistente.Disponibilidad = libroActualizado.Disponibilidad;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("EliminarLibro/{id}")]
        public async Task<IActionResult> EliminarLibro(int id)
        {
            var libroExistente = await _context.Libros.FirstOrDefaultAsync(l => l.IdLibro == id);
            if (libroExistente == null)
            {
                return NotFound();
            }
            _context.Libros.Remove(libroExistente);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
