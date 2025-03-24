using Banking.Application.DTOs;
using Banking.Application.Interfaces;
using BankingAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Tests.TransacaoTest
{
    public class TransacaoControllerTests
    {
        private readonly Mock<ITransacaoService> _mockService;
        private readonly TransacaoController _controller;

        public TransacaoControllerTests()
        {
            _mockService = new Mock<ITransacaoService>();
            _controller = new TransacaoController(_mockService.Object);
        }

        [Fact]
        public async Task Transferir_DeveRetornarBadRequest_QuandoValorMenorOuIgualAZero()
        {
            var transferenciaDto = new TransferenciaDto
            {
                NumeroDocumentoOrigem = "95962559088",
                NumeroDocumentoDestino = "04503736060",
                Valor = 0
            };

            var resultado = await _controller.Transferir(transferenciaDto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(resultado);
            Assert.Equal("O valor da transferência deve ser maior que zero.", badRequestResult.Value);
        }

        [Fact]
        public async Task Transferir_DeveRetornarOk_QuandoTransferenciaBemSucedida()
        {
            var transferenciaDto = new TransferenciaDto
            {
                NumeroDocumentoOrigem = "24214333047",
                NumeroDocumentoDestino = "53040707043",
                Valor = 100
            };

            _mockService.Setup(s => s.TransferirAsync(transferenciaDto))
                .ReturnsAsync((true, new List<string>()));

            var resultado = await _controller.Transferir(transferenciaDto);

            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal("Transferência concluída com sucesso.", okResult.Value);
        }

        [Fact]
        public async Task Transferir_DeveRetornarBadRequest_QuandoTransferenciaFalhar()
        {
            var transferenciaDto = new TransferenciaDto
            {
                NumeroDocumentoOrigem = "53040707043",
                NumeroDocumentoDestino = "24214333047",
                Valor = 100
            };

            var erros = new List<string>
            {
                "Conta de origem não encontrada.",
                "Saldo insuficiente."
            };

            _mockService.Setup(s => s.TransferirAsync(transferenciaDto))
                .ReturnsAsync((false, erros));

            var resultado = await _controller.Transferir(transferenciaDto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(resultado);
            var mensagemErro = Assert.IsType<List<string>>(badRequestResult.Value);
            Assert.Equal(erros, mensagemErro);
        }
    }
}
