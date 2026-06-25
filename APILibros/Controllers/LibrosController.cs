using Microsoft.AspNetCore.Mvc;
using APILibros.Modelos;

namespace APILibros.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LibrosController : Controller
    {
        private readonly AppDbContext _context;

        public LibrosController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet("ObtenerLibros")]
        public IActionResult ObtenerLibros()
        {
            var libros = _context.Libros.ToList();
            return Ok(libros);
        }
        [HttpGet("ObtenerLibroPorId/{id}")]
        public IActionResult ObtenerLibroPorId(int id)
        {
            var libro = _context.Libros.FirstOrDefault(l => l.IdLibro == id);
            if (libro == null)
            {
                return NotFound();
            }
            return Ok(libro);
        }
        [HttpPost("AgregarLibro")]
        public IActionResult AgregarLibro([FromBody] LibrosModel libro)
        {
            if (libro == null)
            {
                return BadRequest();
            }
            _context.Libros.Add(libro);
            _context.SaveChanges();
            return CreatedAtAction(nameof(ObtenerLibroPorId), new { id = libro.IdLibro }, libro);
        }
        [HttpPut("ActualizarLibro/{id}")]
        public IActionResult ActualizarLibro(int id, [FromBody] LibrosModel libroActualizado)
        {
            var libroExistente = _context.Libros.FirstOrDefault(l => l.IdLibro == id);
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
            _context.SaveChanges();
            return NoContent();
        }
        [HttpDelete("EliminarLibro/{id}")]
        public IActionResult EliminarLibro(int id)
        {
            var libroExistente = _context.Libros.FirstOrDefault(l => l.IdLibro == id);
            if (libroExistente == null)
            {
                return NotFound();
            }
            _context.Libros.Remove(libroExistente);
            _context.SaveChanges();
            return NoContent();
        }
        }
}
