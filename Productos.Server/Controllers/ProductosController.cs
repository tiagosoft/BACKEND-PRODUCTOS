using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Productos.Server.Models;

namespace Productos.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly ProductosContext _context;
        private readonly IConfiguration _configuration;


        public ProductosController(ProductosContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;

        }

        [HttpPost]
        [Route("crearProducto")]
        public async Task<IActionResult> CreateProduct(Producto producto)
        {
            //guardar el producto en la base de datos
            await _context.Productos.AddAsync(producto);
            await _context.SaveChangesAsync();

            //devolver un mensaje de exito
            return Ok();
        }

        [HttpGet]
        [Route("listaProducto")]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProducts()
        {
            //Obten la lista de productos de la base de datos
            var productos = await _context.Productos.ToListAsync();

            //devuelve una lista de productos
            return Ok(productos);
        }


        [HttpGet]
        [Route("verProducto")]
        public async Task<IActionResult> GetProduct(int id)
        {
            //obtener el producto de la base de datos
            Producto producto = await _context.Productos.FindAsync(id);

            //devolver el producto
            if (producto == null)
            {
                return NotFound();
            }

            return Ok(producto);
        }

        [HttpPut]
        [Route("editarProducto")]
        public async Task<IActionResult> UpdateProduct(int id, Producto producto)
        {
            //Actualizar el producto en la base de datos
            var productoExistente = await _context.Productos.FindAsync(id);
            productoExistente!.Nombre = producto.Nombre;
            productoExistente.Descripcion = producto.Descripcion;
            productoExistente.Precio = producto.Precio;

            await _context.SaveChangesAsync();

            //devolver un mensaje de exito
            return Ok();
        }

        [HttpDelete]
        [Route("eliminarProducto")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            //Eliminar producto de la base de datos
            var productoBorrado = await _context.Productos.FindAsync(id);
            _context.Productos.Remove(productoBorrado!);

            await _context.SaveChangesAsync();

            //Devolver un mensaje de exito
            return Ok();
        }

        [HttpGet]
        [Route("productosPorCategoria/{categoriaId}")]
        public async Task<IActionResult> GetProductosPorCategoria(int categoriaId)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            var productosConCategoria = new List<dynamic>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                string sql = @"
            SELECT p.Id AS ProductoId, p.Nombre AS ProductoNombre, p.Descripcion, p.Precio,
                   c.Id AS CategoriaId, c.Nombre AS CategoriaNombre, c.Descripcion AS CategoriaDescripcion
            FROM Productos p
            INNER JOIN Categorias c ON p.CategoriaId = c.Id
            WHERE c.Id = @CategoriaId
        ";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    // Agregar parámetro
                    command.Parameters.AddWithValue("@CategoriaId", categoriaId);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            productosConCategoria.Add(new
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("ProductoId")),
                                Nombre = reader.GetString(reader.GetOrdinal("ProductoNombre")),
                                Descripcion = reader.GetString(reader.GetOrdinal("Descripcion")),
                                Precio = reader.GetDecimal(reader.GetOrdinal("Precio")),
                                Categoria = new
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("CategoriaId")),
                                    Nombre = reader.GetString(reader.GetOrdinal("CategoriaNombre")),
                                    Descripcion = reader.GetString(reader.GetOrdinal("CategoriaDescripcion"))
                                }
                            });
                        }
                    }
                }
            }

            return Ok(productosConCategoria);
        }

    }
}
