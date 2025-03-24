using Banking.Application.DTOs;
using Banking.Domain.Entities;
using Banking.Domain.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Application.Validations
{
    public class TransacaoValidator : AbstractValidator<TransferenciaDto>
    {
        private readonly IContaBancariaRepository _contaBancariaRepository;
        public TransacaoValidator(IContaBancariaRepository contaBancariaRepository)
        {
            _contaBancariaRepository = contaBancariaRepository;

            RuleFor(t => t.NumeroDocumentoOrigem)
                .NotEmpty()
                .WithMessage("O Documento da conta de origem é obrigatório.");

            RuleFor(t => t.NumeroDocumentoDestino)
                .NotEmpty()
                .WithMessage("O Documento da conta de destino é obrigatório.");

            RuleFor(t => t.NumeroDocumentoDestino)
                .NotEqual(t => t.NumeroDocumentoOrigem)
                .WithMessage("A conta de destino não pode ser igual à conta de origem.");

            RuleFor(t => t.Valor)
                .GreaterThan(0)
                .WithMessage("O valor da transferência deve ser maior que zero.");

            RuleFor(t => t)
                .CustomAsync(async (transacao, context, cancellationToken) =>
                {

                    var contaOrigem = await _contaBancariaRepository.GetByDocumentoAsync(transacao.NumeroDocumentoOrigem);
                    var contaDestino = await _contaBancariaRepository.GetByDocumentoAsync(transacao.NumeroDocumentoDestino);

                    // Verificando se as contas existem
                    if (contaOrigem == null)
                        context.AddFailure("NumeroDocumentoOrigem", "A conta de origem não existe.");

                    if (contaDestino == null)
                        context.AddFailure("NumeroDocumentoDestino", "A conta de destino não existe.");

                    // Se alguma das contas não existir, não precisa continuar
                    if (contaOrigem == null || contaDestino == null)
                        return;

                    // Verificando se as contas estão ativas
                    if (contaOrigem.Ativa != true)
                        context.AddFailure("Ativa", "A conta de origem não está ativa.");

                    if (contaDestino.Ativa != true)
                        context.AddFailure("Ativa", "A conta de destino não está ativa.");

                    if (contaOrigem.Saldo < transacao.Valor)
                        context.AddFailure("Valor", "Saldo insuficiente na conta de origem para realizar a transferência.");
                });
        }
    }
}
