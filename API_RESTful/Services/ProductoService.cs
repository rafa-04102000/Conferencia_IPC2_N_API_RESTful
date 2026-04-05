using API_RESTful.Models;
using API_RESTful.Repositories;

namespace API_RESTful.Services
{
    public class ProductoService
    {
        private readonly RepositorioTienda _repositorio;

        // Inyección de dependencias
        public ProductoService(RepositorioTienda repositorio)
        {
            _repositorio = repositorio;
        }

        // Método para listar todos los productos
        public List<Producto> ObtenerTodos()
        {
            return _repositorio.ObtenerProductos();
        }

        // Método para crear un nuevo producto con validaciones
        public void AgregarProducto(Producto nuevoProducto)
        {
            // Regla de Negocio 1: El nombre no puede estar vacío
            if (string.IsNullOrWhiteSpace(nuevoProducto.Nombre))
            {
                throw new Exception("Error: El nombre del producto es obligatorio.");
            }

            // Regla de Negocio 2: Lógica de precios
            if (nuevoProducto.Precio <= 0)
            {
                throw new Exception("Error: El precio del producto debe ser mayor a Q0.00.");
            }

            // Regla de Negocio 3: No permitir productos con el mismo nombre
            if (_repositorio.ExisteProducto(nuevoProducto.Nombre))
            {
                throw new Exception($"El producto '{nuevoProducto.Nombre}' ya existe en el catálogo.");
            }

            // Si pasa todas las validaciones (filtros de calidad), lo guardamos
            _repositorio.AgregarProducto(nuevoProducto);
        }
    }
}
