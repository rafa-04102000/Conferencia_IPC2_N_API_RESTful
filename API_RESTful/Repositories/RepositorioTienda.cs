using API_RESTful.Models;

namespace API_RESTful.Repositories
{
    public class RepositorioTienda
    {
        // Estas listas actúan como nuestras "tablas" de base de datos
        private readonly List<Usuario> _usuarios = new List<Usuario>();
        private readonly List<Producto> _productos = new List<Producto>();

        // Métodos para gestionar Usuarios
        public List<Usuario> ObtenerUsuarios() => _usuarios;

        public void AgregarUsuario(Usuario usuario)
        {
            // Lógica simple para auto-incrementar el ID
            usuario.Id = _usuarios.Count + 1;
            _usuarios.Add(usuario);
        }

        // Métodos para gestionar Productos
        public List<Producto> ObtenerProductos() => _productos;

        public void AgregarProducto(Producto producto)
        {
            producto.Id = _productos.Count + 1;
            _productos.Add(producto);
        }

        public bool ExisteProducto(string nombre)
        {
            // esto devuelve un booleano, true si encuentra un producto con ese nombre, false si no lo encuentra
            // el .Any() es un método de LINQ que devuelve true si al menos un elemento de la colección
            // cumple la condición dada, y false si ningún elemento cumple la condición.
            return _productos.Any(p => p.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase));
        }
    }
}
