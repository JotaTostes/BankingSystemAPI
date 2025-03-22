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
            // Arrange
            var contas = new List<ContaBancaria>
        {
            new ContaBancaria { NomeCliente = "João", Documento = "123" },
            new ContaBancaria { NomeCliente = "Maria", Documento = "456" }
        };

            _serviceMock.Setup(s => s.ObterTodasContasAsync()).ReturnsAsync(contas);

            // Act
            var result = await _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<ContaBancaria>>(okResult.Value);
            Assert.Equal(2, returnValue.Count());
        }

        [Fact]
        public async Task CriarConta_ShouldReturnBadRequest_WhenRequiredFieldsAreMissing()
        {
            // Arrange
            var dto = new CriarContaBancariaDto
            {
                NomeCliente = "",
                Documento = ""
            };

            // Act
            var result = await _controller.CriarConta(dto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Nome do cliente e número do documento são obrigatórios", badRequestResult.Value);
        }

        [Fact]
        public async Task CriarConta_ShouldReturnConflict_WhenDocumentAlreadyExists()
        {
            // Arrange
            var dto = new CriarContaBancariaDto { NomeCliente = "João", Documento = "123" };
            _serviceMock.Setup(s => s.CriarContaAsync(dto)).ReturnsAsync(false);

            // Act
            var result = await _controller.CriarConta(dto);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal("Já existe uma conta com este número de documento", conflictResult.Value);
        }

        [Fact]
        public async Task CriarConta_ShouldReturnCreatedAtAction_WhenAccountIsCreated()
        {
            // Arrange
            var dto = new CriarContaBancariaDto { NomeCliente = "João", Documento = "123" };
            _serviceMock.Setup(s => s.CriarContaAsync(dto)).ReturnsAsync(true);

            // Act
            var result = await _controller.CriarConta(dto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("ObterContas", createdAtActionResult.ActionName);
        }

        [Fact]
        public async Task DesativarConta_ShouldReturnNotFound_WhenAccountDoesNotExist()
        {
            // Arrange
            _serviceMock.Setup(s => s.DesativarContaAsync("123", It.IsAny<string>())).ReturnsAsync(false);

            // Act
            var result = await _controller.DesativarConta("123");

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Conta não encontrada ou já está inativa", notFoundResult.Value);
        }

        [Fact]
        public async Task DesativarConta_ShouldReturnNoContent_WhenAccountIsSuccessfullyDeactivated()
        {
            // Arrange
            _serviceMock.Setup(s => s.DesativarContaAsync("123", It.IsAny<string>())).ReturnsAsync(true);

            // Act
            var result = await _controller.DesativarConta("123");

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }

}