using Banking.Application.DTOs;

namespace Banking.Application.Interfaces
{
    public interface ITransacaoService
    {
        Task<bool> TransferirAsync(TransferenciaDto transferenciaDto);
    }
}
