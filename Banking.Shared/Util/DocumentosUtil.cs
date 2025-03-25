using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Banking.Shared.Util
{
    public class DocumentosUtil
    {
        /// <summary>
        /// Retorna CPF ou CNPJ sem caracteres especiais
        /// </summary>
        /// <param name="numeroDocumento"></param>
        /// <returns></returns>
        public static string RemoverCaracteresEspeciais(string numeroDocumento)
        {
            if (string.IsNullOrWhiteSpace(numeroDocumento))
                return string.Empty;

            return Regex.Replace(numeroDocumento, @"[^\d]", "");
        }

        /// <summary>
        /// Função que valida um CPF
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns></returns>
        public static bool IsValidCPF(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf) || cpf.Length != 11 || !Regex.IsMatch(cpf, @"^\d{11}$"))
                return false;

            if (AllDigitsEqual(cpf))
                return false;

            var (firstDigit, secondDigit) = CalculateDigitCPF(cpf, 9);
            return cpf[9] == (firstDigit + '0') && cpf[10] == (secondDigit + '0');
        }

        /// <summary>
        /// Verifica se todos os dígitos são iguais
        /// </summary>
        /// <param name="cpfOuCnpj"></param>
        /// <returns></returns>
        private static bool AllDigitsEqual(string cpfOuCnpj) => cpfOuCnpj.All(d => d == cpfOuCnpj[0]);

        /// <summary>
        /// Calcula um dígito verificador CPF
        /// </summary>
        /// <param name="numero"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private static (int firstDigit, int secondDigit) CalculateDigitCPF(string number, int length)
        {
            int[] multiplicadoresCPF1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicadoresCPF2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            int soma = number.Take(length)
                             .Select((digit, index) => (digit - '0') * multiplicadoresCPF1[index])
                             .Sum();

            int resto = soma % 11;
            int firstDigit = (resto < 2) ? 0 : 11 - resto;

            soma = number.Take(length + 1)
                         .Select((digit, index) => (digit - '0') * multiplicadoresCPF2[index])
                         .Sum();

            resto = soma % 11;
            int secondDigit = (resto < 2) ? 0 : 11 - resto;

            return (firstDigit, secondDigit);
        }

        /// <summary>
        /// Calcula um dígito verificador CNPJ
        /// </summary>
        /// <param name="number"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private static (int firstDigit, int secondDigit) CalculateDigitCNPJ(string number, int length)
        {
            int[] multiplicadoresCNPJ1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicadoresCNPJ2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            int soma = number.Take(length)
                             .Select((digit, index) => (digit - '0') * multiplicadoresCNPJ1[index])
                             .Sum();

            int resto = soma % 11;
            int firstDigit = (resto < 2) ? 0 : 11 - resto;

            soma = number.Take(length + 1)
                         .Select((digit, index) => (digit - '0') * multiplicadoresCNPJ2[index])
                         .Sum();

            resto = soma % 11;
            int secondDigit = (resto < 2) ? 0 : 11 - resto;

            return (firstDigit, secondDigit);
        }


        /// <summary>
        /// Função que valida um CNPJ
        /// </summary>
        /// <param name="cnpj"></param>
        /// <returns></returns>
        public static bool IsValidCNPJ(string cnpj)
        {
            if (string.IsNullOrWhiteSpace(cnpj) || cnpj.Length != 14 || !Regex.IsMatch(cnpj, @"^\d{14}$"))
                return false;

            if (AllDigitsEqual(cnpj))
                return false;

            var (firstDigit, secondDigit) = CalculateDigitCNPJ(cnpj, 12);

            return cnpj[12] == (firstDigit + '0') && cnpj[13] == (secondDigit + '0');
        }
    }
}
