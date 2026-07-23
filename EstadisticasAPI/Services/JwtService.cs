using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace EstadisticasAPI.Services
{
    // Emite los JWT (HS256) que UTNGolCoinAPI valida en su filtro JAX-RS para
    // proteger sus rutas (RF25). La clave secreta es compartida entre ambos
    // backends vía configuración (appsettings.json aquí, constante allá).
    public class JwtService
    {
        private readonly string _secretKey;
        private readonly string _issuer;

        public JwtService(IConfiguration configuration)
        {
            _secretKey = configuration["Jwt:SecretKey"]
                ?? throw new InvalidOperationException("Falta Jwt:SecretKey en appsettings.json");
            _issuer = configuration["Jwt:Issuer"] ?? "EstadisticasAPI";
        }

        // Token de usuario: se emite en login/registro y viaja con el usuario
        // en cada llamada a UTNGolCoinAPI (billetera, predicciones).
        public string GenerarTokenUsuario(int usuarioId, string rol)
        {
            return GenerarToken(usuarioId.ToString(), rol, TimeSpan.FromHours(4));
        }

        // Token de servicio: lo usa EstadisticasAPI para autenticarse a sí misma
        // frente a UTNGolCoinAPI al notificar la liquidación de un resultado
        // (no hay un usuario/admin "conectado" en ese momento, es server-to-server).
        public string GenerarTokenSistema()
        {
            return GenerarToken("sistema-estadisticas", "admin", TimeSpan.FromMinutes(5));
        }

        private string GenerarToken(string subject, string rol, TimeSpan vigencia)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, subject),
                new Claim("rol", rol)
            };

            var token = new JwtSecurityToken(
                issuer: _issuer,
                claims: claims,
                expires: DateTime.UtcNow.Add(vigencia),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
