namespace Productos.Server.Models
{
    public class Categoria
    {
        public int Id { get; set; }              // Clave primaria
        public string Nombre { get; set; }       // Nombre de la categoría
        public string Descripcion { get; set; }  // Opcional
    }
}
