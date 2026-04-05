using API_RESTful.Models;
using API_RESTful.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_RESTful.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly ProductoService _servicio;

        // 2. Inyección de Dependencias: Pedimos el Servicio
        public ProductosController(ProductoService servicio)
        {
            _servicio = servicio;
        }

        // 3. Endpoint GET: Para leer datos
        [HttpGet("listar")] // URL: api/Productos/listar
        public IActionResult GetProductos()
        {
            var productos = _servicio.ObtenerTodos();
            return Ok(productos); // Devuelve un HTTP 200 OK con la lista en formato JSON
        }

        // 4. Endpoint POST: Para enviar datos
        [HttpPost("crear")] // URL: api/Productos/crear
        public IActionResult PostProducto([FromBody] Producto nuevoProducto)
        {
            try
            {
                // Le pasamos el producto al Servicio para que aplique las reglas de negocio
                _servicio.AgregarProducto(nuevoProducto);

                return Ok(new { Mensaje = "Producto creado exitosamente" });
            }
            catch (Exception ex)
            {
                // Si el Servicio lanza un error (ej. precio negativo), devolvemos un 400 Bad Request
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpGet("~/isalive")]
        // [HttpGet("/isalive")] <-- Esto hace exactamente lo mismo
        [Tags("Monitoreo")] // Esto es solo para organizar mejor la documentación en Swagger, lo separa de esta coleccion de endpoints
        public IActionResult IsAlive()
        {
            return Ok("API está viva y funcionando correctamente.");
        }
    }
}
