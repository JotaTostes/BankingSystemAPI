using Banking.Application.DTOs;
using Banking.Application.Interfaces;
using Banking.Domain.Entities;
using Banking.Domain.Interfaces;
using FluentValidation;

namespace Banking.Application.Services
{
    public class TransacaoService : ITransacaoService
    {
        private readonly ITransacoesRepository _repository;
        private readonly IContaBancariaRepository _contaBancariaRepository;
        private readonly IValidator<TransferenciaDto> _transacaoValidator;

        public TransacaoService(ITransacoesRepository repository,
            IContaBancariaRepository contaBancariaRepository,
            IValidator<TransferenciaDto> transacaoValidator)
        {
            _repository = repository;
            _contaBancariaRepository = contaBancariaRepository;
            _transacaoValidator = transacaoValidator;
        }

        public async Task<(bool Sucesso, List<string> Erros)> TransferirAsync(TransferenciaDto transferenciaDto)
        {
            var validationResult = await _transacaoValidator.ValidateAsync(transferenciaDto);

            if (!validationResult.IsValid)
            {
                return (false, validationResult.Errors.Select(e => e.ErrorMessage).ToList());
            }

            var contaOrigem = await _contaBancariaRepository.GetByDocumentoAsync(transferenciaDto.NumeroDocumentoOrigem);
            var contaDestino = await _contaBancariaRepository.GetByDocumentoAsync(transferenciaDto.NumeroDocumentoDestino);

            contaOrigem.Sacar(transferenciaDto.Valor);
            contaDestino.Depositar(transferenciaDto.Valor);

            var transacao = new Transacao(
                contaOrigem.Id,
                contaDestino.Id,
                transferenciaDto.Valor);

            await _repository.AdicionarAsync(transacao);
            await _repository.SalvarAlteracoesAsync();

            await _contaBancariaRepository.UpdateAsync(contaOrigem);
            await _contaBancariaRepository.UpdateAsync(contaDestino);
            await _contaBancariaRepository.SalvarAlteracoesAsync();

            return (true, new List<string>());
        }

    }
}
