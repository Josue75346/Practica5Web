using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SistemaFarmacia.Models;

namespace SistemaFarmacia.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Estante> Estantes { get; set; }
        public DbSet<Medicamento> Medicamentos { get; set; }
        public DbSet<Auditoria> Auditoria { get; set; }
        public DbSet<Bitacora> Bitacora { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Medicamento>()
                .Property(m => m.Precio)
                .HasPrecision(10, 2);
        }
    }
}