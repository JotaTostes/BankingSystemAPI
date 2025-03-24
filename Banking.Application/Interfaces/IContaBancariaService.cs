using Banking.Application.DTOs;
using Banking.Domain.Entities;

namespace Banking.Application.Interfaces
{
    public interface IContaBancariaService
    {
        Task<IEnumerable<ContaBancaria>> ObterTodasContasAsync();
        Task<(bool Sucesso, List<string> Erros)> CriarContaAsync(CriarContaBancariaDto criarContaBancaria);
        Task<IEnumerable<ContaBancariaDto>> BuscarContasAsync(string nome, string documento);
        Task<bool> DesativarContaAsync(string documento, string usuarioResponsavel);
    }
}
