using EstadisticasAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EstadisticasAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Seleccion> Selecciones { get; set; }
        public DbSet<Sede> Sedes { get; set; }
        public DbSet<Partido> Partidos { get; set; }
        public DbSet<Auditoria> Auditorias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Partido>()
                .HasOne(p => p.SeleccionLocal)
                .WithMany()
                .HasForeignKey(p => p.SeleccionLocalId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Partido>()
                .HasOne(p => p.SeleccionVisitante)
                .WithMany()
                .HasForeignKey(p => p.SeleccionVisitanteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.NombreUsuario)
                .IsUnique();
        }
    }
}