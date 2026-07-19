using EstadisticasAPI.Data;
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
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(usuario.PasswordHash);
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, usuario);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
        {
            if (id != usuario.Id) return BadRequest();
            _context.Entry(usuario).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound();
            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
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