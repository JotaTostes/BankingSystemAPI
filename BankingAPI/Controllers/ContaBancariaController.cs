using Banking.Application.DTOs;
using Banking.Application.Interfaces;
using Banking.Domain.Entities;
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

        /// <summary>
        /// Obtém todas as contas bancárias
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ContaBancaria>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get() => Ok(await _service.ObterTodasContasAsync());

        /// <summary>
        /// Cria uma nova conta bancária
        /// </summary>
        /// <param name="criarContaBancariaDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<ContaBancariaDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CriarConta([FromBody] CriarContaBancariaDto criarContaBancariaDto)
        {
            var (sucesso, erros) = await _service.CriarContaAsync(criarContaBancariaDto);

            if (!sucesso)
            {
                if (erros.Any())
                    return BadRequest(new { Mensagem = "Erro ao criar conta.", Erros = erros });
            }

            return CreatedAtAction(nameof(ObterContas), new { filtroDocumento = criarContaBancariaDto.Documento }, null);
        }

        /// <summary>
        /// Obtém contas bancárias com base em filtros
        /// </summary>
        /// <param name="nome"></param>
        /// <param name="documento"></param>
        /// <returns></returns>
        [HttpGet("ContasByFiltro")]
        [ProducesResponseType(typeof(IEnumerable<ContaBancariaDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ObterContas([FromQuery] string? nome, [FromQuery] string? documento)
        {
            var contas = await _service.BuscarContasAsync(nome, documento);
            return Ok(contas);
        }

        /// <summary>
        /// Desativa uma conta bancária
        /// </summary>
        /// <param name="numeroDocumento"></param>
        /// <returns></returns>
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
