using Microsoft.Build.Evaluation;

namespace PL.Models
{
    public class Pelicula
    {
        public int IdMovie { get; set; }
        public string Descripcion { get; set; }
        public string Nombre { get; set; }
        public string Fecha { get; set; }
        public string Imagen { get; set; }
        public List<object> Peliculas { get; set; }
    }
}
