using Banking.Domain.Entities;
using Banking.Domain.Interfaces;
using Banking.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Infrastructure.Repositories
{
    public  class TransacoesRepository : ITransacoesRepository
    {
        private readonly BankingDbContext _context;

        public TransacoesRepository(BankingDbContext context)
        {
            _context = context;
        }

        public async Task AdicionarAsync(Transacao transacao)
        {
            await _context.Transacoes.AddAsync(transacao);
        }

        public async Task SalvarAlteracoesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
