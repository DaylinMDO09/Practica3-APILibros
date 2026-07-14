using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APILibros.Modelos
{
    public class AutorModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdAutor { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string? Nombre { get; set; }

        [Required]
        [StringLength(50)]
        public string? Nacionalidad { get; set; }

        [Range (1900, 2100)]
        public int AnioNacimiento { get; set; }
    }
}
