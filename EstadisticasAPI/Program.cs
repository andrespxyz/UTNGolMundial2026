using EstadisticasAPI.Data;
using EstadisticasAPI.Services;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddScoped<PartidoResultadoService>();
builder.Services.AddSingleton<JwtService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "UTN GolMundial 2026 — Estadísticas API",
        Version = "v1",
        Description = "API REST para gestión de estadísticas del Mundial de Fútbol 2026. Maneja selecciones, sedes, partidos, resultados y usuarios.",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Universidad Técnica del Norte",
            Email = "info@utn.edu.ec"
        }
    });
}); 

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
//prueba
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();
app.Run();
