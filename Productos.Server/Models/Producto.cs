using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Productos.Server.Models
{
    public class Producto
    {
        public int Id { get; set; }
        [MaxLength(50,ErrorMessage = "El campo {0} debe tener maximo {1} caracteres")]
        public string Nombre { get; set; } = null!;//no permite nulos
        [DataType(DataType.MultilineText)]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener maximo {1} caracteres")]
        public string Descripcion { get; set; } = null!;//no permite nulos
        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:C2}")]//formato moneda
        public decimal Precio { get; set; }

        // Nueva columna
        public int CategoriaId { get; set; }

    }
}
