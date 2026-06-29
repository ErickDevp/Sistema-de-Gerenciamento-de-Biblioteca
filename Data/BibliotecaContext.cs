using BibliotecaMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaMVC.Data
{
    public class BibliotecaContext : DbContext
    {
        public BibliotecaContext(DbContextOptions<BibliotecaContext> options)
            : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Livro> Livros { get; set; }
        public DbSet<Emprestimo> Emprestimos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Usuario
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
                entity.Property(e => e.SenhaHash).IsRequired();
                entity.Property(e => e.Role).IsRequired().HasMaxLength(20);
            });

            // Categoria
            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
            });

            // Livro
            modelBuilder.Entity<Livro>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Titulo).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Autor).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Disponivel).HasDefaultValue(true);

                entity.HasOne(e => e.Categoria)
                      .WithMany(c => c.Livros)
                      .HasForeignKey(e => e.CategoriaId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Emprestimo
            modelBuilder.Entity<Emprestimo>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.NomeLeitor).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Status).IsRequired().HasMaxLength(20);

                entity.HasOne(e => e.Livro)
                      .WithMany(l => l.Emprestimos)
                      .HasForeignKey(e => e.LivroId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
