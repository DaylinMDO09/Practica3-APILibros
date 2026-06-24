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
        [MaxLength(200)]
        public string? Titulo { get; set; }

        [Required]
        [MaxLength(200)]
        public string? Autor { get; set; }

        [Required]
        public DateTime AnioPublicacion { get; set; } = DateTime.MinValue;

        [MaxLength(10)]
        public string? Genero { get; set; }

        public int? NumeroPaginas { get; set; }

        [Required]
        public Decimal Precio { get; set; } = 0.0m;
        [Required]
        public bool Disponibilidad { get; set; } = true;
    }
}
