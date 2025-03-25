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
                .Custom((transacao, context) =>
                {

                    var contaOrigem =  _contaBancariaRepository.GetByDocumentoAsync(transacao.NumeroDocumentoOrigem).Result;
                    var contaDestino =  _contaBancariaRepository.GetByDocumentoAsync(transacao.NumeroDocumentoDestino).Result;

                    if (contaOrigem == null)
                        context.AddFailure("NumeroDocumentoOrigem", "A conta de origem não existe.");

                    if (contaDestino == null)
                        context.AddFailure("NumeroDocumentoDestino", "A conta de destino não existe.");

                    if (contaOrigem == null || contaDestino == null)
                        return;

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
