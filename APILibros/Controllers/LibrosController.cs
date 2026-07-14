using Microsoft.AspNetCore.Mvc;
using APILibros.Modelos;
using APILibros.Key;
using Microsoft.EntityFrameworkCore;

namespace APILibros.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiKey]
    public class LibrosController : ControllerBase
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
        public async Task<IActionResult> ObtenerLibrosPaginados(int pagina = 1,int tamanoPagina = 10,string? buscar = null,string? ordenarPor = "Precio",string direccion = "asc")
        {
            if (pagina <= 0 || tamanoPagina <= 0)
            {
                return BadRequest(new { msg = "Los parámetros de paginación deben ser mayores a cero." });
            }

            var query = _context.Libros.AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(buscar))
            {
                query = query.Where(l => l.Titulo.Contains(buscar));
            }

            query = direccion.ToLower() == "desc"
                ? query.OrderByDescending(e => EF.Property<object>(e, ordenarPor))
                : query.OrderBy(e => EF.Property<object>(e, ordenarPor));
                     
            var totalRegistros = await query.CountAsync();
            var libros = await query
                .Skip((pagina - 1) * tamanoPagina)
                .Take(tamanoPagina)
                .ToListAsync();

            var resultado = new
            {
                pagina,
                tamanoPagina,
                totalRegistros,
                totalPaginas = (int)Math.Ceiling((double)totalRegistros / tamanoPagina),
                datos = libros
            };

            return Ok(resultado);
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
            libroExistente.IdAutor = libroActualizado.IdAutor;
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
