using Banking.Application.DTOs;

namespace Banking.Application.Interfaces
{
    public interface ITransacaoService
    {
        Task<(bool Sucesso, List<string> Erros)> TransferirAsync(TransferenciaDto transferenciaDto);
    }
}
