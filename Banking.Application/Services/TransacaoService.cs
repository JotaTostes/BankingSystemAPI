using Banking.Application.DTOs;
using Banking.Application.Interfaces;
using Banking.Domain.Entities;
using Banking.Domain.Interfaces;

namespace Banking.Application.Services
{
    public class TransacaoService : ITransacaoService
    {
        private readonly ITransacoesRepository _repository;
        private readonly IContaBancariaRepository _contaBancariaRepository;

        public TransacaoService(ITransacoesRepository repository, IContaBancariaRepository contaBancariaRepository)
        {
            _repository = repository;
            _contaBancariaRepository = contaBancariaRepository;
        }

        public async Task<bool> TransferirAsync(TransferenciaDto transferenciaDto)
        {
            var contaOrigem = await _contaBancariaRepository.GetByDocumentoAsync(transferenciaDto.NumeroDocumentoOrigem);
            var contaDestino = await _contaBancariaRepository.GetByDocumentoAsync(transferenciaDto.NumeroDocumentoDestino);

            // Valida as contas
            if (contaOrigem == null || contaDestino == null)
                return false;

            if (!contaOrigem.Ativa || !contaDestino.Ativa)
                return false;

            if (!contaOrigem.PodeSacar(transferenciaDto.Valor))
                return false;

            contaOrigem.Sacar(transferenciaDto.Valor);
            contaDestino.Depositar(transferenciaDto.Valor);

            var transacao = new Transacao(
                contaOrigem.Id,
                contaDestino.Id,
                transferenciaDto.Valor);

            await _repository.AdicionarAsync(transacao);

            await _contaBancariaRepository.UpdateAsync(contaOrigem);
            await _contaBancariaRepository.UpdateAsync(contaDestino);

            await _contaBancariaRepository.SalvarAlteracoesAsync();
            await _repository.SalvarAlteracoesAsync();

            return true;
        }
    }
}
