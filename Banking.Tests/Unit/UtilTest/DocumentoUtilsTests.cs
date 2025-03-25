using Banking.Shared.Util;
using FluentAssertions;

namespace Banking.Tests.Unit.UtilTest
{
    public class DocumentoUtilsTests
    {
        [Theory]
        [InlineData("123.456.789-01", "12345678901")]
        [InlineData("12.345.678/0001-99", "12345678000199")]
        [InlineData("12345678901", "12345678901")]
        [InlineData("  ", "")]
        [InlineData(null, "")]
        public void RemoverCaracteresEspeciais_DeveRetornarSomenteNumeros(string input, string expected)
        {
            var resultado = DocumentosUtil.RemoverCaracteresEspeciais(input);
            resultado.Should().Be(expected);
        }

        #region CPF
        [Fact]
        public void IsValidCPF_ValidCPF_ReturnsTrue()
        {
            string cpf = "76319507098";
            bool result = DocumentosUtil.IsValidCPF(cpf);
            Assert.True(result);
        }

        [Fact]
        public void IsValidCPF_InvalidCPF_AllDigitsEqual_ReturnsFalse()
        {
            string cpf = "11111111111";
            bool result = DocumentosUtil.IsValidCPF(cpf);
            Assert.False(result);
        }

        [Fact]
        public void IsValidCPF_InvalidCPF_IncorrectCheckDigits_ReturnsFalse()
        {
            string cpf = "12345678900";
            bool result = DocumentosUtil.IsValidCPF(cpf);
            Assert.False(result);
        }
        #endregion

        #region CNPJ
        [Fact]
        public void IsValidCNPJ_ValidCNPJ_ReturnsTrue()
        {
            string cnpj = "92022716000108";
            bool result = DocumentosUtil.IsValidCNPJ(cnpj);
            Assert.True(result);
        }

        [Fact]
        public void IsValidCNPJ_InvalidCNPJ_AllDigitsEqual_ReturnsFalse()
        {
            string cnpj = "11111111111111";
            bool result = DocumentosUtil.IsValidCNPJ(cnpj);
            Assert.False(result);
        }

        [Fact]
        public void IsValidCNPJ_InvalidCNPJ_IncorrectCheckDigits_ReturnsFalse()
        {
            string cnpj = "12345678000100";
            bool result = DocumentosUtil.IsValidCNPJ(cnpj);
            Assert.False(result);
        }
        #endregion
    }
}
