using Banking.Application.DTOs;
using Banking.Application.Interfaces;
using Banking.Application.Services;
using Banking.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContaBancariaController : ControllerBase
    {
        private readonly IContaBancariaService _service;
        public ContaBancariaController(IContaBancariaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _service.ObterTodasContasAsync());

        [HttpPost]
        public async Task<IActionResult> CriarConta([FromBody] CriarContaBancariaDto criarContaBancariaDto)
        {
            if (string.IsNullOrWhiteSpace(criarContaBancariaDto.NomeCliente) ||
                string.IsNullOrWhiteSpace(criarContaBancariaDto.Documento))
            {
                return BadRequest("Nome do cliente e número do documento são obrigatórios");
            }

            var resultado = await _service.CriarContaAsync(criarContaBancariaDto);

            if (!resultado)
                return Conflict("Já existe uma conta com este número de documento");

            return CreatedAtAction(nameof(ObterContas), new { filtroDocumento = criarContaBancariaDto.Documento }, null);
        }

        [HttpGet]
        public async Task<IActionResult> ObterContas([FromQuery] string? nome, [FromQuery] string? documento)
        {
            var contas = await _service.BuscarContasAsync(nome, documento);
            return Ok(contas);
        }

        [HttpPut("{numeroDocumento}/desativar")]
        public async Task<IActionResult> DesativarConta(string numeroDocumento)
        {
            const string usuarioResponsavel = "AdministradorSistema";

            var resultado = await _service.DesativarContaAsync(numeroDocumento, usuarioResponsavel);

            if (!resultado)
                return NotFound("Conta não encontrada ou já está inativa");

            return NoContent();
        }
    }
}
