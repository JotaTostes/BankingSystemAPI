using Banking.Application.DTOs;
using Banking.Application.Services;
using Banking.Application.Validations;
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
        private readonly TransacaoService _service;
        private readonly TransacaoValidator _mockTransacaoValidator;

        public TransacaoServiceTests()
        {
            _mockTransacoesRepo = new Mock<ITransacoesRepository>();
            _mockContaBancariaRepo = new Mock<IContaBancariaRepository>();
            _mockTransacaoValidator = new TransacaoValidator(_mockContaBancariaRepo.Object);

            _service = new TransacaoService(
                _mockTransacoesRepo.Object,
                _mockContaBancariaRepo.Object,
                _mockTransacaoValidator
            );
        }

        [Fact]
        public async Task TransferirAsync_DeveRetornarFalseSeContaOrigemNaoExistir()
        {
            // Arrange
            var transferenciaDto = new TransferenciaDto
            {
                NumeroDocumentoOrigem = "85163884093",
                NumeroDocumentoDestino = "61461113091",
                Valor = 100
            };

            _mockContaBancariaRepo.Setup(repo => repo.GetByDocumentoAsync("85163884093"))
                .ReturnsAsync((ContaBancaria)null);

            var (sucesso, erros) = await _service.TransferirAsync(transferenciaDto);

            Assert.False(sucesso);
            Assert.Contains("A conta de origem não existe.", erros);
        }

        [Fact]
        public async Task TransferirAsync_DeveRetornarTrueSeTransferenciaForBemSucedida()
        {
            // Arrange
            var transferenciaDto = new TransferenciaDto
            {
                NumeroDocumentoOrigem = "26848139068",
                NumeroDocumentoDestino = "27816465041",
                Valor = 100
            };

            var valorInicialContas = 1000;
            var contaOrigem = new ContaBancaria("João", "26848139068")
            {
                Saldo = valorInicialContas
            };
            var contaDestino = new ContaBancaria("Maria", "27816465041")
            {
                Saldo = valorInicialContas
            };

            _mockContaBancariaRepo.Setup(repo => repo.GetByDocumentoAsync("26848139068"))
                .ReturnsAsync(contaOrigem);
            _mockContaBancariaRepo.Setup(repo => repo.GetByDocumentoAsync("27816465041"))
                .ReturnsAsync(contaDestino);
            _mockContaBancariaRepo.Setup(repo => repo.UpdateAsync(It.IsAny<ContaBancaria>()))
                .Returns(Task.CompletedTask);
            _mockTransacoesRepo.Setup(repo => repo.AdicionarAsync(It.IsAny<Transacao>()))
                .Returns(Task.CompletedTask);

            var (sucesso, erros) = await _service.TransferirAsync(transferenciaDto);

            Assert.True(sucesso);
            Assert.Empty(erros);
            Assert.Equal(valorInicialContas - transferenciaDto.Valor, contaOrigem.Saldo);
            Assert.Equal(valorInicialContas + transferenciaDto.Valor, contaDestino.Saldo);
        }

        [Fact]
        public async Task TransferirAsync_DeveRetornarFalseSeContaDestinoNaoExistir()
        {
            var transferenciaDto = new TransferenciaDto
            {
                NumeroDocumentoOrigem = "26848139068",
                NumeroDocumentoDestino = "27816465041",
                Valor = 100
            };

            var contaOrigem = new ContaBancaria("João", "123");
            _mockContaBancariaRepo.Setup(repo => repo.GetByDocumentoAsync("123"))
                .ReturnsAsync(contaOrigem);
            _mockContaBancariaRepo.Setup(repo => repo.GetByDocumentoAsync("456"))
                .ReturnsAsync((ContaBancaria)null);

            var (sucesso, erros) = await _service.TransferirAsync(transferenciaDto);

            Assert.False(sucesso);
            Assert.Contains("A conta de destino não existe.", erros);
        }
    }
}
