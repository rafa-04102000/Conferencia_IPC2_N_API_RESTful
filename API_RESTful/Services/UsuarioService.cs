using API_RESTful.Models;
using API_RESTful.Repositories;

using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API_RESTful.Services
{
    public class UsuarioService
    {
        // Variable privada de solo lectura para guardar nuestro repositorio
        private readonly RepositorioTienda _repositorio;

        // CONSTRUCTOR: ¡Aquí ocurre la Inyección de Dependencias!
        public UsuarioService(RepositorioTienda repositorio)
        {
            _repositorio = repositorio;
        }

        // Validar y crear un usuario
        public void CrearUsuario(Usuario nuevoUsuario)
        {

            // Lógica de negocio: Validar que el correo no exista ya
            var existe = _repositorio.ObtenerUsuarios()
                                     .Any(u => u.Correo == nuevoUsuario.Correo);

            if (existe)
            {
                throw new Exception("Error: El correo ya está registrado.");
            }

            // Logica de negocio 2: Validar que los productos asociados existan en el catálogo global
            if (nuevoUsuario.Productos != null && nuevoUsuario.Productos.Count > 0)
            {
                foreach (var producto in nuevoUsuario.Productos)
                {
                    // Si el producto no existe en el catálogo global, disparamos el error
                    if (!_repositorio.ExisteProducto(producto.Nombre))
                    {
                        throw new Exception($"El producto '{producto.Nombre}' no existe. Por favor, créelo primero en el catálogo.");
                    }
                }
            }

            // Si todo está bien, le decimos al repositorio que lo guarde
            _repositorio.AgregarUsuario(nuevoUsuario);
        }

        // Obtener la lista de usuarios
        public List<Usuario> ObtenerUsuarios() => _repositorio.ObtenerUsuarios();


        // Metodo para iniciar sesión y obtener un JWT
        public string IniciarSesion(string correo, string password)
        {
            // 1. Buscamos al usuario en la base de datos (Repositorio)
            var usuario = _repositorio.ObtenerUsuarios()
                .FirstOrDefault(u => u.Correo == correo && u.Password == password);

            if (usuario == null)
            {
                throw new Exception("Error: Credenciales incorrectas.");
            }

            // 2. Si existe, fabricamos y firmamos el JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var claveSecreta = Encoding.ASCII.GetBytes("EstaEsUnaClaveSuperSecretaParaMiConferenciaIPC2!!");

            // Aquí guardamos los datos públicos (Claims) DENTRO del Payload
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.Email, usuario.Correo),
            new Claim(ClaimTypes.Name, usuario.Nombre)
        }),
                Expires = DateTime.UtcNow.AddHours(1), // El pasaporte caduca en 1 hora
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(claveSecreta), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token); // Retorna el token fabricado como un string
        }
    }
}
