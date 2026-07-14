using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APILibros.Modelos
{
    public class LibrosModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdLibro { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 1)]
        public string? Titulo { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int IdAutor { get; set; }

        [Required]
        [Range(1900, 2100)]
        public int AnioPublicacion { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string? Genero { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int? NumeroPaginas { get; set; }

        [Required]
        [Range(0.0, double.MaxValue)]
        public Decimal Precio { get; set; } = 0.0m;
        
        public bool Disponibilidad { get; set; } = true;
    }
}
