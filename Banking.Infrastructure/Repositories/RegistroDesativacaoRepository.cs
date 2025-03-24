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
    public  class RegistroDesativacaoRepository : IRegistroDesativacaoRepository
    {
        private readonly BankingDbContext _context;
        public RegistroDesativacaoRepository(BankingDbContext context)
        {
            _context = context;
        }

        public async Task AdicionarAsync(RegistroDesativacaoConta registro)
        {
            await _context.RegistrosDesativacao.AddAsync(registro);
        }

        public async Task SalvarAlteracoesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
