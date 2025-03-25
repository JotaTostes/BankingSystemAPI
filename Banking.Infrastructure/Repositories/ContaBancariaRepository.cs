using Banking.Domain.Entities;
using Banking.Domain.Interfaces;
using Banking.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Banking.Infrastructure.Repositories
{
    public class ContaBancariaRepository : IContaBancariaRepository
    {
        private readonly BankingDbContext _context;
        public ContaBancariaRepository(BankingDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ContaBancaria>> GetAllAsync() =>
            await _context.ContasBancarias.ToListAsync();

        public async Task<ContaBancaria?> GetByDocumentoAsync(string documento) =>
            await _context.ContasBancarias.FirstOrDefaultAsync(c => c.Documento == documento);

        public ContaBancaria? GetByDocumento(string documento) =>
            _context.ContasBancarias.FirstOrDefault(c => c.Documento == documento);

        public async Task AddAsync(ContaBancaria conta)
        {
            await _context.ContasBancarias.AddAsync(conta);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ContaBancaria conta)
        {
            _context.ContasBancarias.Update(conta);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<ContaBancaria>> BuscarContasAsync(string nome, string documento)
        {
            var query = _context.ContasBancarias.AsQueryable();

            if (!string.IsNullOrWhiteSpace(nome))
            {
                query = query.Where(c => c.NomeCliente.Contains(nome));
            }

            if (!string.IsNullOrWhiteSpace(documento))
            {
                query = query.Where(c => c.Documento == documento);
            }

            return await query.ToListAsync();
        }

        public async Task SalvarAlteracoesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public bool DocumentoExiste(string documento)
        {
            return _context.ContasBancarias
                .Any(a => a.Documento == documento);
        }
    }
}
