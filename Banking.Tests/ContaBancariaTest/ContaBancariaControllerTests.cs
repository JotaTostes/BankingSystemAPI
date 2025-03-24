using Banking.Application.DTOs;
using Banking.Application.Interfaces;
using Banking.Domain.Entities;
using BankingAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Banking.Tests.ContaBancariaTest
{
    public class ContaBancariaControllerTests
    {
        private readonly Mock<IContaBancariaService> _serviceMock;
        private readonly ContaBancariaController _controller;

        public ContaBancariaControllerTests()
        {
            _serviceMock = new Mock<IContaBancariaService>();
            _controller = new ContaBancariaController(_serviceMock.Object);
        }

        [Fact]
        public async Task Get_ShouldReturnOk_WhenAccountsExist()
        {
            var contas = new List<ContaBancaria>
        {
            new ContaBancaria { NomeCliente = "João", Documento = "123" },
            new ContaBancaria { NomeCliente = "Maria", Documento = "456" }
        };

            _serviceMock.Setup(s => s.ObterTodasContasAsync()).ReturnsAsync(contas);

            var result = await _controller.Get();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<ContaBancaria>>(okResult.Value);
            Assert.Equal(2, returnValue.Count());
        }

        [Fact]
        public async Task CriarConta_ShouldReturnBadRequest_WhenRequiredFieldsAreMissing()
        {
            var dto = new CriarContaBancariaDto
            {
                NomeCliente = "",
                Documento = ""   
            };

            var erros = new List<string>
            {
                "O nome do cliente é obrigatório",
                "O documento do cliente é obrigatório"
            };

            _serviceMock.Setup(s => s.CriarContaAsync(dto)).ReturnsAsync((false, erros));

            var result = await _controller.CriarConta(dto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = badRequestResult.Value;
            var mensagem = response.GetType().GetProperty("Mensagem")?.GetValue(response, null);
            var errosRetornados = response.GetType().GetProperty("Erros")?.GetValue(response, null) as List<string>;

            Assert.Contains("O nome do cliente é obrigatório", errosRetornados);
            Assert.Contains("O documento do cliente é obrigatório", errosRetornados);
        }

        [Fact]
        public async Task CriarConta_ShouldReturnBadRequest_WhenDocumentAlreadyExists()
        {
            var dto = new CriarContaBancariaDto { NomeCliente = "João", Documento = "38726453789" };
            var erros = new List<string> { "Já existe uma conta com este número de documento." };

            _serviceMock.Setup(s => s.CriarContaAsync(dto)).ReturnsAsync((false, erros));

            var result = await _controller.CriarConta(dto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);

            var response = badRequestResult.Value;

            var mensagem = response.GetType().GetProperty("Mensagem")?.GetValue(response, null);
            var errosRetornados = response.GetType().GetProperty("Erros")?.GetValue(response, null) as List<string>;

            Assert.NotNull(mensagem);
            Assert.Equal("Erro ao criar conta.", mensagem);

            Assert.NotNull(errosRetornados);
            Assert.Contains("Já existe uma conta com este número de documento.", errosRetornados);
        }

        [Fact]
        public async Task CriarConta_ShouldReturnCreatedAtAction_WhenAccountIsCreated()
        {
            var dto = new CriarContaBancariaDto { NomeCliente = "João", Documento = "123" };
            _serviceMock.Setup(s => s.CriarContaAsync(dto)).ReturnsAsync((true, new List<string>()));

            var result = await _controller.CriarConta(dto);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("ObterContas", createdAtActionResult.ActionName);
        }

        [Fact]
        public async Task DesativarConta_ShouldReturnNotFound_WhenAccountDoesNotExist()
        {
            _serviceMock.Setup(s => s.DesativarContaAsync("123", It.IsAny<string>())).ReturnsAsync(false);

            var result = await _controller.DesativarConta("123");

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Conta não encontrada ou já está inativa", notFoundResult.Value);
        }

        [Fact]
        public async Task DesativarConta_ShouldReturnNoContent_WhenAccountIsSuccessfullyDeactivated()
        {
            _serviceMock.Setup(s => s.DesativarContaAsync("123", It.IsAny<string>())).ReturnsAsync(true);

            var result = await _controller.DesativarConta("123");

            Assert.IsType<NoContentResult>(result);
        }
    }

}