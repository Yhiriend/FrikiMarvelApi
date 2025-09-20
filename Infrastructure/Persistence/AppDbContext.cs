using Microsoft.EntityFrameworkCore;
using FrikiMarvelApi.Domain.Entities;

namespace FrikiMarvelApi.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Character> Characters { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<ComicFavorite> ComicFavorites { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de la entidad Character
            modelBuilder.Entity<Character>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.ImageUrl).HasMaxLength(500);
                entity.Property(e => e.MarvelId).IsRequired().HasMaxLength(50);
                
                // Índices para mejorar el rendimiento
                entity.HasIndex(e => e.MarvelId).IsUnique();
                entity.HasIndex(e => e.Name);
                entity.HasIndex(e => e.IsActive);
            });

            // Configuración de la entidad User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Identification).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.Property(e => e.PasswordHash).IsRequired();
                
                // Índices únicos para email e identificación
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.Identification).IsUnique();
                entity.HasIndex(e => e.IsActive);
            });

            // Configuración de la entidad ComicFavorite
            modelBuilder.Entity<ComicFavorite>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.ComicData).IsRequired();
                entity.Property(e => e.AddedDate).IsRequired();
                
                // Relación con User
                entity.HasOne(e => e.User)
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
                
                // Índices para mejorar el rendimiento
                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.IsActive);
                entity.HasIndex(e => e.AddedDate);
                
                // Índice compuesto para búsquedas eficientes
                entity.HasIndex(e => new { e.UserId, e.IsActive });
            });
        }
    }
}
