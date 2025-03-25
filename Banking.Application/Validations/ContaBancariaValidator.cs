using Banking.Application.DTOs;
using Banking.Domain.Interfaces;
using Banking.Shared.Util;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Banking.Shared.Validations
{
    public class ContaBancariaValidator : AbstractValidator<CriarContaBancariaDto>
    {
        private readonly IContaBancariaRepository _contaBancariaRepository;

        public ContaBancariaValidator(IContaBancariaRepository contaBancariaRepository)
        {
            _contaBancariaRepository = contaBancariaRepository;

            RuleFor(c => c.NomeCliente)
                .NotEmpty().WithMessage("O nome do cliente é obrigatório")
                .Length(2, 100).WithMessage("O nome do cliente deve ter entre 2 e 100 caracteres");

            RuleFor(c => c.Documento)
                .NotEmpty().WithMessage("O documento do cliente é obrigatório")
                .Must(BeValidDocument).WithMessage("O documento informado não é válido")
                .Must(documento => !_contaBancariaRepository.DocumentoExiste(documento))
                .WithMessage("Já existe uma conta bancária cadastrada para este documento");
        }

        private bool BeValidDocument(string documento)
        {
            if (string.IsNullOrWhiteSpace(documento))
                return false;

            documento = Regex.Replace(documento, @"[^\d]", "");

            if (documento.Length == 11)
                return DocumentosUtil.IsValidCPF(documento);
            else if (documento.Length == 14)
                return DocumentosUtil.IsValidCNPJ(documento);

            return false;
        }
    }
}
