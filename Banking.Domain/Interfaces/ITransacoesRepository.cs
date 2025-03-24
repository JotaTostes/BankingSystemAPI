using Banking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Domain.Interfaces
{
    public interface ITransacoesRepository
    {
        Task AdicionarAsync(Transacao transacao);
        Task SalvarAlteracoesAsync();
    }
}
