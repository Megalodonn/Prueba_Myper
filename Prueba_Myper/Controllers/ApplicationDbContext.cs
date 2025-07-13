using Microsoft.EntityFrameworkCore;
using Prueba_Myper.Models;

namespace Prueba_Myper.Controllers
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Trabajador> Trabajadores { get; set; }
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Provincia> Provincias { get; set; }
        public DbSet<Distrito> Distritos { get; set; }

        public DbSet<TrabajadorDto> TrabajadorListado { get; set; } // No mapeada a tabla

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TrabajadorDto>().HasNoKey().ToView(null);

            modelBuilder.Entity<Trabajador>(entity =>
            {
                entity.ToTable("Trabajadores");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.TipoDocumento);
                entity.Property(e => e.NumeroDocumento);
                entity.Property(e => e.Nombres);
                entity.Property(e => e.Sexo);
                entity.Property(e => e.IdDepartamento);
                entity.Property(e => e.IdProvincia);
                entity.Property(e => e.IdDistrito);
            });

            modelBuilder.Entity<Departamento>(entity =>
            {
                entity.ToTable("Departamento");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.NombreDepartamento).HasColumnName("nombreDepartamento");
            });

            modelBuilder.Entity<Provincia>(entity =>
            {
                entity.ToTable("Provincia");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.IdDepartamento);
                entity.Property(e => e.NombreProvincia); //.HasColumnName("nombreProvincia");
            });

            modelBuilder.Entity<Distrito>(entity =>
            {
                entity.ToTable("Distrito");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.IdProvincia);
                entity.Property(e => e.NombreDistrito).HasColumnName("nombreDistrito");
            });
        }
    }
}
