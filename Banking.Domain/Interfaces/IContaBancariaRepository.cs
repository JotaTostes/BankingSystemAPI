using Banking.Domain.Entities;

namespace Banking.Domain.Interfaces
{
    public interface IContaBancariaRepository
    {
        Task<IEnumerable<ContaBancaria>> GetAllAsync();
        Task<ContaBancaria?> GetByDocumentoAsync(string documento);
        Task AddAsync(ContaBancaria conta);
        Task UpdateAsync(ContaBancaria conta);
        Task<IEnumerable<ContaBancaria>> BuscarContasAsync(string nome, string documento);
        Task<bool> DocumentoExisteAsync(string documento);
        Task SalvarAlteracoesAsync();
    }
}
