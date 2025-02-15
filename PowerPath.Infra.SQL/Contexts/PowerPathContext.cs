using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PowerPath.Domain.Entities;

namespace PowerPath.Infra.SQL.Contexts;

public partial class PowerPathContext(IConfiguration configuration, DbContextOptions<PowerPathContext> options) : DbContext(options)
{
    private readonly IConfiguration _configuration = configuration;

    public virtual DbSet<Medidor> Medidor { get; set; }

    public virtual DbSet<Usuario> Usuario { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection")!;
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_bin")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Medidor>(entity =>
        {
            entity.HasKey(e => new { e.Instalacao, e.Lote })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("medidor");

            entity.Property(e => e.Instalacao).HasMaxLength(10);
            entity.Property(e => e.Alteracao).HasColumnType("datetime");
            entity.Property(e => e.Criacao).HasColumnType("datetime");
            entity.Property(e => e.Excluido)
                .HasDefaultValueSql("b'0'")
                .HasColumnType("bit(1)");
            entity.Property(e => e.Fabricante).HasMaxLength(15);
            entity.Property(e => e.Operadora).HasMaxLength(5);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("usuario")
                .UseCollation("utf8mb4_bin");

            entity.Property(e => e.Nome).HasMaxLength(50);
            entity.Property(e => e.Senha).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
