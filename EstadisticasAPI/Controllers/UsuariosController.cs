using EstadisticasAPI.Data;
using EstadisticasAPI.Helpers;
using EstadisticasAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EstadisticasAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsuarios()
        {
            var usuarios = await _context.Usuarios
                .Select(u => new
                {
                    id = u.Id,
                    nombreUsuario = u.NombreUsuario,
                    email = u.Email,
                    rol = u.Rol,
                    fechaRegistro = u.FechaRegistro,
                    activo = u.Activo
                })
                .ToListAsync();
            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios
                .Where(u => u.Id == id)
                .Select(u => new
                {
                    id = u.Id,
                    nombreUsuario = u.NombreUsuario,
                    email = u.Email,
                    rol = u.Rol,
                    fechaRegistro = u.FechaRegistro,
                    activo = u.Activo
                })
                .FirstOrDefaultAsync();
            if (usuario == null) return NotFound();
            return Ok(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> PostUsuario(Usuario usuario)
        {
            if (await _context.Usuarios.AnyAsync(u => u.Email == usuario.Email))
                return BadRequest(new { mensaje = "El email ya está registrado." });

            if (await _context.Usuarios.AnyAsync(u => u.NombreUsuario == usuario.NombreUsuario))
                return BadRequest(new { mensaje = "El nombre de usuario ya existe." });

            usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(usuario.PasswordHash);
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, usuario);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
        {
            var existente = await _context.Usuarios.FindAsync(id);
            if (existente == null) return NotFound();

            // Solo se actualizan Rol y Activo — nunca tocar PasswordHash/Email/NombreUsuario
            // desde aquí, porque el llamador (AdminFrontend) no envía esos campos.
            existente.Rol = usuario.Rol;
            existente.Activo = usuario.Activo;

            await _context.SaveChangesAsync();
            await AuditoriaHelper.RegistrarAsync(_context, Request, "ACTUALIZAR_USUARIO",
                $"Usuario #{id} — rol: {existente.Rol}, activo: {existente.Activo}");
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound();
            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            await AuditoriaHelper.RegistrarAsync(_context, Request, "ELIMINAR_USUARIO", $"Usuario #{id} eliminado");
            return NoContent();
        }

        [HttpGet("por-rol/{rol}")]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetPorRol(string rol)
        {
            return await _context.Usuarios
                .Where(u => u.Rol == rol && u.Activo)
                .OrderBy(u => u.NombreUsuario)
                .ToListAsync();
        }

        [HttpPost("cambiar-password")]
        public async Task<IActionResult> CambiarPassword([FromBody] CambiarPasswordDto dto)
        {
            var usuario = await _context.Usuarios.FindAsync(dto.Id);
            if (usuario == null) return NotFound();
            usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NuevaPassword);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        public class CambiarPasswordDto
        {
            public int Id { get; set; }
            public string NuevaPassword { get; set; } = string.Empty;
        }
    }
}