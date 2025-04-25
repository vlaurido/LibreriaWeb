using System.ComponentModel.DataAnnotations;

namespace LibreriaWeb.Models
{
    public class Libro
    {
        public int Id { get; set; }
        public string Titulo { get; set; }

        [Display(Name = "Año de publicación")]
        public int AnioPublicacion { get; set; }

        // FK y navegación
        [Display(Name = "Autor")]
        public int AutorId { get; set; }
        public Autor Autor { get; set; }
    }
}
