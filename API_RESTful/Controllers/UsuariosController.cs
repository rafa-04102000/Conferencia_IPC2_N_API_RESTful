using API_RESTful.Models;
using API_RESTful.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API_RESTful.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuarioService _servicio;

        public UsuariosController(UsuarioService servicio)
        {
            _servicio = servicio;
        }

        [HttpGet("listar")]
        public IActionResult GetUsuarios()
        {
            var usuarios = _servicio.ObtenerUsuarios(); 
            return Ok(usuarios);
        }

        [HttpPost("crear")]
        public IActionResult PostUsuario([FromBody] Usuario nuevoUsuario)
        {
            try
            {
                _servicio.CrearUsuario(nuevoUsuario);
                return Ok(new { Mensaje = "Usuario registrado exitosamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }


        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            try
            {
                var token = _servicio.IniciarSesion(request.Correo, request.Password);
                // Devolvemos el JWT al frontend
                return Ok(new { Token = token, Mensaje = "Login exitoso" });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { Error = ex.Message }); // HTTP 401
            }
        }

        // ---> ESTE ES EL ENDPOINT PROTEGIDO <---
        [Authorize] // ¡El candado! Sin token, Postman recibe un 401 Unauthorized
        [HttpGet("mi-perfil")]
        public IActionResult VerMiPerfil()
        {
            // .NET extrae mágicamente los datos del token válido y los pone en "User"
            var correoUsuario = User.FindFirstValue(ClaimTypes.Email);
            var nombreUsuario = User.FindFirstValue(ClaimTypes.Name);

            return Ok(new
            {
                Mensaje = "¡Lograste entrar a la zona segura!",
                Datos = $"Hola {nombreUsuario}, tu correo es {correoUsuario}"
            });
        }
    }
}
