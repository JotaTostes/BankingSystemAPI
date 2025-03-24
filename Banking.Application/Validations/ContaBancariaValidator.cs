using Banking.Application.DTOs;
using Banking.Domain.Interfaces;
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
                .MustAsync(async (documento, cancellation) =>
                    !await _contaBancariaRepository.DocumentoExisteAsync(documento))
                .WithMessage("Já existe uma conta bancária cadastrada para este documento");
        }


        private bool BeValidDocument(string documento)
        {
            if (string.IsNullOrWhiteSpace(documento))
                return false;

            documento = Regex.Replace(documento, @"[^\d]", "");

            if (documento.Length == 11)
                return IsValidCPF(documento);
            else if (documento.Length == 14)
                return IsValidCNPJ(documento);

            return false;
        }

        private bool IsValidCPF(string cpf)
        {
            bool allDigitsEqual = true;
            for (int i = 1; i < cpf.Length; i++)
            {
                if (cpf[i] != cpf[0])
                {
                    allDigitsEqual = false;
                    break;
                }
            }

            if (allDigitsEqual)
                return false;

            int soma = 0;
            for (int i = 0; i < 9; i++)
                soma += int.Parse(cpf[i].ToString()) * (10 - i);

            int resto = soma % 11;
            int dv1 = resto < 2 ? 0 : 11 - resto;

            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(cpf[i].ToString()) * (11 - i);

            resto = soma % 11;
            int dv2 = resto < 2 ? 0 : 11 - resto;

            return dv1 == int.Parse(cpf[9].ToString()) && dv2 == int.Parse(cpf[10].ToString());
        }

        private bool IsValidCNPJ(string cnpj)
        {
            bool allDigitsEqual = true;
            for (int i = 1; i < cnpj.Length; i++)
            {
                if (cnpj[i] != cnpj[0])
                {
                    allDigitsEqual = false;
                    break;
                }
            }

            if (allDigitsEqual)
                return false;

            int[] multiplicadores1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma = 0;

            for (int i = 0; i < 12; i++)
                soma += int.Parse(cnpj[i].ToString()) * multiplicadores1[i];

            int resto = soma % 11;
            int dv1 = resto < 2 ? 0 : 11 - resto;

            int[] multiplicadores2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            soma = 0;

            for (int i = 0; i < 13; i++)
                soma += int.Parse(cnpj[i < 12 ? i : 12].ToString()) * multiplicadores2[i];

            resto = soma % 11;
            int dv2 = resto < 2 ? 0 : 11 - resto;

            return dv1 == int.Parse(cnpj[12].ToString()) && dv2 == int.Parse(cnpj[13].ToString());
        }
    }
}
