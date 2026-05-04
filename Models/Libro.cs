using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using LibreriaWeb.Models.Validations;

namespace LibreriaWeb.Models
{
    public class Libro
    {
        public int Id { get; set; }
        public string Titulo { get; set; }

        [Display(Name = "Año de publicación")]
        [NoFuture(ErrorMessage = "El año de publicación no puede ser mayor al año actual.")]
        public int AnioPublicacion { get; set; }

        // FK y navegación
        [Display(Name = "Autor")]
        public int AutorId { get; set; }
        [ForeignKey("AutorId")]
        [ValidateNever]
        public Autor Autor { get; set; }
        [Display(Name = "Portada")]
        public string? ImagenRuta { get; set; }
        [NotMapped]
        public IFormFile? ImagenArchivo { get; set; }
    }
}
