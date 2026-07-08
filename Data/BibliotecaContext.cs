using Biblioteca.Models;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Data
{
    public class BibliotecaContext : DbContext
    {
        public BibliotecaContext(DbContextOptions<BibliotecaContext> options)
            : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Livro> Livros { get; set; }
        public DbSet<Exemplar> Exemplares { get; set; }
        public DbSet<Emprestimo> Emprestimos { get; set; }
        public DbSet<Leitor> Leitores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
                entity.Property(e => e.SenhaHash).IsRequired();
                entity.Property(e => e.Role).IsRequired().HasMaxLength(20);
            });

            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<Livro>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Titulo).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Autor).IsRequired().HasMaxLength(150);
                entity.Property(e => e.ImagemUrl).HasMaxLength(500);

                entity.HasOne(e => e.Categoria)
                      .WithMany(c => c.Livros)
                      .HasForeignKey(e => e.CategoriaId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Exemplar>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Tombo).IsUnique();
                entity.Property(e => e.Tombo).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Situacao).HasMaxLength(30);
                entity.Property(e => e.Disponivel).HasDefaultValue(true);

                entity.HasOne(e => e.Livro)
                      .WithMany(l => l.Exemplares)
                      .HasForeignKey(e => e.LivroId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Leitor>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Cpf).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Cpf).IsRequired().HasMaxLength(14);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Telefone).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Endereco).HasMaxLength(200);
                entity.Property(e => e.Ativo).HasDefaultValue(true);
            });

            modelBuilder.Entity<Emprestimo>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Status).IsRequired().HasMaxLength(20);

                entity.HasOne(e => e.Livro)
                      .WithMany()
                      .HasForeignKey(e => e.LivroId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Exemplar)
                      .WithMany(ex => ex.Emprestimos)
                      .HasForeignKey(e => e.ExemplarId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Leitor)
                      .WithMany(l => l.Emprestimos)
                      .HasForeignKey(e => e.LeitorId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
