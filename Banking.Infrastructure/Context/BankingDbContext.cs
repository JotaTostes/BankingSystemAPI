using Banking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace Banking.Infrastructure.Context
{
    public class BankingDbContext : DbContext
    {
        public BankingDbContext(DbContextOptions<BankingDbContext> options) : base(options) { }

        public DbSet<ContaBancaria> ContasBancarias { get; set; }
        public DbSet<Transacao> Transacoes { get; set; }
        public DbSet<RegistroDesativacaoConta> RegistrosDesativacao { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContaBancaria>()
                .ToTable("ContasBancarias")
                .HasIndex(a => a.Documento)
                .IsUnique();

            modelBuilder.Entity<Transacao>()
                .ToTable("Transacoes")
                .Property(t => t.Valor)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<RegistroDesativacaoConta>()
                .ToTable("RegistroDesativacaoConta");

            modelBuilder.Entity<ContaBancaria>()
                .Property(a => a.Saldo)
                .HasColumnType("decimal(18,2)");
        }
    }
}
