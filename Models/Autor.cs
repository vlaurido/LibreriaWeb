namespace LibreriaWeb.Models
{
    public class Autor
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        // Relación 1-N
        public ICollection<Libro> Libros { get; set; }
    }
}
