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
                .HasIndex(a => a.Documento)
                .IsUnique();
        }
    }
}
