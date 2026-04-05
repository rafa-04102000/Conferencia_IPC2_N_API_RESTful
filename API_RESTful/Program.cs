using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();
// Agregamos el generador de Swagger (Reemplaza a AddOpenApi)
builder.Services.AddSwaggerGen();

// ---> CONFIGURACIÓN DEL TOKEN JWT <---
var claveSecreta = Encoding.ASCII.GetBytes("EstaEsUnaClaveSuperSecretaParaMiConferenciaIPC2!!");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opciones =>
    {
        opciones.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(claveSecreta),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

// Registrar el Repositorio como una instancia única global (Singleton)
builder.Services.AddSingleton<API_RESTful.Repositories.RepositorioTienda>();

// 2. Registro de los "Cerebros" (Scoped: 1 por cada petición HTTP)
builder.Services.AddScoped<API_RESTful.Services.UsuarioService>();
builder.Services.AddScoped<API_RESTful.Services.ProductoService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    // Activamos la interfaz gráfica de Swagger solo si estamos en modo Desarrollo
    app.UseSwagger();
    app.UseSwaggerUI();

}

// ---> MUY IMPORTANTE: Orden de los Middlewares <---
app.UseAuthentication(); // 1. ¿Quién eres? (Revisa el token)

app.UseHttpsRedirection();

app.UseAuthorization();  // 2. ¿Puedes pasar? (Revisa si la ruta tiene candado)

app.MapControllers();

app.Run();
