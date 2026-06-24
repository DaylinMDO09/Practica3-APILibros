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
    }
}
