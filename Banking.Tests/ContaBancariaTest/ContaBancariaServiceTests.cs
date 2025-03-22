using Banking.Application.DTOs;
using Banking.Application.Services;
using Banking.Domain.Entities;
using Banking.Domain.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Tests.ContaBancariaTest
{
    public class ContaBancariaServiceTests
    {
        private readonly Mock<IContaBancariaRepository> _mockContaBancariaRepository;
        private readonly Mock<IRegistroDesativacaoRepository> _mockRegistroDesativacaoRepository;
        private readonly ContaBancariaService _service;

        public ContaBancariaServiceTests()
        {
            _mockContaBancariaRepository = new Mock<IContaBancariaRepository>();
            _mockRegistroDesativacaoRepository = new Mock<IRegistroDesativacaoRepository>();
            _service = new ContaBancariaService(_mockContaBancariaRepository.Object, _mockRegistroDesativacaoRepository.Object);
        }

        [Fact]
        public async Task ObterTodasContasAsync_DeveRetornarListaDeContas()
        {
            // Arrange
            var contasBancarias = new List<ContaBancaria>
            {
                new ContaBancaria("João", "12345678901"),
                new ContaBancaria("Maria", "98765432100")
            };

            _mockContaBancariaRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(contasBancarias);

            var result = await _service.ObterTodasContasAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task CriarContaAsync_DeveRetornarTrue_QuandoContaNaoExistir()
        {
            var dto = new CriarContaBancariaDto { NomeCliente = "Maria", Documento = "12345678901" };
            _mockContaBancariaRepository.Setup(repo => repo.DocumentoExisteAsync(dto.Documento)).ReturnsAsync(false);
            _mockContaBancariaRepository.Setup(repo => repo.AddAsync(It.IsAny<ContaBancaria>())).Returns(Task.CompletedTask);

            var result = await _service.CriarContaAsync(dto);

            Assert.True(result);
            _mockContaBancariaRepository.Verify(repo => repo.AddAsync(It.IsAny<ContaBancaria>()), Times.Once);
        }

        [Fact]
        public async Task CriarContaAsync_DeveRetornarFalse_QuandoDocumentoExistir()
        {
            var dto = new CriarContaBancariaDto { NomeCliente = "João", Documento = "12345678901" };
            _mockContaBancariaRepository.Setup(repo => repo.DocumentoExisteAsync(dto.Documento)).ReturnsAsync(true);

            var result = await _service.CriarContaAsync(dto);

            Assert.False(result);
            _mockContaBancariaRepository.Verify(repo => repo.AddAsync(It.IsAny<ContaBancaria>()), Times.Never);
        }

        [Fact]
        public async Task BuscarContasAsync_DeveRetornarContasFiltradasCorretamente()
        {
            var contasBancarias = new List<ContaBancaria>
            {
                new ContaBancaria("João Silva", "12345678901") ,
                new ContaBancaria("Maria", "98765432100")
            };
            _mockContaBancariaRepository.Setup(repo => repo.BuscarContasAsync("João", "12345678901")).ReturnsAsync(contasBancarias);

            var result = await _service.BuscarContasAsync("João", "12345678901");

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("João Silva", result.First().NomeCliente);
            Assert.Equal(1000, result.First().Saldo);
        }


        [Fact]
        public async Task DesativarContaAsync_DeveRetornarTrue_QuandoContaForDesativada()
        {
            // Arrange
            var conta = new ContaBancaria("Cliente", "12345678901") { Ativa = true };
            _mockContaBancariaRepository.Setup(repo => repo.GetByDocumentoAsync("12345678901")).ReturnsAsync(conta);
            _mockContaBancariaRepository.Setup(repo => repo.UpdateAsync(conta)).Returns(Task.CompletedTask);
            _mockRegistroDesativacaoRepository.Setup(repo => repo.AdicionarAsync(It.IsAny<RegistroDesativacaoConta>())).Returns(Task.CompletedTask);

            // Act
            var result = await _service.DesativarContaAsync("12345678901", "AdministradorSistema");

            Assert.True(result);
            Assert.False(conta.Ativa);
            _mockRegistroDesativacaoRepository.Verify(repo => repo.AdicionarAsync(It.IsAny<RegistroDesativacaoConta>()), Times.Once);
        }

        [Fact]
        public async Task DesativarContaAsync_DeveRetornarFalse_QuandoContaNaoExistir()
        {
            _mockContaBancariaRepository.Setup(repo => repo.GetByDocumentoAsync("12345678901")).ReturnsAsync((ContaBancaria)null);

            var result = await _service.DesativarContaAsync("12345678901", "AdministradorSistema");

            Assert.False(result);
        }

        [Fact]
        public async Task DesativarContaAsync_DeveRetornarFalse_QuandoContaJaEstiverDesativada()
        {
            var conta = new ContaBancaria("Cliente", "12345678901") { Ativa = false };
            _mockContaBancariaRepository.Setup(repo => repo.GetByDocumentoAsync("12345678901")).ReturnsAsync(conta);

            var result = await _service.DesativarContaAsync("12345678901", "AdministradorSistema");

            Assert.False(result);
        }


    }
}
