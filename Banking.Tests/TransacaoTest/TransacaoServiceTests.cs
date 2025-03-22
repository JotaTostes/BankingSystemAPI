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

namespace Banking.Tests.TransacaoTest
{
    public class TransacaoServiceTests
    {
        private readonly Mock<ITransacoesRepository> _mockTransacoesRepo;
        private readonly Mock<IContaBancariaRepository> _mockContaBancariaRepo;
        private readonly TransacaoService _transacaoService;

        public TransacaoServiceTests()
        {
            _mockTransacoesRepo = new Mock<ITransacoesRepository>();
            _mockContaBancariaRepo = new Mock<IContaBancariaRepository>();
            _transacaoService = new TransacaoService(_mockTransacoesRepo.Object, _mockContaBancariaRepo.Object);
        }

        [Fact]
        public async Task TransferirAsync_DeveRetornarFalseSeContaOrigemNaoExistir()
        {
            // Arrange
            var transferenciaDto = new TransferenciaDto
            {
                NumeroDocumentoOrigem = "123",
                NumeroDocumentoDestino = "456",
                Valor = 100
            };

            _mockContaBancariaRepo.Setup(repo => repo.GetByDocumentoAsync("123"))
                .ReturnsAsync((ContaBancaria)null);

            // Act
            var resultado = await _transacaoService.TransferirAsync(transferenciaDto);

            // Assert
            Assert.False(resultado);
        }

        [Fact]
        public async Task TransferirAsync_DeveRetornarTrueSeTransferenciaForBemSucedida()
        {
            // Arrange
            var transferenciaDto = new TransferenciaDto
            {
                NumeroDocumentoOrigem = "22222222222",
                NumeroDocumentoDestino = "11111111111",
                Valor = 100
            };

            var valorInicialContas = 1000;
            var contaOrigem = new ContaBancaria("João", "22222222222");
            var contaDestino = new ContaBancaria("Maria", "11111111111");

            _mockContaBancariaRepo.Setup(repo => repo.GetByDocumentoAsync("22222222222"))
                .ReturnsAsync(contaOrigem);
            _mockContaBancariaRepo.Setup(repo => repo.GetByDocumentoAsync("11111111111"))
                .ReturnsAsync(contaDestino);
            _mockContaBancariaRepo.Setup(repo => repo.UpdateAsync(It.IsAny<ContaBancaria>()))
                .Returns(Task.CompletedTask);
            _mockTransacoesRepo.Setup(repo => repo.AdicionarAsync(It.IsAny<Transacao>()))
                .Returns(Task.CompletedTask);

            var resultado = await _transacaoService.TransferirAsync(transferenciaDto);

            Assert.True(resultado);
            Assert.Equal(valorInicialContas - transferenciaDto.Valor, contaOrigem.Saldo);
            Assert.Equal(valorInicialContas + transferenciaDto.Valor, contaDestino.Saldo);
        }

        [Fact]
        public async Task TransferirAsync_DeveRetornarFalseSeContaDestinoNaoExistir()
        {
            // Arrange
            var transferenciaDto = new TransferenciaDto
            {
                NumeroDocumentoOrigem = "123",
                NumeroDocumentoDestino = "456",
                Valor = 100
            };

            var contaOrigem = new ContaBancaria("João", "123");
            _mockContaBancariaRepo.Setup(repo => repo.GetByDocumentoAsync("123"))
                .ReturnsAsync(contaOrigem);
            _mockContaBancariaRepo.Setup(repo => repo.GetByDocumentoAsync("456"))
                .ReturnsAsync((ContaBancaria)null); // Simula conta destino inexistente

            // Act
            var resultado = await _transacaoService.TransferirAsync(transferenciaDto);

            // Assert
            Assert.False(resultado);
        }
    }
}
