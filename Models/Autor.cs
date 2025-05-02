using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace LibreriaWeb.Models
{
    public class Autor
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        // Relación 1-N
        [ValidateNever] // Esto evita que se valide campo libro al crear autor
        public ICollection<Libro> Libros { get; set; }
    }
}
