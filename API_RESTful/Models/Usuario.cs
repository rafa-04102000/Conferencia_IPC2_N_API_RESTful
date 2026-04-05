namespace API_RESTful.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty; // ¡Recuerda: nunca devolver esto en el token!

        // Relación: Un usuario puede tener muchos productos
        // Pueder ser null si el usuario no tiene productos asociados,
        // por eso el tipo es List<Producto>? y se inicializa con una nueva lista vacía.
        public List<Producto>? Productos { get; set; } = new List<Producto>();
    }
}
